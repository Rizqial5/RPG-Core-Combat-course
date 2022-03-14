using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.SceneManagement;

namespace RPG.Saving
{
    public class SavingSystem : MonoBehaviour 
    {


        public IEnumerator LoadLastScene(string saveFile)
        {
            Dictionary<string, object> state = LoadFile(saveFile);
            if(state.ContainsKey("lastSceneBuildIndex"))
            {
                int buildIndex = (int)state["lastSceneBuildIndex"];
                if(buildIndex != SceneManager.GetActiveScene().buildIndex)
                {
                    yield return SceneManager.LoadSceneAsync(buildIndex);
                }
            }
            
            RestoreState(state);

        }
        public void Save(string saveFile)
        {
            Dictionary<string, object> state = LoadFile(saveFile);
            CaptureState(state);
            
            SaveFile(saveFile, state);

        }

        

        public void Load(string saveFile)
        {
            
            RestoreState(LoadFile(saveFile));
            
        }

        public void Delete(string saveFile)
        {
            File.Delete(GetSaveFileFromPath(saveFile));
        }

        private Dictionary<string, object> LoadFile(string saveFile)
        {
            string path = GetSaveFileFromPath(saveFile);
            if(!File.Exists(path))
            {
                return new Dictionary<string, object>();
            }
            using (FileStream stream = File.Open(path, FileMode.Open))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                return (Dictionary<string, object>)formatter.Deserialize(stream);
            }
        }

        private void SaveFile(string saveFile, object state)
        {
            string path = GetSaveFileFromPath(saveFile);
            print(" Save to " + path);
            using (FileStream stream = File.Open(path, FileMode.Create))
            {
                
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                
                binaryFormatter.Serialize(stream, state);
            }
        }

       
        private void CaptureState(Dictionary<string, object> state)
        {
            foreach (SaveableEntity saveable in FindObjectsOfType<SaveableEntity>() )
            {
                state[saveable.GetUniqeIdentifier()] = saveable.CaptureState();
            }

            state["lastSceneBuildIndex"] = SceneManager.GetActiveScene().buildIndex;
        }

        private void RestoreState(Dictionary<string, object> state)
        {
            foreach (SaveableEntity saveable in FindObjectsOfType<SaveableEntity>() )
            {
                string id = saveable.GetUniqeIdentifier();
                if(state.ContainsKey(id))
                {
                    saveable.RestoreState(state[id]);
                }
                
            }
        }


        private string GetSaveFileFromPath(string saveFile)
        {
            return Path.Combine(Application.persistentDataPath, saveFile + ".sav");
        }
    }
}