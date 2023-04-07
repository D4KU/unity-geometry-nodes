using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;

namespace GeometryNodes
{
    /// <summary>
    /// Exposes methods to initialize and reset Geometry Nodes inside
    /// the graph of the neighboring <see cref="ScriptMachine"/>
    /// </summary>
    [RequireComponent(typeof(ScriptMachine))]
    public class GeometryMachine : MonoBehaviour
    {
        public ScriptMachine Script => GetComponent<ScriptMachine>();
        public bool Initialized { get; private set; }

        private void OnDestroy() => Clear();

        /// <summary>
        /// Trigger every <see cref="Entry"/> event in the graph linked to
        /// this object
        /// </summary>
        public void TraverseGraph() => EventBus.Trigger(Entry.HOOK_NAME, gameObject);

        public void Initialize()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                ScriptMachine script = Script;
                UnityEditor.AssemblyReloadEvents.beforeAssemblyReload += Clear;

                /// Initialize <see cref="ScriptMachine"/>
                Invoke(script, "Awake");
                Invoke(script, "OnEnable");
                Invoke(script, "Start");
            }
#endif

            Initialized = true;
            TraverseGraph();
        }

        public void Clear()
        {
            ScriptMachine script = Script;
            Initialized = false;

            // Clear Geometry Nodex after every entry node
            if (script.graph != null)
                foreach (IUnit unit in script.graph.units)
                    if (unit is Entry entry)
                        entry.trigger.ClearDownstream();

#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                UnityEditor.AssemblyReloadEvents.beforeAssemblyReload -= Clear;

                /// Reset <see cref="ScriptMachine"/>
                Invoke(script, "OnDisable");
                Invoke(script, "OnDestroy");
            }
#endif
        }

        /// <summary>
        /// Invoke a private method with the given name on the given object
        /// </summary>
        private static void Invoke<T>(T obj, string methodName) => typeof(T)
            .GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance)
            ?.Invoke(obj, null);
    }
}
