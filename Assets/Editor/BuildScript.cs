using System;
using System.Linq;
using UnityEditor;

public class BuildScript {

    public static void BuildAndroid() {
        EditorSetup.AndroidSdkRoot = Environment.GetEnvironmentVariable("ANDROID_SDK_ROOT");
        EditorSetup.AndroidNdkRoot = Environment.GetEnvironmentVariable("ANDROID_NDK_HOME");
        EditorSetup.JdkRoot = Environment.GetEnvironmentVariable("JAVA_HOME");
        Build(BuildTarget.Android);
    }

    public static void BuildWebGL()
    {
        Build(BuildTarget.WebGL);
    }

    public static void Build(BuildTarget target) {
        VersionHelper.setBuildProperties();
        string[] levels = { "Assets/main.unity" };
        BuildPipeline.BuildPlayer(levels.ToArray(), Environment.GetCommandLineArgs().Last(), target, BuildOptions.None);
        VersionHelper.revertBuildPropertiesToDefault();
    }
}