using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace GeometryNodes
{
    [CustomEditor(typeof(Variables))]
    internal class VariablesEditor : Editor
    {
        Editor wrapped;

        public override void OnInspectorGUI()
        {
            if (wrapped == null)
                wrapped = Editor.CreateEditor(target, typeof(LudiqBehaviourEditor));

            EditorGUI.BeginChangeCheck();
            wrapped.OnInspectorGUI();
            bool changed = EditorGUI.EndChangeCheck();

            if (!((Component)target).TryGetComponent(out GeometryMachine input))
                return;

            if (input.Initialized)
            {
                if (GUILayout.Button("Clear Geometry Nodes"))
                    input.Clear();
                else if (changed)
                    input.TraverseGraph();
            }
            else
            {
                if (GUILayout.Button("Initialize Geometry Nodes"))
                    input.Initialize();
            }
        }
    }
}
