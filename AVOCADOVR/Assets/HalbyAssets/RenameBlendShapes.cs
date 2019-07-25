using UnityEngine;

namespace HALBY.Utility
{
    [RequireComponent(typeof(SkinnedMeshRenderer))]
    public class RenameBlendShapes : MonoBehaviour
    {
        public SkinnedMeshRenderer Renderer;
        public string Regex = "";
        public string Replace = "";

        // Use this for initialization
        void OnValidate()
        {
            if (Renderer) return;
            Renderer = GetComponent<SkinnedMeshRenderer>();
        }
    }
}