using System;
using Microsoft.Build.Utilities;
using Microsoft.Build.Framework;
using System.Diagnostics;

namespace AnomalousMSBuild.Tasks
{
	public class BuildXCode : Task
	{
		public BuildXCode ()
		{
		}
		
		public override bool Execute ()
		{
			try
            {
                ProcessStartInfo xcodeBuildProcInfo = new ProcessStartInfo("xcodebuild", String.Format("-configuration {0}", Configuration));
                xcodeBuildProcInfo.UseShellExecute = false;
                xcodeBuildProcInfo.RedirectStandardOutput = true;
                xcodeBuildProcInfo.CreateNoWindow = true;

                Process xcodebuildProc = Process.Start(xcodeBuildProcInfo);
				String line;
                while ((line = xcodebuildProc.StandardOutput.ReadLine()) != null) 
				{ 
					Log.LogMessageFromText(line, MessageImportance.High);
				}
				if(xcodebuildProc.ExitCode != 0)
				{
					Log.LogError("xcodebuild failed with exit code: {0}", xcodebuildProc.ExitCode.ToString());
					return false;
				}
            }
            catch (Exception e)
            {
                Log.LogErrorFromException(e);
                Log.LogError("Failed to build xcode project");
				return false;
            }
			return true;
		}
		
		[Required]
		/// <summary>
		/// This will specify the configuration passed to -configuration to xcodebuild.
		/// </summary>
		public String Configuration { get; set; }
	}
}

