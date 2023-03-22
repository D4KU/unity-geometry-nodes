using UnityEngine;

namespace GeometryNodes
{
    [ExecuteAlways]
    internal class Group : MonoBehaviour
    {
        private void Awake()
        {
            gameObject.hideFlags = HideFlags.DontSave;
        }
    }
}
