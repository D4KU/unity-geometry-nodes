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

        private void Initialize()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                UnityEditor.AssemblyReloadEvents.beforeAssemblyReload += Clear;
                var script = Script;
                if (script.graphData == null)
                {
                    Invoke(script, "Awake");
                    Invoke(script, "OnEnable");
                    Invoke(script, "Start");
                }
            }
#endif
            foreach (var pair in floats)
                Initialize(pair.key, pair.value);
            foreach (var pair in ints)
                Initialize(pair.key, pair.value);
            foreach (var pair in bools)
                Initialize(pair.key, pair.value);

            initialized = true;
        }

        [ContextMenu(nameof(Clear))]
        public void Clear()
        {
            initialized = false;

            foreach (var pair in floats)
                Clear<float>(pair.key);
            foreach (var pair in ints)
                Clear<int>(pair.key);
            foreach (var pair in bools)
                Clear<bool>(pair.key);

#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                UnityEditor.AssemblyReloadEvents.beforeAssemblyReload -= Clear;
                var script = Script;
                if (script.graphData != null)
                {
                    Invoke(script, "OnDisable");
                    Invoke(script, "OnDestroy");
                }
            }
#endif
        }

        public void SetValue<T>(string eventKey, T value)
        {
            if (!initialized)
                Initialize();
            Trigger(eventKey, value);
        }

        private void Initialize<T>(string eventKey, T value)
            => Trigger(Input<T>.GetInitCommand(eventKey), value);

        private void Clear<T>(string eventKey)
            => Trigger(Input<T>.GetClearCommand(eventKey), default(T));

        private void Trigger<T>(string eventKey, T value)
            => EventBus.Trigger(Input<T>.HOOK_NAME, gameObject, (eventKey, value));

        private static void Invoke<T>(T obj, string methodName) => typeof(T)
            .GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance)
            ?.Invoke(obj, null);
    }
}
