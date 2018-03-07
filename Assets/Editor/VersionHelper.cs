using System;
using System.Diagnostics;
using System.Linq;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Callbacks;
using UnityEngine;

class VersionHelper : IPreprocessBuild
{
    public int callbackOrder { get { return 0; } }
    public void OnPreprocessBuild(BuildTarget target, string path)
    {
#if UNITY_EDITOR
        setBuildProperties();
#endif
    }

    [PostProcessBuild]
    public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject)
    {
#if UNITY_EDITOR
        revertBuildPropertiesToDefault();
#endif
    }

    public static void setBuildProperties()
    {
        DateTime currdate = DateTime.Now;
        string versionAppend = "";
        string gitVersion = GetCommitId();
        if (gitVersion.Length > 0)
            versionAppend = " (" + gitVersion + ")";
        int twoSecondsSinceProject = (int)(currdate - new DateTime(2017, 2, 10)).TotalSeconds / 2; //number of 2 seconds since start of project is enough until 2153-03-19

        // Package Name
        foreach (BuildTargetGroup group in Enum.GetValues(typeof(BuildTargetGroup)).Cast<BuildTargetGroup>())
            PlayerSettings.SetApplicationIdentifier(group, "ch.romibi.stalnias");
        // on android?

        // Package Version
        PlayerSettings.bundleVersion = "stalnias.α." + currdate.ToString("yy.MM.dd") + versionAppend;
        PlayerSettings.iOS.buildNumber = gitVersion;
        PlayerSettings.Android.bundleVersionCode = twoSecondsSinceProject;
        //update switch, ps4 etc stuff
    }

    public static void revertBuildPropertiesToDefault()
    {
        PlayerSettings.bundleVersion = "stalnias.α.unknown";
        PlayerSettings.iOS.buildNumber = "0";
        PlayerSettings.Android.bundleVersionCode = 1;
        //reset switch, ps4 etc stuff
    }

    public static string GetCommitId()
    // by https://stackoverflow.com/q/26515656
    {
        string strCommit = "";

        Process p = new Process();
        // Set path to git exe.
        p.StartInfo.FileName = "git";
        // Set git command.
        p.StartInfo.Arguments = "rev-parse --short HEAD";
        // Set working directory.
        p.StartInfo.WorkingDirectory = Application.dataPath + "/../";
        p.StartInfo.RedirectStandardOutput = true;
        p.StartInfo.UseShellExecute = false;
        p.Start();

        // Pass output to variable.
        strCommit = p.StandardOutput.ReadToEnd();

        p.WaitForExit();

        if (string.IsNullOrEmpty(strCommit) == true)
        {
            UnityEngine.Debug.LogError("UNABLE TO COMMIT HASH");
        }

        strCommit = strCommit.Trim();

        return strCommit;
    }
}