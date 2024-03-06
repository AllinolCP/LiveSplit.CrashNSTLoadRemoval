using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrashNSTLoadRemoval.Memory
{
	public abstract class GameMemory
	{
		private Process process;

		private string processName;

		protected GameMemory(string processName)
		{
			this.processName = processName;
		}

		protected abstract void OnHook(Process process);
		protected abstract void OnUnhook();

		public bool ProcessHooked => process != null && !process.HasExited;

        public int[] GetProcesses()
        {
            return Process.GetProcessesByName(processName).Select(process => process.Id).ToArray<int>();
        }

		public bool HookProcess(Process prc)
		{
			if (process != null && process.HasExited)
			{
				process = null;
				OnUnhook();

				return false;
			}

			if (process == null)
			{
				process = prc;

				if (process == null || process.HasExited)
				{
					return false;
				}

				MemoryReader.Update64Bit(process);

				OnHook(process);
			}

			return process != null;
		}
	}
}
