using Unity.VisualScripting;
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;
using System.Linq;

namespace GeometryNodes
{
    [CustomEditor(typeof(GeometryNodeInput))]
    [CanEditMultipleObjects]
    public class GeometryNodeInputEditor : Editor
    {
        enum InputType { Int, Float, Bool };

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            Dictionary<string, HashSet<GeometryNodeInput>> floats = new();
            Dictionary<string, HashSet<GeometryNodeInput>> ints = new();
            Dictionary<string, HashSet<GeometryNodeInput>> bools = new();

            // Fill dictionary
            foreach (GeometryNodeInput input in targets)
            {
                foreach (IUnit unit in input.GetComponent<ScriptMachine>().graph.units)
                {
                    switch (unit)
                    {
                        case FloatInput i:
                            Add(floats, i.Key, input);
                            break;
                        case IntInput i:
                            Add(ints, i.Key, input);
                            break;
                        case BoolInput i:
                            Add(bools, i.Key, input);
                            break;
                        default:
                            continue;
                    }
                }
            }

            Render(floats, EditorGUILayout.FloatField, x => x.floats);
            Render(ints  , EditorGUILayout.IntField  , x => x.ints);
            Render(bools , EditorGUILayout.Toggle    , x => x.bools);
        }

        private void Add(
            Dictionary<string, HashSet<GeometryNodeInput>> target,
            string key,
            GeometryNodeInput input)
        {
            if (string.IsNullOrWhiteSpace(key))
                return;

            if (!target.TryGetValue(key, out var list))
            {
                list = new();
                target.Add(key, list);
            }
            list.Add(input);
        }

        private void Render<T>(
            Dictionary<string, HashSet<GeometryNodeInput>> dict,
            Func<GUIContent, T, GUILayoutOption[], T> field,
            Func<GeometryNodeInput, Dictionary<string, T>> getter)
        {
            foreach (var (key, set) in dict)
            {
                getter(set.First()).TryGetValue(key, out T oldValue);

                EditorGUILayout.BeginHorizontal();
                T newValue = field(new GUIContent(key), oldValue, new GUILayoutOption[0]);
                bool clicked = GUILayout.Button(nameof(GeometryNodeInput.Reset), GUILayout.Width(80));
                EditorGUILayout.EndHorizontal();

                if (clicked)
                {
                    foreach (var target in set)
                        target.Reset<T>(key);
                    continue;
                }

                if (newValue.Equals(oldValue))
                    continue;

                foreach (var target in set)
                {
                    target.Trigger(key, newValue);
                    getter(target)[key] = newValue;
                }
            }
        }
    }
}
