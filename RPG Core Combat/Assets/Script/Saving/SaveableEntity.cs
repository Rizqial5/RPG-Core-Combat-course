using RPG.Core;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

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
            return new SerializeableVector3(transform.position);
        }

        public void RestoreState(object state)
        {   
            SerializeableVector3 position = (SerializeableVector3)state;
            GetComponent<NavMeshAgent>().enabled = false;
            transform.position = position.ToVector();
            GetComponent<NavMeshAgent>().enabled = true;
            GetComponent<ActionScheduler>().CancelCurrentAction();
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