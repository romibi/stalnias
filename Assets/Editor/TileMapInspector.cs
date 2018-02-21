using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CustomEditor(typeof(TileMap))]
[InitializeOnLoad]
public class TileMapInspector : Editor {
    public TileMapInspector()
    {
        try
        {
            triggerRebuild();
        }
        catch { }
        EditorApplication.playModeStateChanged += onPlayModeChanged;
    }

    static bool wasPlaying = false;
    
    IEnumerator loader;
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        if (GUILayout.Button("Rebuild")) {
            triggerRebuild();
        }
        if (GUILayout.Button("Abort"))
        {
            EditorApplication.update -= OnEditorUpdate;
        }
    }

    protected virtual void OnEditorUpdate()
    {
        TileMap tilemap = (TileMap)target;
        if(tilemap.EditorRequestOngoing() && loader!=null) {
            loader.MoveNext();
        } else
        {
            EditorApplication.update -= OnEditorUpdate;
            if (loader != null){
                try
                {
                    tilemap.EditorLoadMap();
                }
                catch { }
            }
        }
    }

    public void onPlayModeChanged(PlayModeStateChange state) {
        if (state == PlayModeStateChange.EnteredPlayMode)
            wasPlaying = true;
        if (EditorApplication.isCompiling)
            return;
        if (state == PlayModeStateChange.EnteredEditMode)
        {
            if (wasPlaying)
            {
                loader = null;
                try {
                    triggerRebuild();
                    wasPlaying = false;
                }
                catch { }
            }
        }
    }

    protected void triggerRebuild()
    {
        TileMap tilemap = (TileMap)target;
        loader = tilemap.EditorRequestMap();
        EditorApplication.update += OnEditorUpdate;
    }
}
