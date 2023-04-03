using UnityEngine;

namespace GeometryNodes
{
    [DisallowMultipleComponent]
    internal class PositionOverride : MonoBehaviour
    {
        private Vector3 original;
        public Vector3 Original
        {
            get => original;
            set => original = value;
        }
    }
}
