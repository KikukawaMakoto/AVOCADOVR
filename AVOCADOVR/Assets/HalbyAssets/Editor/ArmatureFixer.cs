using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace HALBY.Utility
{
    public class ArmatureFixer : EditorWindow
    {
        private Transform _rootBone;
        private GameObject _meshes;

        [MenuItem("HALBY/Armature Fixer")]
        private static void Create()
        {
            GetWindow<ArmatureFixer>();
        }

        private void OnGUI()
        {
            _rootBone = EditorGUILayout.ObjectField("Root Bone (Hips)", _rootBone, typeof(Transform),
                true) as Transform;
            _meshes = EditorGUILayout.ObjectField("Meshes", _meshes, typeof(GameObject), true) as GameObject;
            if (_rootBone != null && _meshes != null && GUILayout.Button("Fix!"))
            {   
                var bones = _rootBone.GetComponentsInChildren<Transform>();
                var meshes = _meshes.GetComponentsInChildren<SkinnedMeshRenderer>();
                
                Undo.RecordObjects(meshes, "meshes changes");
                
                foreach (var mesh in meshes)
                {
                    mesh.rootBone = _rootBone;
                    foreach (var bone in mesh.bones)
                    {
                        Debug.Log(bone);
                    }
                    mesh.bones = (from bone in mesh.bones let targetBone = _rootBone.Find(bone.name) select targetBone ? targetBone : bone).ToArray();
                    foreach (var bone in mesh.bones)
                    {
                        Debug.Log(bone);
                    }
                }
            }
        }
    }
}