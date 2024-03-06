using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrashNSTLoadRemoval.Memory
{
	public class CrashMemory : GameMemory
	{
		public CrashMemory() : base("CrashBandicootNSaneTrilogy")
		{
			Loading = new GamePointer<int>(0x1A648A0, 0x30, 0X20, 0XB8, 0X128, 0X50, 0x18, 0x824);
		}

		public GamePointer<int> Lives { get; }
		public GamePointer<int> Masks { get; }
		public GamePointer<int> Loading { get; }

		protected override void OnHook(Process process)
		{
            Loading.Process = process;
		}

		protected override void OnUnhook()
		{
            Loading.Process = null;
		}

		public void Refresh()
		{
            Loading.Refresh();
		}
	}
}
