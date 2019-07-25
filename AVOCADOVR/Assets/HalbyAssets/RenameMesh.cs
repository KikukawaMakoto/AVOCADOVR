using UnityEngine;

namespace HALBY.Utility
{
    [RequireComponent(typeof(SkinnedMeshRenderer))]
    public class RenameMesh : MonoBehaviour
    {
        public SkinnedMeshRenderer Renderer;
        public string Name = "";

        private void OnValidate()
        {
            if (Renderer) return;
            Renderer = GetComponent<SkinnedMeshRenderer>();
        }
    }
}