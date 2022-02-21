using UnityEngine;

namespace RPG.Saving
{
    public class SavingWrapper : MonoBehaviour 
    {
        const string defaultSave = "Save";

        SavingSystem savingSystem;

        private void Start() {
            savingSystem = GetComponent<SavingSystem>();
        }
        private void Update() 
        {
            
            if(Input.GetKeyDown(KeyCode.S))
            {
                savingSystem.Save(defaultSave);
            }
            if(Input.GetKeyDown(KeyCode.L))
            {
                savingSystem.Load(defaultSave);
            }

        }
    }
}