using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace GeometryNodes
{
    public static class Utility
    {
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

        public static void SetValue<T>(
            this List<GeometryNodeInput.Pair<T>> list,
            string key,
            T value)
        {
            for (int i = 0; i < list.Count; i++)
            {
                var pair = list[i];
                if (pair.key == key)
                {
                    pair.value = value;
                    list[i] = pair;
                    return;
                }
            }

            list.Add(new() { key = key, value = value });
        }

        public static T Duplicate<T>(this T original, Transform parent = null)
            where T : Component
            {
                T copy = Object.Instantiate(original, parent);
                copy.AddComponent<Copy>();
                copy.gameObject.hideFlags = HideFlags.DontSave;
                return copy;
            }

        public static bool GetOrAddComponent<T>(this Component neighbor, out T target) where T : Component
        {
            if (neighbor.TryGetComponent(out target))
                return true;
            target = neighbor.AddComponent<T>();
            return false;
        }

        public static void MakeSibling(this Transform target, ref Transform sibling, string name)
        {
            if (sibling)
            {
                if (target.IsChildOf(sibling))
                    return;
            }
            else
            {
                sibling = new GameObject($"{target.name} {name}", typeof(Group)).transform;
            }

            sibling.parent = target.parent;
            sibling.SetSiblingIndex(target.GetSiblingIndex());
            sibling.localPosition = target.localPosition;
        }

        /// <summary>
        /// Transform a <paramref name="point"/> from the local space of
        /// <paramref name="source"/> to the one of <paramref name="target"/>
        /// </summary>
        public static Vector3 TransformPoint(this Transform source, Vector3 point, Transform target)
        {
            Vector3 v = source ? source.TransformPoint(point) : point;
            return target ? target.InverseTransformPoint(v) : v;
        }

        public static void SafeDestroy(this Transform t)
        {
            if (t)
                t.gameObject.SafeDestroy();
        }

        public static void SafeDestroy(this Object o)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
                Object.DestroyImmediate(o);
            else
#endif
                Object.Destroy(o);
        }
    }
}
