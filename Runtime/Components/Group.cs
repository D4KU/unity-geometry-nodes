using UnityEngine;

namespace GeometryNodes
{
    /// <summary>
    /// Flags objects created by nodes to group a set of objects
    /// </summary>
    [DisallowMultipleComponent]
    internal class Group : MonoBehaviour
    {
        [Tooltip("Copies of a group share their ID with the original. " +
            "Original groups have unique IDs in regards to each other.")]
        private int id;

        /// <summary>
        /// <see cref="Copy"/>s of a group share their ID with the original.
        /// Original groups have unique IDs in regards to each other.
        /// </summary>
        public int Id
        {
            get => id;
            set => id = value;
        }
    }
}
