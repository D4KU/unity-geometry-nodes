using UnityEngine;

namespace GeometryNodes
{
    [DisallowMultipleComponent]
    internal class Group : MonoBehaviour
    {
        private int id;
        public int Id
        {
            get => id;
            set => id = value;
        }
    }
}
