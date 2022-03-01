using System.Collections;
using UnityEngine;
using RPG.Saving;


namespace RPG.SceneManagement
{
    public class SavingWrapper : MonoBehaviour 
    {
        const string defaultSave = "Save";
        [SerializeField] float fadeIntime = 1f;
        SavingSystem savingSystem;

        private IEnumerator Start() {
            Fade fade = FindObjectOfType<Fade>();
            savingSystem = GetComponent<SavingSystem>();

            fade.FadeOutImmediate();
            
            yield return savingSystem.LoadLastScene(defaultSave);
            yield return fade.FadeIn(fadeIntime);

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