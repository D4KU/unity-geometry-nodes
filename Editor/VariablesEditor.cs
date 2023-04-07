using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace GeometryNodes
{
    /// <summary>
    /// Adds buttons for managing Geometry Nodes next to input variables
    /// </summary>
    [CustomEditor(typeof(Variables))]
    internal class VariablesEditor : Editor
    {
        /// <summary>
        /// Default editor for Variable component
        /// </summary>
        private Editor wrapped;

        public override void OnInspectorGUI()
        {
            if (wrapped == null)
                wrapped = Editor.CreateEditor(target, typeof(LudiqBehaviourEditor));

            EditorGUI.BeginChangeCheck();
            wrapped.OnInspectorGUI();
            bool varChanged = EditorGUI.EndChangeCheck();

            if (!((Component)target).TryGetComponent(out GeometryMachine machine))
                return;

            if (machine.Initialized)
            {
                if (GUILayout.Button("Clear Geometry Nodes"))
                    machine.Clear();
                else if (varChanged)
                    machine.TraverseGraph();
            }
            else
            {
                if (GUILayout.Button("Initialize Geometry Nodes"))
                    machine.Initialize();
            }
        }
    }
}
