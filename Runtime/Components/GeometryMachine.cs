using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;

namespace GeometryNodes
{
    [RequireComponent(typeof(ScriptMachine))]
    public class GeometryMachine : MonoBehaviour
    {
        public ScriptMachine Script => GetComponent<ScriptMachine>();
        public bool Initialized { get; private set; }

        private void OnDestroy() => Clear();

        public void TraverseGraph()
        {
            EventBus.Trigger(Entry.HOOK_NAME, gameObject);
        }

        public void Initialize()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                ScriptMachine script = Script;
                UnityEditor.AssemblyReloadEvents.beforeAssemblyReload += Clear;
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

            if (script.graph != null)
                foreach (IUnit unit in script.graph.units)
                    if (unit is Entry entry)
                        entry.trigger.ClearDownstream();

#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                UnityEditor.AssemblyReloadEvents.beforeAssemblyReload -= Clear;
                Invoke(script, "OnDisable");
                Invoke(script, "OnDestroy");
            }
#endif
        }

        private static void Invoke<T>(T obj, string methodName) => typeof(T)
            .GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance)
            ?.Invoke(obj, null);
    }
}
