using UnityEditor;
using UnityEngine;

namespace HALBY.Utility
{
    
    public class SaveMeshAsAsset : EditorWindow
    {
        private string _path = "";
        private SkinnedMeshRenderer _renderer;
        
        [MenuItem("HALBY/Save Mesh As Asset")]
        private static void Create()
        {
            GetWindow<SaveMeshAsAsset>("Save Mesh As Asset");            
        }

        private void OnGUI()
        {
            _renderer = EditorGUILayout.ObjectField("Mesh", _renderer, typeof(SkinnedMeshRenderer), true) as SkinnedMeshRenderer;
            _path = EditorGUILayout.TextField("Save Path", _path);
            if (GUILayout.Button("Save"))
            {
                if (_renderer == null) return;
                var mesh = _renderer.sharedMesh;
                AssetDatabase.CreateAsset(mesh, _path);
                AssetDatabase.SaveAssets();
            }
        }
    }

}