using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace GeometryNodes
{
    internal static class Utility
    {
        public static void RecursiveClear(this IUnitOutputPort output)
        {
            List<GeometryUnit> toClear = new();
            Stack<IUnitOutputPort> ports = new();
            ports.Push(output);

            while (ports.Count > 0)
            {
                foreach (IUnitConnection connection in ports.Pop().connections)
                {
                    IUnit successor = connection.destination.unit;
                    if (successor is GeometryUnit geoUnit)
                        toClear.Add(geoUnit);
                    foreach (IUnitOutputPort port in successor.outputs)
                        ports.Push(port);
                }
            }

            for (int i = toClear.Count - 1; i >= 0; i--)
                toClear[i].Clear();
        }

        public static T Duplicate<T>(
                this T original,
                Vector3 position = default,
                Quaternion rotation = default,
                Transform parent = null)
            where T : Component
            {
                T copy = Object.Instantiate(
                        original,
                        position,
                        rotation == default ? Quaternion.identity : rotation,
                        parent);
                copy.AddComponent<CopyMark>();
                copy.gameObject.hideFlags = HideFlags.DontSave;
                return copy;
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

        public static void SaveDestroy(this Transform t)
        {
            if (t)
                t.gameObject.SaveDestroy();
        }

        public static void SaveDestroy(this Object o)
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
