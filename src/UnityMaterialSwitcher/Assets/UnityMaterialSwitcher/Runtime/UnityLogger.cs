using UnityEngine;

namespace MaterialSwitcher
{
    public static class UnityLogger
    {
        /// <summary>
        /// Logs a message to the Unity Console 
        /// only when DEVELOPMENT_BUILD or UNITY_EDITOR is defined.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        [
            System.Diagnostics.Conditional("DEVELOPMENT_BUILD"),
            System.Diagnostics.Conditional("UNITY_EDITOR"),
        ]
        [HideInCallstack]
        public static void Log(object message)
        {
            Debug.Log(message);
        }

        /// <summary>
        /// Logs an error message to the Unity Console 
        /// only when DEVELOPMENT_BUILD or UNITY_EDITOR is defined.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        [
            System.Diagnostics.Conditional("DEVELOPMENT_BUILD"),
            System.Diagnostics.Conditional("UNITY_EDITOR"),
        ]
        [HideInCallstack]
        public static void LogError(object message)
        {
            Debug.LogError(message);
        }
    }
}