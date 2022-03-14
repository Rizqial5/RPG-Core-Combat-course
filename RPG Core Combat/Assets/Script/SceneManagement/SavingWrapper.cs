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

        private void Awake() {
            StartCoroutine(LoadLastScene());
        }
        private IEnumerator LoadLastScene() {
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
            if (Input.GetKeyDown(KeyCode.Delete))
            {
                Delete();
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

        public void Delete()
        {
            savingSystem.Delete(defaultSave);
        }
    }
}