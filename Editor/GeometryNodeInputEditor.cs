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
            foreach (GeometryNodeInput target in targets)
            {
                ScriptMachine script = target.Script;
                if (script.graph == null)
                    continue;

                foreach (IUnit unit in script.graph.units)
                {
                    switch (unit)
                    {
                        case FloatInput i:
                            Add(floats, i.key, target);
                            break;
                        case IntInput i:
                            Add(ints, i.key, target);
                            break;
                        case BoolInput i:
                            Add(bools, i.key, target);
                            break;
                        default:
                            continue;
                    }
                }
            }

            Render(floats, EditorGUILayout.FloatField, x => x.floats);
            Render(ints  , EditorGUILayout.IntField  , x => x.ints);
            Render(bools , EditorGUILayout.Toggle    , x => x.bools);

            if (GUILayout.Button(nameof(GeometryNodeInput.Clear)))
                foreach (GeometryNodeInput target in targets)
                    target.Clear();
        }

        private void Add(
            Dictionary<string, HashSet<GeometryNodeInput>> dict,
            string key,
            GeometryNodeInput input)
        {
            if (string.IsNullOrWhiteSpace(key))
                return;

            if (!dict.TryGetValue(key, out var value))
            {
                value = new();
                dict.Add(key, value);
            }
            value.Add(input);
        }

        private void Render<T>(
            Dictionary<string, HashSet<GeometryNodeInput>> dict,
            Func<GUIContent, T, GUILayoutOption[], T> field,
            Func<GeometryNodeInput, List<GeometryNodeInput.Pair<T>>> getter)
        {
            foreach (var (key, targets) in dict)
            {
                T oldValue = getter(targets.First()).Find(x => x.key == key).value;
                T newValue = field(new GUIContent(key), oldValue, new GUILayoutOption[0]);
                if (newValue.Equals(oldValue))
                    continue;

                foreach (GeometryNodeInput target in targets)
                {
                    target.SetValue(key, newValue);
                    var inputs = getter(target);
                    inputs.SetValue(key, newValue);

                    HashSet<string> oldKeys = new(inputs.Select(x => x.key));
                    oldKeys.ExceptWith(dict.Keys);
                    inputs.RemoveAll(x => oldKeys.Contains(x.key));
                    EditorUtility.SetDirty(target);
                }
            }
        }
    }
}
