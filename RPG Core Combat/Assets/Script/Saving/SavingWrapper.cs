using UnityEngine;

namespace RPG.Saving
{
    public class SavingWrapper : MonoBehaviour 
    {
        const string defaultSave = "Save";

        SavingSystem savingSystem;

        private void Start() {
            savingSystem = GetComponent<SavingSystem>();
            Load();
        }
        private void Update() 
        {
            
            if(Input.GetKeyDown(KeyCode.S))
            {
                Save();
            }
            if (Input.GetKeyDown(KeyCode.L))
            {
                Load();
            }

        }

        public void Load()
        {
            savingSystem.Load(defaultSave);
        }

        public void Save()
        {
            savingSystem.Save(defaultSave);
        }
    }
}