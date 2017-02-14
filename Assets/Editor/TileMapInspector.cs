using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CustomEditor(typeof(TileMap))]
public class TileMapInspector : Editor {
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        if (GUILayout.Button("Rebuild")) {
            TileMap tilemap = (TileMap)target;
            tilemap.BuildMap();
        }
    }
}
