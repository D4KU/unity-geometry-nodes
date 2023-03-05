using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;

namespace GeometryNodes
{
    [RequireComponent(typeof(ScriptMachine))]
    public class GeometryNodeInput : MonoBehaviour
    {
        public Dictionary<string, float> floats = new();
        public Dictionary<string, int> ints = new();
        public Dictionary<string, bool> bools = new();

        public void Reset<T>(string eventKey)
            => Trigger(GeometryInput<T>.GetResetCommand(eventKey), default(T));

        public void Trigger<T>(string eventKey, T value)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying
                && TryGetComponent(out ScriptMachine script)
                && script.graphData == null)
            {
                Invoke<ScriptMachine>(script, "Awake");
                Invoke<ScriptMachine>(script, "OnEnable");
            }
#endif

            EventBus.Trigger(GeometryInput<T>.HOOK_NAME, gameObject, (eventKey, value));
        }

        private void Invoke<T>(T obj, string methodName) => typeof(T)
            .GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance)
           ?.Invoke(obj, null);
    }
}
