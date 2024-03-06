namespace LiveSplit.UI.Components
{
	partial class CrashNSTLoadRemovalSettings
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.lblVersion = new System.Windows.Forms.Label();
            this.processListComboBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.selected = new System.Windows.Forms.Label();
            this.hookBtn = new System.Windows.Forms.Button();
            this.hookStatus = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.chkAutoSplitterDisableOnSkip = new System.Windows.Forms.CheckBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.autoSplitNameLbl = new System.Windows.Forms.Label();
            this.autoSplitCategoryLbl = new System.Windows.Forms.Label();
            this.enableAutoSplitterChk = new System.Windows.Forms.CheckBox();
            this.LoadingState = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblVersion
            // 
            this.lblVersion.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblVersion.AutoSize = true;
            this.lblVersion.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVersion.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.lblVersion.Location = new System.Drawing.Point(431, 490);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(37, 13);
            this.lblVersion.TabIndex = 21;
            this.lblVersion.Text = "v0.0.0";
            this.lblVersion.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblVersion.Click += new System.EventHandler(this.lblVersion_Click);
            // 
            // processListComboBox
            // 
            this.processListComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.processListComboBox.FormattingEnabled = true;
            this.processListComboBox.Items.AddRange(new object[] {
            "Full Display (Primary)"});
            this.processListComboBox.Location = new System.Drawing.Point(86, 6);
            this.processListComboBox.Name = "processListComboBox";
            this.processListComboBox.Size = new System.Drawing.Size(345, 21);
            this.processListComboBox.TabIndex = 22;
            this.processListComboBox.DropDown += new System.EventHandler(this.processListComboBox_DropDown);
            this.processListComboBox.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(28, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 13);
            this.label1.TabIndex = 23;
            this.label1.Text = "Process:";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(476, 532);
            this.tabControl1.TabIndex = 38;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage1.Controls.Add(this.LoadingState);
            this.tabPage1.Controls.Add(this.selected);
            this.tabPage1.Controls.Add(this.hookBtn);
            this.tabPage1.Controls.Add(this.hookStatus);
            this.tabPage1.Controls.Add(this.lblVersion);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.processListComboBox);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(468, 506);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Setup";
            this.tabPage1.Click += new System.EventHandler(this.tabPage1_Click);
            // 
            // selected
            // 
            this.selected.AutoSize = true;
            this.selected.Location = new System.Drawing.Point(28, 73);
            this.selected.Name = "selected";
            this.selected.Size = new System.Drawing.Size(55, 13);
            this.selected.TabIndex = 44;
            this.selected.Text = "Selected: ";
            this.selected.Click += new System.EventHandler(this.label2_Click);
            // 
            // hookBtn
            // 
            this.hookBtn.Location = new System.Drawing.Point(356, 38);
            this.hookBtn.Name = "hookBtn";
            this.hookBtn.Size = new System.Drawing.Size(75, 23);
            this.hookBtn.TabIndex = 43;
            this.hookBtn.Text = "HOOK";
            this.hookBtn.UseVisualStyleBackColor = true;
            this.hookBtn.Click += new System.EventHandler(this.hookBtn_Click);
            // 
            // hookStatus
            // 
            this.hookStatus.AutoSize = true;
            this.hookStatus.Location = new System.Drawing.Point(28, 43);
            this.hookStatus.Name = "hookStatus";
            this.hookStatus.Size = new System.Drawing.Size(101, 13);
            this.hookStatus.TabIndex = 42;
            this.hookStatus.Text = "Status: Not Hooked";
            // 
            // tabPage2
            // 
            this.tabPage2.AutoScroll = true;
            this.tabPage2.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage2.Controls.Add(this.chkAutoSplitterDisableOnSkip);
            this.tabPage2.Controls.Add(this.label6);
            this.tabPage2.Controls.Add(this.label3);
            this.tabPage2.Controls.Add(this.autoSplitNameLbl);
            this.tabPage2.Controls.Add(this.autoSplitCategoryLbl);
            this.tabPage2.Controls.Add(this.enableAutoSplitterChk);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(468, 506);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "AutoSplitter";
            // 
            // chkAutoSplitterDisableOnSkip
            // 
            this.chkAutoSplitterDisableOnSkip.AutoSize = true;
            this.chkAutoSplitterDisableOnSkip.Location = new System.Drawing.Point(150, 9);
            this.chkAutoSplitterDisableOnSkip.Name = "chkAutoSplitterDisableOnSkip";
            this.chkAutoSplitterDisableOnSkip.Size = new System.Drawing.Size(239, 17);
            this.chkAutoSplitterDisableOnSkip.TabIndex = 43;
            this.chkAutoSplitterDisableOnSkip.Text = "Disable AutoSplitter on Skip until manual Split";
            this.chkAutoSplitterDisableOnSkip.UseVisualStyleBackColor = true;
            this.chkAutoSplitterDisableOnSkip.CheckedChanged += new System.EventHandler(this.chkAutoSplitterDisableOnSkip_CheckedChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold);
            this.label6.Location = new System.Drawing.Point(251, 75);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(188, 16);
            this.label6.TabIndex = 42;
            this.label6.Text = "Number of Loads per Split";
            this.label6.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold);
            this.label3.Location = new System.Drawing.Point(25, 75);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(51, 16);
            this.label3.TabIndex = 41;
            this.label3.Text = "Splits:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // autoSplitNameLbl
            // 
            this.autoSplitNameLbl.AutoSize = true;
            this.autoSplitNameLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold);
            this.autoSplitNameLbl.Location = new System.Drawing.Point(24, 29);
            this.autoSplitNameLbl.Name = "autoSplitNameLbl";
            this.autoSplitNameLbl.Size = new System.Drawing.Size(49, 16);
            this.autoSplitNameLbl.TabIndex = 40;
            this.autoSplitNameLbl.Text = "Name";
            this.autoSplitNameLbl.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // autoSplitCategoryLbl
            // 
            this.autoSplitCategoryLbl.AutoSize = true;
            this.autoSplitCategoryLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold);
            this.autoSplitCategoryLbl.Location = new System.Drawing.Point(24, 49);
            this.autoSplitCategoryLbl.Name = "autoSplitCategoryLbl";
            this.autoSplitCategoryLbl.Size = new System.Drawing.Size(71, 16);
            this.autoSplitCategoryLbl.TabIndex = 39;
            this.autoSplitCategoryLbl.Text = "Category";
            this.autoSplitCategoryLbl.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // enableAutoSplitterChk
            // 
            this.enableAutoSplitterChk.AutoSize = true;
            this.enableAutoSplitterChk.Location = new System.Drawing.Point(28, 9);
            this.enableAutoSplitterChk.Name = "enableAutoSplitterChk";
            this.enableAutoSplitterChk.Size = new System.Drawing.Size(116, 17);
            this.enableAutoSplitterChk.TabIndex = 0;
            this.enableAutoSplitterChk.Text = "Enable AutoSplitter";
            this.enableAutoSplitterChk.UseVisualStyleBackColor = true;
            this.enableAutoSplitterChk.CheckedChanged += new System.EventHandler(this.enableAutoSplitterChk_CheckedChanged);
            // 
            // LoadingState
            // 
            this.LoadingState.AutoSize = true;
            this.LoadingState.Location = new System.Drawing.Point(28, 105);
            this.LoadingState.Name = "LoadingState";
            this.LoadingState.Size = new System.Drawing.Size(55, 13);
            this.LoadingState.TabIndex = 45;
            this.LoadingState.Text = "Selected: ";
            // 
            // CrashNSTLoadRemovalSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl1);
            this.Name = "CrashNSTLoadRemovalSettings";
            this.Padding = new System.Windows.Forms.Padding(7);
            this.Size = new System.Drawing.Size(474, 532);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.ResumeLayout(false);

		}

		#endregion
		private System.Windows.Forms.Label lblVersion;
		private System.Windows.Forms.ComboBox processListComboBox;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tabPage1;
		private System.Windows.Forms.TabPage tabPage2;
		private System.Windows.Forms.CheckBox enableAutoSplitterChk;
		private System.Windows.Forms.Label autoSplitCategoryLbl;
		private System.Windows.Forms.Label autoSplitNameLbl;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.CheckBox chkAutoSplitterDisableOnSkip;
        public System.Windows.Forms.Label hookStatus;
        private System.Windows.Forms.Button hookBtn;
        public System.Windows.Forms.Label selected;
        public System.Windows.Forms.Label LoadingState;
    }
}
