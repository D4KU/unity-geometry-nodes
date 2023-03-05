using UnityEngine;

namespace GeometryNodes
{
    [ExecuteAlways]
    internal class CopyMark : MonoBehaviour
    {
        private void Awake()
        {
            gameObject.hideFlags = HideFlags.DontSave;
        }
    }
}
