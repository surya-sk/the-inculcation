using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace CultGame.Saving
{
    /// <summary>
    /// Makes an object saveable upon attaching this script
    /// </summary>
    [ExecuteAlways]
    public class SaveableObject : MonoBehaviour
    {
        public string uniqueId = "";
        static Dictionary<string, SaveableObject> fullScan = new Dictionary<string, SaveableObject>();
#if UNITY_EDITOR
        private void Update()
        {
            if (string.IsNullOrEmpty(gameObject.scene.path))
            {
                return;
            }
            SerializedObject serializedObject = new SerializedObject(this);
            SerializedProperty property = serializedObject.FindProperty("uniqueId");
            if (string.IsNullOrEmpty(property.stringValue) || !isUnique(property.stringValue))
            {
                property.stringValue = System.Guid.NewGuid().ToString();
                serializedObject.ApplyModifiedProperties();
            }
            fullScan[property.stringValue] = this;
        }
#endif

        public string GetUniqueIdentifier()
        {
            return uniqueId;
        }

        /// <summary>
        /// Checks if the id is unique
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private bool isUnique(string id)
        {
            if (!fullScan.ContainsKey(id) || fullScan[id] == this)
            {
                return true;
            }
            if (fullScan[id] == null || fullScan[id].GetUniqueIdentifier() != id)
            {
                fullScan.Remove(id);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Saves current state of the object 
        /// </summary>
        /// <returns></returns>
        public object CaptureState()
        {
            Dictionary<string, object> state = new Dictionary<string, object>();
            foreach (ISaveable saveable in GetComponents<ISaveable>())
            {
                state[saveable.GetType().ToString()] = saveable.CaptureState();
            }
            return state;
        }

        /// <summary>
        /// Returns the saved state of the object
        /// </summary>
        /// <param name="state"></param>
        public void RestoreState(object state)
        {
            Dictionary<string, object> dicState = (Dictionary<string, object>)state;
            foreach (ISaveable saveable in GetComponents<ISaveable>())
            {
                string saveableType = saveable.GetType().ToString();
                if (dicState.ContainsKey(saveableType))
                {
                    saveable.RestoreState(dicState[saveableType]);
                }
            }
        }
    }
}
