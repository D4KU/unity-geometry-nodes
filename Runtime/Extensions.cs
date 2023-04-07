using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace GeometryNodes
{
    /// <summary>
    /// Common Geometry Node functionality
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Reset all Geometry Nodes downstream from the given port in reverse
        /// order. Nodes furthest away are reset first.
        /// </summary>
        public static void ClearDownstream(this IUnitPort output)
        {
            List<GeometryUnit> toClear = new();
            Stack<IUnitPort> ports = new();
            ports.Push(output);

            while (ports.Count > 0)
            {
                foreach (IUnitConnection connection in ports.Pop().connections)
                {
                    IUnit successor = connection.destination.unit;
                    if (successor is GeometryUnit gUnit)
                        toClear.Add(gUnit);
                    foreach (IUnitOutputPort port in successor.outputs)
                        ports.Push(port);
                }
            }

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
                copy.transform.RemoveOverrides();
                copy.gameObject.hideFlags = HideFlags.DontSave;
                return copy;
            }

        /// <summary>
        /// Remove all transform overrides from a object and restore its
        /// original transformation
        /// </summary>
        public static void RemoveOverrides(this Transform t)
        {
            PositionOverride.Remove(t);
            RotationOverride.Remove(t);
            ScaleOverride.Remove(t);
        }

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
