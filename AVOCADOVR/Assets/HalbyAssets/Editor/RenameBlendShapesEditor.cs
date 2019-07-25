using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace HALBY.Utility
{
    [CustomEditor(typeof(RenameBlendShapes))]
    public class RenameBlendShapesEditor : Editor
    {
        private string _blendShapeName = "";

        private struct BlendShapeInfo
        {
            public string Name;
            public float Weight;
            public Vector3[] Vertices;
            public Vector3[] Normals;
            public Vector3[] Tangents;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var rbs = target as RenameBlendShapes;
            if (!rbs) return;

            if (GUILayout.Button("Rename!"))
            {
                var mesh = rbs.Renderer.sharedMesh;
                var count = mesh.blendShapeCount;

                var blendShapes = new List<BlendShapeInfo>();

                // 元あるBlendShapeの格納作業
                for (var i = 0; i < count; i++)
                {
                    var bsName = mesh.GetBlendShapeName(i);

                    // 名前の置換作業
                    var newName = Regex.Replace(bsName, rbs.Regex, rbs.Replace);

                    var frames = mesh.GetBlendShapeFrameCount(i);
                    for (var j = 0; j < frames; j++)
                    {
                        var weight = mesh.GetBlendShapeFrameWeight(i, j);

                        var deltaVertices = new Vector3[mesh.vertexCount];
                        var deltaNormals = new Vector3[mesh.vertexCount];
                        var deltaTangents = new Vector3[mesh.vertexCount];

                        mesh.GetBlendShapeFrameVertices(i, j, deltaVertices, deltaNormals, deltaTangents);

                        blendShapes.Add(new BlendShapeInfo()
                        {
                            Name = newName,
                            Weight = weight,
                            Vertices = deltaVertices,
                            Normals = deltaNormals,
                            Tangents = deltaTangents
                        });
                    }
                }

                // 一括削除
                mesh.ClearBlendShapes();

                // 追加する
                blendShapes.ForEach(info =>
                    mesh.AddBlendShapeFrame(info.Name, info.Weight, info.Vertices, info.Normals, info.Tangents));
            }

            // デバッグ用
//            _blendShapeName = EditorGUILayout.TextField("Blend Shape Name", _blendShapeName);
//            if (GUILayout.Button("Get BlendShape Info"))
//            {
//                var mesh = rbs.Renderer.sharedMesh;
//                var index = mesh.GetBlendShapeIndex(_blendShapeName);
//
//                var frames = mesh.GetBlendShapeFrameCount(index);
//                for (var i = 0; i < frames; i++)
//                {   
//                    var weight = mesh.GetBlendShapeFrameWeight(index, i);
//                    
//                    var deltaVertices = new Vector3[mesh.vertexCount];
//                    var deltaNormals = new Vector3[mesh.vertexCount];
//                    var deltaTangents = new Vector3[mesh.vertexCount];
//                    
//                    Debug.Log("Blend Shape " + _blendShapeName + " has " + weight + " weight on " + i + " frame.");
//                    Debug.Log(deltaVertices);
//                }
//            }
        }
    }
}