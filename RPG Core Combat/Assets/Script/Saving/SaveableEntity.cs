using UnityEditor;
using UnityEngine;

namespace RPG.Saving
{
    [ExecuteAlways]
    public class SaveableEntity : MonoBehaviour    
    {
        [SerializeField] string uniqueIdentifier = "";
        public string GetUniqeIdentifier()
        {
            return uniqueIdentifier;
        }

        public object CaptureState()
        {
            print("Capture state for " + GetUniqeIdentifier());
            return null;
        }

        public void RestoreState(object state)
        {   
            print("Restoring state for " + GetUniqeIdentifier());
        }


#if UNITY_EDITOR
        private void Update() {
            if(Application.IsPlaying(gameObject)) return;
            if(string.IsNullOrEmpty(gameObject.scene.path)) return;

            SerializedObject serializedObject = new SerializedObject(this);
            SerializedProperty property = serializedObject.FindProperty("uniqueIdentifier");

            if( string.IsNullOrEmpty(property.stringValue))
            {
                property.stringValue = System.Guid.NewGuid().ToString();
                serializedObject.ApplyModifiedProperties();
            }


        }
#endif
    }
}