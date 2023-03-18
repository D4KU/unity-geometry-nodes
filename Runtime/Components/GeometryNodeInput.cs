using System;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;

namespace GeometryNodes
{
    [RequireComponent(typeof(ScriptMachine))]
    public class GeometryNodeInput : MonoBehaviour
    {
        [Serializable]
        public struct Pair<T>
        {
            public string key;
            public T value;
        }

        [HideInInspector] public List<Pair<float>> floats = new();
        [HideInInspector] public List<Pair<int>> ints = new();
        [HideInInspector] public List<Pair<bool>> bools = new();
        private bool initialized = false;

        public ScriptMachine Script => GetComponent<ScriptMachine>();

        private void OnDestroy() => Clear();

        private void Initialize(ScriptMachine script)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                UnityEditor.AssemblyReloadEvents.beforeAssemblyReload += Clear;
                Invoke(script, "Awake");
                Invoke(script, "OnEnable");
                Invoke(script, "Start");
            }
#endif
            foreach (var pair in floats)
                SetValue(pair.key, pair.value, script);
            foreach (var pair in ints)
                SetValue(pair.key, pair.value, script);
            foreach (var pair in bools)
                SetValue(pair.key, pair.value, script);

            initialized = true;
        }

        [ContextMenu(nameof(Clear))]
        public void Clear()
        {
            initialized = false;
            ScriptMachine script = Script;

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

        public void SetValue<T>(string key, T value)
        {
            ScriptMachine script = Script;
            if (script.graph == null)
            {
                initialized = false;
                return;
            }

            if (initialized)
                SetValue<T>(key, value, script);
            else
                Initialize(script);

            EventBus.Trigger(Entry.HOOK_NAME, gameObject);
        }

        private void SetValue<T>(string key, T value, ScriptMachine script)
        {
            foreach (IUnit unit in script.graph.units)
                if (unit is Input<T> i && i.key == key)
                    i.value = value;
        }

        private static void Invoke<T>(T obj, string methodName) => typeof(T)
            .GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance)
            ?.Invoke(obj, null);
    }
}
