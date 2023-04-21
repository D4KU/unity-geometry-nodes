using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace GeometryNodes
{
    /// <summary>
    /// Common Geometry Node functionality
    /// </summary>
    internal static class Extensions
    {
        /// <summary>
        /// Reset all Geometry Nodes downstream from the given port in reverse
        /// order. Nodes furthest away are reset first.
        /// </summary>
        public static void ClearDownstream(this IUnitPort port)
        {
            List<GeometryUnit> toClear = new();
            Stack<IUnitPort> ports = new();
            ports.Push(port);

            while (ports.Count > 0)
            {
                foreach (IUnitConnection connection in ports.Pop().connections)
                {
                    IUnit successor = connection.destination.unit;
                    if (successor is GeometryUnit gUnit)
                        toClear.Add(gUnit);
                    foreach (IUnitOutputPort output in successor.outputs)
                        ports.Push(output);
                }
            }

            // First clear nodes found deepest in graph
            for (int i = toClear.Count - 1; i >= 0; i--)
                toClear[i].Clear();
        }

        /// <summary>
        /// Create a copy of the given object
        /// </summary>
        public static T Duplicate<T>(this T original, Transform parent = null)
            where T : Component
            {
                T copy = Object.Instantiate(original, parent);
                copy.AddComponent<Copy>();
                copy.gameObject.hideFlags = HideFlags.DontSave;
                return copy;
            }

        /// <summary>
        /// Get the <see cref="Group"/> ID of the object with the given
        /// component
        /// </summary>
        public static int? GroupId(this Component neighbor)
            => neighbor.TryGetComponent(out Group g) ? g.Id : null;

        /// <summary>
        /// Return all child objects that have to be removed from
        /// <paramref name="parent"/>s hierarchy before it gets destroyed.
        /// </summary>
        public static List<Transform> ChildrenToRescue(this Transform parent)
            => parent.Cast<Transform>()
                     .Where(x => x.GetComponent<Copy>() == null)
                     .ToList();

        /// <summary>
        /// Get a neighboring component, add one if none exists
        /// </summary>
        /// <returns>True if a component was already present</returns>
        public static bool GetOrAddComponent<T>(this Component neighbor, out T target)
            where T : Component
        {
            if (neighbor.TryGetComponent(out target))
                return true;
            target = neighbor.AddComponent<T>();
            return false;
        }

        /// <summary>
        /// Create an object with the given <paramref name="name"/>
        /// in the hierarchy between the given <paramref name="target"/> object
        /// and its parent.
        /// </summary>
        public static Transform Group(this Transform target, string name)
        {
            GameObject group = new($"{target.name} {name}");
            group.AddComponent<Group>().Id = Random.Range(int.MinValue, int.MaxValue);
            group.hideFlags = HideFlags.DontSave;

            Transform t = group.transform;
            t.SetParent(target.parent, false);
            t.localPosition = target.localPosition;
            t.SetSiblingIndex(target.GetSiblingIndex());
            target.parent = t;
            return t;
        }

        /// <summary>
        /// Add <paramref name="count"/> copies of <paramref name="original"/>
        /// to <paramref name="list"/>. Parent copies to
        /// <paramref name="parent"/>
        /// </summary>
        public static void AddCopies(
            this List<Transform> list,
            int count,
            Transform original,
            Transform parent)
        {
            for (int i = list.Count; i < count; i++)
            {
                Transform copy = original.Duplicate(parent);
                copy.name += $"({i})";
                list.Add(copy);
            }
        }

        /// <summary>
        /// Destroy working inside and outside Play mode
        /// </summary>
        public static void SafeDestroy(this Object o)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
                Object.DestroyImmediate(o);
            else
#endif
                Object.Destroy(o);
        }

        /// <inheritdoc cref="SafeDestroy(Object)"/>
        public static void SafeDestroy(this Transform t)
        {
            if (t) t.gameObject.SafeDestroy();
        }
    }
}
