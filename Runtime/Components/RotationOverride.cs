using UnityEngine;

namespace GeometryNodes
{
    /// <summary>
    /// Flags objects rotated through a node
    /// </summary>
    [DisallowMultipleComponent]
    public class RotationOverride : MonoBehaviour
    {
        [Tooltip("The object's local rotation before any node touched it")]
        private Quaternion original;

        /// <summary>
        /// The object's local rotation before any node touched it
        /// </summary>
        public Quaternion Original
        {
            get => original;
            set => original = value;
        }

        /// <summary>
        /// Add and initialize a new override if none exists next to the
        /// given Transform. Otherwise return the existing override.
        /// </summary>
        public static RotationOverride Add(Transform t)
        {
            if (!t.GetOrAddComponent(out RotationOverride o))
                o.Original = t.localRotation;
            return o;
        }

        /// <summary>
        /// If an override exists next to the given transform, remove it
        /// and restore the original local rotation.
        /// </summary>
        public static void Remove(Transform t)
        {
            if (t == null)
                return;

            if (t.TryGetComponent(out RotationOverride o))
            {
                t.localRotation = o.Original;
                o.SafeDestroy();
            }
        }
    }
}
