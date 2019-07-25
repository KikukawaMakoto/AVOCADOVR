using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace HALBY.Utility
{
    [CustomEditor(typeof(RenameMesh))]
    public class RenameMeshEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var rm = target as RenameMesh;
            if (!rm) return;

            if (GUILayout.Button("Rename"))
            {
                rm.Renderer.sharedMesh.name = rm.Name;
            }
        }
    }

}
