using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Build.Utilities;
using Microsoft.Build.Framework;
using System.IO;

namespace AnomalousMSBuild.Tasks
{
    /// <summary>
    /// This task will write the version of a specified versioned directory to a
    /// file by replacing keywords in another file. This is basicly a msbuild
    /// version of subwcrev.
    /// </summary>
    public class WriteSubversionVersion : Task
    {
        public override bool Execute()
        {
            bool success = true;
            int versionNumber = 0;
            String entriesFile = Path.Combine(Directory, SubversionFolder, EntriesFile);
            if (File.Exists(entriesFile))
            {
                using (TextReader svnFileReader = new StreamReader(File.OpenRead(entriesFile)))
                {
                    String versionLine = null;
                    for (int i = 0; i < VersionLineNumber; ++i)
                    {
                        versionLine = svnFileReader.ReadLine();
                    }

                    Log.LogMessageFromText(String.Format("Version is {0}", versionLine), MessageImportance.High);
                    if (!int.TryParse(versionLine, out versionNumber))
                    {
                        Log.LogWarning("Could not parse version out of file {0} on line {1}. Creating file with 0 as the revision.", entriesFile, VersionLineNumber);
                    }
                }
            }
            else
            {
                Log.LogWarning("Could not find {0}. Creating file with 0 as the revision.", entriesFile);
            }

            try
            {
                String sourceFileText = null;
                using (TextReader sourceFileReader = new StreamReader(File.OpenRead(SourceFile)))
                {
                    sourceFileText = sourceFileReader.ReadToEnd();
                }

                String destFileText = sourceFileText.Replace(VersionTag, versionNumber.ToString());

                using (TextWriter destFileWriter = new StreamWriter(DestFile, false))
                {
                    destFileWriter.Write(destFileText);
                }
            }
            catch (Exception e)
            {
                Log.LogErrorFromException(e);
                success = false;
            }

            return success;
        }

        /// <summary>
        /// The directory to read the subversion version from (under SubverisonFolder default is .svn).
        /// </summary>
        [Required]
        public String Directory { get; set; }

        /// <summary>
        /// The source file.
        /// </summary>
        [Required]
        public String SourceFile { get; set; }

        /// <summary>
        /// The file to write output to.
        /// </summary>
        [Required]
        public String DestFile { get; set; }

        String subversionFolder = ".svn";
        /// <summary>
        /// The name of the subversion folder. Defaults to .svn
        /// </summary>
        public String SubversionFolder
        {
            get
            {
                return subversionFolder;
            }
            set
            {
                subversionFolder = value;
            }
        }

        String entriesFile = "entries";
        /// <summary>
        /// The name of the entriesFile file. Defaults to entries
        /// </summary>
        public String EntriesFile
        {
            get
            {
                return entriesFile;
            }
            set
            {
                entriesFile = value;
            }
        }

        int versionLineNumber = 4;
        /// <summary>
        /// The number of lines in the entries file to read before the version number is found.
        /// </summary>
        public int VersionLineNumber
        {
            get
            {
                return versionLineNumber;
            }
            set
            {
                versionLineNumber = value;
            }
        }

        String versionTag = "$WCREV$";
        /// <summary>
        /// The tag to replace with the version number.
        /// </summary>
        public String VersionTag
        {
            get
            {
                return versionTag;
            }
            set
            {
                versionTag = value;
            }
        }
    }
}
