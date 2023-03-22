using UnityEngine;

namespace GeometryNodes
{
    [ExecuteAlways]
    internal class Copy : MonoBehaviour
    {
        private void Awake()
        {
            gameObject.hideFlags = HideFlags.DontSave;
        }
    }
}
