﻿using CrashNSaneLoadDetector;
using LiveSplit.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using CrashNSTLoadRemoval.Memory;

namespace LiveSplit.UI.Components
{
	

	public partial class CrashNSTLoadRemovalSettings : UserControl
	{
		#region Public Fields

		public bool AutoSplitterEnabled = false;

		public bool AutoSplitterDisableOnSkipUntilSplit = false;

		public bool RemoveFadeouts = false;
    public bool RemoveFadeins = false;

    public bool SaveDetectionLog = false;

    public int AverageBlackLevel = -1;

    public string DetectionLogFolderName = "CrashNSTLoadRemovalLog";

    //Number of frames to wait for a change from load -> running and vice versa.
    public int AutoSplitterJitterToleranceFrames = 8;

		//If you split manually during "AutoSplitter" mode, I ignore AutoSplitter-splits for 50 frames. (A little less than 2 seconds)
		//This means that if a split would happen during these frames, it is ignored.
		public int AutoSplitterManualSplitDelayFrames = 50;

		#endregion Public Fields

		#region Private Fields

		private AutoSplitData autoSplitData = null;

		private float captureAspectRatioX = 16.0f;

		private float captureAspectRatioY = 9.0f;

		private List<string> captureIDs = null;

		private Size captureSize = new Size(300, 100);

		private float cropOffsetX = 0.0f;

		private float cropOffsetY = -40.0f;

		private bool drawingPreview = false;

		private List<Control> dynamicAutoSplitterControls;

		private float featureVectorResolutionX = 1920.0f;

		private float featureVectorResolutionY = 1080.0f;

		private ImageCaptureInfo imageCaptureInfo;

		private Bitmap lastDiagnosticCapture = null;

		private List<int> lastFeatures = null;

		private Bitmap lastFullCapture = null;

		private Bitmap lastFullCroppedCapture = null;

		private int lastMatchingBins = 0;

		private LiveSplitState liveSplitState = null;

		//private string DiagnosticsFolderName = "CrashNSTDiagnostics/";
		private int numCaptures = 0;

		private int numScreens = 1;

		private Dictionary<string, XmlElement> AllGameAutoSplitSettings;

		private Bitmap previewImage = null;

		//-1 -> full screen, otherwise index process list
		private int processCaptureIndex = -1;

		private Process[] processList;
		private int scalingValue = 100;
		private float scalingValueFloat = 1.0f;
		private string selectedCaptureID = "";
		private Point selectionBottomRight = new Point(0, 0);
		private Rectangle selectionRectanglePreviewBox;
		private Point selectionTopLeft = new Point(0, 0);
        private CrashMemory memory;
        private Boolean isLoading;

        #endregion Private Fields

        #region Public Constructors

        public CrashNSTLoadRemovalSettings(CrashMemory mem, LiveSplitState state)
        {
            memory = mem;
            InitializeComponent();

      //RemoveFadeins = chkRemoveFadeIns.Checked;

            //memory.Loading.OnValueChange += Crash_OnLoadingChanged;

            AllGameAutoSplitSettings = new Dictionary<string, XmlElement>();
			dynamicAutoSplitterControls = new List<Control>();
			CreateAutoSplitControls(state);
			liveSplitState = state;
			//processListComboBox.SelectedIndex = 0;
			lblVersion.Text = "v" + Assembly.GetExecutingAssembly().GetName().Version.ToString(3);


            RefreshCaptureWindowList();
			//processListComboBox.SelectedIndex = 0;
		}

		#endregion Public Constructors

		#region Public Methods
        
        public void initProccessesList()
        {
            memory.GetProcesses();
        }

		public void ChangeAutoSplitSettingsToGameName(string gameName, string category)
		{
			gameName = removeInvalidXMLCharacters(gameName);
			category = removeInvalidXMLCharacters(category);

			//TODO: go through gameSettings to see if the game matches, enter info based on that.
			foreach (var control in dynamicAutoSplitterControls)
			{
				tabPage2.Controls.Remove(control);
			}

			dynamicAutoSplitterControls.Clear();

			//Add current game to gameSettings
			XmlDocument document = new XmlDocument();

			var gameNode = document.CreateElement(autoSplitData.GameName + autoSplitData.Category);

			//var categoryNode = document.CreateElement(autoSplitData.Category);

			foreach (AutoSplitEntry splitEntry in autoSplitData.SplitData)
			{
				gameNode.AppendChild(ToElement(document, splitEntry.SplitName, splitEntry.NumberOfLoads));
			}


			AllGameAutoSplitSettings[autoSplitData.GameName + autoSplitData.Category] = gameNode;

			//otherGameSettings[]

			CreateAutoSplitControls(liveSplitState);

			//Change controls if we find the chosen game
			foreach (var gameSettings in AllGameAutoSplitSettings)
			{
				if (gameSettings.Key == gameName + category)
				{
					var game_element = gameSettings.Value;

					//var splits_element = game_element[autoSplitData.Category];
					Dictionary<string, int> usedSplitNames = new Dictionary<string, int>();
					foreach (XmlElement number_of_loads in game_element)
					{
						var up_down_controls = tabPage2.Controls.Find(number_of_loads.LocalName, true);

						if (usedSplitNames.ContainsKey(number_of_loads.LocalName) == false)
						{
							usedSplitNames[number_of_loads.LocalName] = 0;
						}
						else
						{
							usedSplitNames[number_of_loads.LocalName]++;
						}

						//var up_down = tabPage2.Controls.Find(number_of_loads.LocalName, true).FirstOrDefault() as NumericUpDown;

						NumericUpDown up_down = (NumericUpDown)up_down_controls[usedSplitNames[number_of_loads.LocalName]];

						if (up_down != null)
						{
							up_down.Value = Convert.ToInt32(number_of_loads.InnerText);
						}
					}

				}
			}
		}
		public int GetCumulativeNumberOfLoadsForSplit(string splitName)
		{
			int numberOfLoads = 0;
			splitName = removeInvalidXMLCharacters(splitName);
			foreach (AutoSplitEntry entry in autoSplitData.SplitData)
			{
				numberOfLoads += entry.NumberOfLoads;
				if (entry.SplitName == splitName)
				{
					return numberOfLoads;
				}
			}
			return numberOfLoads;
		}

		public int GetAutoSplitNumberOfLoadsForSplit(string splitName)
		{
			splitName = removeInvalidXMLCharacters(splitName);
			foreach (AutoSplitEntry entry in autoSplitData.SplitData)
			{
				if (entry.SplitName == splitName)
				{
					return entry.NumberOfLoads;
				}
			}

			//This should never happen, but might if the splits are changed without reloading the component...
			return 2;
		}

		public XmlNode GetSettings(XmlDocument document)
		{
			//RefreshCaptureWindowList();
			var settingsNode = document.CreateElement("Settings");

			settingsNode.AppendChild(ToElement(document, "Version", Assembly.GetExecutingAssembly().GetName().Version.ToString(3)));

			settingsNode.AppendChild(ToElement(document, "RequiredMatches", FeatureDetector.numberOfBinsCorrect));

			if (captureIDs != null)
			{
				if (processListComboBox.SelectedIndex < captureIDs.Count && processListComboBox.SelectedIndex >= 0)
				{
					var selectedCaptureTitle = captureIDs[processListComboBox.SelectedIndex];

					settingsNode.AppendChild(ToElement(document, "SelectedCaptureTitle", selectedCaptureTitle));
				}
			}
            

			var captureRegionNode = document.CreateElement("CaptureRegion");

			captureRegionNode.AppendChild(ToElement(document, "X", selectionRectanglePreviewBox.X));
			captureRegionNode.AppendChild(ToElement(document, "Y", selectionRectanglePreviewBox.Y));
			captureRegionNode.AppendChild(ToElement(document, "Width", selectionRectanglePreviewBox.Width));
			captureRegionNode.AppendChild(ToElement(document, "Height", selectionRectanglePreviewBox.Height));

			settingsNode.AppendChild(captureRegionNode);

			settingsNode.AppendChild(ToElement(document, "AutoSplitEnabled", enableAutoSplitterChk.Checked));
			settingsNode.AppendChild(ToElement(document, "AutoSplitDisableOnSkipUntilSplit", chkAutoSplitterDisableOnSkip.Checked));
      //settingsNode.AppendChild(ToElement(document, "RemoveFadeins", chkRemoveFadeIns.Checked));

      var splitsNode = document.CreateElement("AutoSplitGames");

			//Re-Add all other games/categories to the xml file
			foreach (var gameSettings in AllGameAutoSplitSettings)
			{
				if (gameSettings.Key != autoSplitData.GameName + autoSplitData.Category)
				{
					XmlNode node = document.ImportNode(gameSettings.Value, true);
					splitsNode.AppendChild(node);
				}
			}

			var gameNode = document.CreateElement(autoSplitData.GameName + autoSplitData.Category);

			//var categoryNode = document.CreateElement(autoSplitData.Category);

			foreach (AutoSplitEntry splitEntry in autoSplitData.SplitData)
			{
				gameNode.AppendChild(ToElement(document, splitEntry.SplitName, splitEntry.NumberOfLoads));
			}
			AllGameAutoSplitSettings[autoSplitData.GameName + autoSplitData.Category] = gameNode;
			//gameNode.AppendChild(categoryNode);
			splitsNode.AppendChild(gameNode);
			settingsNode.AppendChild(splitsNode);
			//settingsNode.AppendChild(ToElement(document, "AutoReset", AutoReset.ToString()));
			//settingsNode.AppendChild(ToElement(document, "Category", category.ToString()));
			/*if (checkedListBox1.Items.Count == SplitsByCategory[category].Length)
			{
				for (int i = 0; i < checkedListBox1.Items.Count; i++)
				{
					SplitsByCategory[category][i].enabled = (checkedListBox1.GetItemCheckState(i) == CheckState.Checked);
				}
			}

			foreach (Split[] category in SplitsByCategory)
			{
				foreach (Split split in category)
				{
					settingsNode.AppendChild(ToElement(document, "split_" + split.splitID, split.enabled.ToString()));
				}
			}*/

			return settingsNode;
		}

		public void SetSettings(XmlNode settings)
		{
			var element = (XmlElement)settings;
			if (!element.IsEmpty)
			{
				Version version;
				if (element["Version"] != null)
				{
					version = Version.Parse(element["Version"].InnerText);
				}
				else {
					version = new Version(1, 0, 0);
				}

				if (element["RequiredMatches"] != null)
				{
					FeatureDetector.numberOfBinsCorrect = Convert.ToInt32(element["RequiredMatches"].InnerText);
				}

				if (element["SelectedCaptureTitle"] != null)
				{
					String selectedCaptureTitle = element["SelectedCaptureTitle"].InnerText;
					selectedCaptureID = selectedCaptureTitle;
					UpdateIndexToCaptureID();
					RefreshCaptureWindowList();
				}

				if (element["ScalingPercent"] != null)
				{
				}

				if (element["CaptureRegion"] != null)
				{
					var element_region = element["CaptureRegion"];
					if (element_region["X"] != null && element_region["Y"] != null && element_region["Width"] != null && element_region["Height"] != null)
					{
						int captureRegionX = Convert.ToInt32(element_region["X"].InnerText);
						int captureRegionY = Convert.ToInt32(element_region["Y"].InnerText);
						int captureRegionWidth = Convert.ToInt32(element_region["Width"].InnerText);
						int captureRegionHeight = Convert.ToInt32(element_region["Height"].InnerText);

						selectionRectanglePreviewBox = new Rectangle(captureRegionX, captureRegionY, captureRegionWidth, captureRegionHeight);
						selectionTopLeft = new Point(captureRegionX, captureRegionY);
						selectionBottomRight = new Point(captureRegionX + captureRegionWidth, captureRegionY + captureRegionHeight);

						//RefreshCaptureWindowList();
					}
				}

				/*foreach (Split[] category in SplitsByCategory)
				{
					foreach (Split split in category)
					{
						if (element["split_" + split.splitID] != null)
						{
							split.enabled = Convert.ToBoolean(element["split_" + split.splitID].InnerText);
						}
					}
				}*/
				if (element["AutoSplitEnabled"] != null)
				{
					enableAutoSplitterChk.Checked = Convert.ToBoolean(element["AutoSplitEnabled"].InnerText);
				}

				if (element["AutoSplitDisableOnSkipUntilSplit"] != null)
				{
					chkAutoSplitterDisableOnSkip.Checked = Convert.ToBoolean(element["AutoSplitDisableOnSkipUntilSplit"].InnerText);
				}

        if (element["AutoSplitGames"] != null)
				{
					var auto_split_element = element["AutoSplitGames"];

					foreach (XmlElement game in auto_split_element)
					{
						if (game.LocalName != autoSplitData.GameName)
						{
							AllGameAutoSplitSettings[game.LocalName] = game;
						}
					}

					if (auto_split_element[autoSplitData.GameName + autoSplitData.Category] != null)
					{
						var game_element = auto_split_element[autoSplitData.GameName + autoSplitData.Category];
						AllGameAutoSplitSettings[autoSplitData.GameName + autoSplitData.Category] = game_element;
						//var splits_element = game_element[autoSplitData.Category];
						Dictionary<string, int> usedSplitNames = new Dictionary<string, int>();
						foreach (XmlElement number_of_loads in game_element)
						{
							var up_down_controls = tabPage2.Controls.Find(number_of_loads.LocalName, true);

							//This can happen if the layout was not saved and contains old splits.
							if(up_down_controls == null || up_down_controls.Length == 0)
							{
								continue;
							}

							if (usedSplitNames.ContainsKey(number_of_loads.LocalName) == false)
							{
								usedSplitNames[number_of_loads.LocalName] = 0;
							}
							else
							{
								usedSplitNames[number_of_loads.LocalName]++;
							}

							//var up_down = tabPage2.Controls.Find(number_of_loads.LocalName, true).FirstOrDefault() as NumericUpDown;

							NumericUpDown up_down = (NumericUpDown)up_down_controls[usedSplitNames[number_of_loads.LocalName]];

							if (up_down != null)
							{
								up_down.Value = Convert.ToInt32(number_of_loads.InnerText);
							}
						}
					}
				}
        
			}
		}

		#endregion Public Methods

		#region Private Methods

		private void AutoSplitUpDown_ValueChanged(object sender, EventArgs e, string splitName)
		{
			foreach (AutoSplitEntry entry in autoSplitData.SplitData)
			{
				if (entry.SplitName == splitName)
				{
					entry.NumberOfLoads = (int)((NumericUpDown)sender).Value;
					return;
				}
			}
		}

		private void checkAutoReset_CheckedChanged(object sender, EventArgs e)
		{
		}

		private void checkedListBox1_ItemCheck(object sender, ItemCheckEventArgs e)
		{
		}

		private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
		{
		}

		private void CreateAutoSplitControls(LiveSplitState state)
		{
			autoSplitCategoryLbl.Text = "Category: " + state.Run.CategoryName;
			autoSplitNameLbl.Text = "Game: " + state.Run.GameName;

			int splitOffsetY = 95;
			int splitSpacing = 50;

			int splitCounter = 0;
			autoSplitData = new AutoSplitData(removeInvalidXMLCharacters(state.Run.GameName), removeInvalidXMLCharacters(state.Run.CategoryName));

			foreach (var split in state.Run)
			{
				//Setup controls for changing AutoSplit settings
				var autoSplitPanel = new System.Windows.Forms.Panel();
				var autoSplitLbl = new System.Windows.Forms.Label();
				var autoSplitUpDown = new System.Windows.Forms.NumericUpDown();

				autoSplitUpDown.Value = 2;
				autoSplitPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
				autoSplitPanel.Controls.Add(autoSplitUpDown);
				autoSplitPanel.Controls.Add(autoSplitLbl);
				autoSplitPanel.Location = new System.Drawing.Point(28, splitOffsetY + splitSpacing * splitCounter);
				autoSplitPanel.Size = new System.Drawing.Size(409, 39);

				autoSplitLbl.AutoSize = true;
				autoSplitLbl.Location = new System.Drawing.Point(3, 10);
				autoSplitLbl.Size = new System.Drawing.Size(199, 13);
				autoSplitLbl.TabIndex = 0;
				autoSplitLbl.Text = split.Name;

				autoSplitUpDown.Location = new System.Drawing.Point(367, 8);
				autoSplitUpDown.Size = new System.Drawing.Size(35, 20);
				autoSplitUpDown.TabIndex = 1;

				//Remove all whitespace to name the control, we can then access it in SetSettings.
				autoSplitUpDown.Name = removeInvalidXMLCharacters(split.Name);

				autoSplitUpDown.ValueChanged += (s, e) => AutoSplitUpDown_ValueChanged(autoSplitUpDown, e, removeInvalidXMLCharacters(split.Name));

				tabPage2.Controls.Add(autoSplitPanel);
				//tabPage2.Controls.Add(autoSplitLbl);
				//tabPage2.Controls.Add(autoSplitUpDown);

				autoSplitData.SplitData.Add(new AutoSplitEntry(removeInvalidXMLCharacters(split.Name), 2));
				dynamicAutoSplitterControls.Add(autoSplitPanel);
				splitCounter++;
			}
		}

		private void enableAutoSplitterChk_CheckedChanged(object sender, EventArgs e)
		{
			AutoSplitterEnabled = enableAutoSplitterChk.Checked;
		}



        private void previewPictureBox_MouseUp(object sender, MouseEventArgs e)
        {
        }

        private void processListComboBox_DropDown(object sender, EventArgs e)
        {
            RefreshCaptureWindowList();
            //processListComboBox.SelectedIndex = 0;
        }

        private void RefreshCaptureWindowList()
		{
			try
			{
				Process[] processListtmp = Process.GetProcesses();
				List<Process> processes_with_name = new List<Process>();

				if (captureIDs != null)
				{
					if (processListComboBox.SelectedIndex < captureIDs.Count && processListComboBox.SelectedIndex >= 0)
					{
						selectedCaptureID = processListComboBox.SelectedItem.ToString();
					}
				}

				captureIDs = new List<string>();

				processListComboBox.Items.Clear();
				numScreens = 0;
				foreach (Process process in processListtmp)
				{
					if (!String.IsNullOrEmpty(process.MainWindowTitle))
					{
						//Console.WriteLine("Process: {0} ID: {1} Window title: {2} HWND PTR {3}", process.ProcessName, process.Id, process.MainWindowTitle, process.MainWindowHandle);
						processListComboBox.Items.Add(process.ProcessName + ": " + process.MainWindowTitle);
						captureIDs.Add(process.ProcessName);
						processes_with_name.Add(process);
					}
				}

				UpdateIndexToCaptureID();

				//processListComboBox.SelectedIndex = 0;
				processList = processes_with_name.ToArray();
			}
			catch(Exception ex)
			{
				Console.WriteLine("Error: " + ex.ToString());
			}
		}

		public string removeInvalidXMLCharacters(string in_string)
		{
			if (in_string == null) return null;

			StringBuilder sbOutput = new StringBuilder();
			char ch;

			bool was_other_char = false;

			for (int i = 0; i < in_string.Length; i++)
			{
				ch = in_string[i];

				if ((ch >= 0x0 && ch <= 0x2F) ||
					(ch >= 0x3A && ch <= 0x40) ||
					(ch >= 0x5B && ch <= 0x60) ||
					(ch >= 0x7B)
					)
				{
					continue;
				}

				//Can't start with a number.
				if (was_other_char == false && ch >= '0' && ch <= '9')
				{
					continue;
				}

				/*if ((ch >= 0x0020 && ch <= 0xD7FF) ||
					(ch >= 0xE000 && ch <= 0xFFFD) ||
					ch == 0x0009 ||
					ch == 0x000A ||
					ch == 0x000D)
				{*/
				sbOutput.Append(ch);
				was_other_char = true;
				//}
			}

			if (sbOutput.Length == 0)
			{
				sbOutput.Append("NULL");
			}

			return sbOutput.ToString();
		}

		private void saveDiagnosticsButton_Click(object sender, EventArgs e)
		{
			try
			{
				FolderBrowserDialog fbd = new FolderBrowserDialog();

				var result = fbd.ShowDialog();

				if (result != DialogResult.OK)
				{
					return;
				}

				//System.IO.Directory.CreateDirectory(fbd.SelectedPath);
				numCaptures++;
				lastFullCapture.Save(fbd.SelectedPath + "/" + numCaptures.ToString() + "_FULL_" + lastMatchingBins + ".jpg", ImageFormat.Jpeg);
				lastFullCroppedCapture.Save(fbd.SelectedPath + "/" + numCaptures.ToString() + "_FULL_CROPPED_" + lastMatchingBins + ".jpg", ImageFormat.Jpeg);
				lastDiagnosticCapture.Save(fbd.SelectedPath + "/" + numCaptures.ToString() + "_DIAGNOSTIC_" + lastMatchingBins + ".jpg", ImageFormat.Jpeg);
				saveFeatureVectorToTxt(lastFeatures, numCaptures.ToString() + "_FEATURES_" + lastMatchingBins + ".txt", fbd.SelectedPath);
			}
			catch(Exception ex)
			{
				Console.WriteLine("Error: " + ex.ToString());
			}
		}

		private void saveFeatureVectorToTxt(List<int> featureVector, string filename, string directoryName)
		{
			System.IO.Directory.CreateDirectory(directoryName);
			try
			{
				using (var file = File.CreateText(directoryName + "/" + filename))
				{
					file.Write("{");
					file.Write(string.Join(",", featureVector));
					file.Write("},\n");
				}
			}
			catch
			{
				//yeah, silent catch is bad, I don't care
			}
		}
        

		private XmlElement ToElement<T>(XmlDocument document, String name, T value)
		{
			var element = document.CreateElement(name);
			element.InnerText = value.ToString();
			return element;
		}

		private void UpdateIndexToCaptureID()
		{
			//Find matching window, set selected index to index in dropdown items
			int item_index = 0;
			for (item_index = 0; item_index < processListComboBox.Items.Count; item_index++)
			{
				String item = processListComboBox.Items[item_index].ToString();
				if (item.Contains(selectedCaptureID))
				{
					processListComboBox.SelectedIndex = item_index;
					//processListComboBox.Text = processListComboBox.SelectedItem.ToString();

					break;
				}
			}
		}

        #endregion Private Methods
        private void Crash_OnLoadingChanged(int old, int newLoad) {
            LoadingState.Text = "Loading: " + (newLoad == 1).ToString();
        }


        private void chkAutoSplitterDisableOnSkip_CheckedChanged(object sender, EventArgs e)
		{
			AutoSplitterDisableOnSkipUntilSplit = chkAutoSplitterDisableOnSkip.Checked;
		}

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void lblVersion_Click(object sender, EventArgs e)
        {

        }

        private void hookBtn_Click(object sender, EventArgs e)
        {
            selected.Text = processList[processListComboBox.SelectedIndex].ProcessName;
            memory.HookProcess(processList[processListComboBox.SelectedIndex]);
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
    public class AutoSplitData
	{
		#region Public Fields

		public string Category;
		public string GameName;
		public List<AutoSplitEntry> SplitData;

		#endregion Public Fields

		#region Public Constructors

		public AutoSplitData(string gameName, string category)
		{
			SplitData = new List<AutoSplitEntry>();
			GameName = gameName;
			Category = category;
		}

		#endregion Public Constructors
	}

	public class AutoSplitEntry
	{
		#region Public Fields

		public int NumberOfLoads = 2;
		public string SplitName = "";

		#endregion Public Fields

		#region Public Constructors

		public AutoSplitEntry(string splitName, int numberOfLoads)
		{
			SplitName = splitName;
			NumberOfLoads = numberOfLoads;
		}

		#endregion Public Constructors
	}
}