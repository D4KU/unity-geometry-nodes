using UnityEngine;

namespace GeometryNodes
{
    [DisallowMultipleComponent]
    internal class RotationOverride : MonoBehaviour
    {
        private Quaternion original;
        public Quaternion Original
        {
            get => original;
            set => original = value;
        }
    }
}
