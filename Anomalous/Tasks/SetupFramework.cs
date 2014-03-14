using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace AnomalousMSBuild.Tasks
{
	public class SetupFramework : Task
	{
        public SetupFramework()
		{
		}
		
		public override bool Execute ()
		{
            try
            {
                runProcess(new ProcessStartInfo("ln", String.Format("-s \"{0}\" Current", CurrentVersion))
                {
                    WorkingDirectory = Path.Combine(BasePath, "Versions")
                });
                runProcess(new ProcessStartInfo("ln", String.Format("-s \"Versions/{1}/{0}\" \"{0}\"", MainBinaryName, CurrentVersion))
                {
                    WorkingDirectory = BasePath
                });
            }
            catch (Exception e)
            {
                Log.LogWarning("Exception Type {0}", e.GetType().FullName);
                Log.LogErrorFromException(e);
                Log.LogError("Failed to setup framework");
                return false;
            }
			return true;
		}

        private void runProcess(ProcessStartInfo procInfo)
        {
            procInfo.UseShellExecute = false;
            procInfo.RedirectStandardOutput = true;
            procInfo.CreateNoWindow = true;

            Process proc = Process.Start(procInfo);
            String line;
            while ((line = proc.StandardOutput.ReadLine()) != null)
            {
                Log.LogMessageFromText(line, MessageImportance.High);
            }

            proc.WaitForExit();
			if (proc.ExitCode > 1) //1 means the link already exists so we ignore that one.
            {
				throw new Exception(String.Format("SetupFramework failed with exit code: {0}", proc.ExitCode.ToString()));
            }
        }
		
		[Required]
		public String BasePath { get; set; }

        [Required]
        public String CurrentVersion { get; set; }

        [Required]
        public String MainBinaryName { get; set; }
    }
}
