using System;
using Microsoft.Build.Utilities;
using System.Diagnostics;
using Microsoft.Build.Framework;
namespace AnomalousMSBuild.Tasks
{
	public class CleanXCode : Task
	{
		public CleanXCode ()
		{
		}
		
		public override bool Execute ()
		{
			try
            {
                ProcessStartInfo xcodeBuildProcInfo = new ProcessStartInfo("xcodebuild", "clean");
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
                Log.LogError("Failed to clean xcode project");
				return false;
            }
			return true;
		}
	}
}

