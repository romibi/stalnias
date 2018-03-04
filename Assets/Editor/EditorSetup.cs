using UnityEditor;

public class EditorSetup {
    public static string AndroidSdkRoot {
        get { return EditorPrefs.GetString("AndroidSdkRoot"); }
        set { EditorPrefs.SetString("AndroidSdkRoot", value); }
    }

    public static string JdkRoot {
        get { return EditorPrefs.GetString("JdkPath"); }
        set { EditorPrefs.SetString("JdkPath", value); }
    }

    // This requires Unity 5.3 or later
    public static string AndroidNdkRoot {
        get { return EditorPrefs.GetString("AndroidNdkRoot"); }
        set { EditorPrefs.SetString("AndroidNdkRoot", value); }
    }
}
