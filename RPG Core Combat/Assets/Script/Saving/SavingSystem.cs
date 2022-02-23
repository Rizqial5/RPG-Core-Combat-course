using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System;
using System.Runtime.Serialization.Formatters.Binary;

namespace RPG.Saving
{
    public class SavingSystem : MonoBehaviour 
    {
        public void Save(string saveFile)
        {
            string path = GetSaveFileFromPath(saveFile);
            print("Save to " + path);
            using (FileStream stream = File.Open(path, FileMode.Create))
            {
                
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                
                binaryFormatter.Serialize(stream, CaptureState());

                
            }
        }

        

        public void Load(string saveFile)
        {
            string path = GetSaveFileFromPath(saveFile);
            print("Load from " + GetSaveFileFromPath(saveFile) );
            using (FileStream stream = File.Open(path, FileMode.Open))
            {
                
                

                BinaryFormatter binaryFormatter = new BinaryFormatter();
                RestorState(binaryFormatter.Deserialize(stream));
                

                
                
                
            }
        }

       
        private object CaptureState()
        {
            Dictionary<string, object> state = new Dictionary<string, object>();
            foreach (SaveableEntity saveable in FindObjectsOfType<SaveableEntity>() )
            {
                state[saveable.GetUniqeIdentifier()] = saveable.CaptureState();
            }
            return null;
        }

        private void RestorState(object state)
        {
            Dictionary<string, object> stateDict = (Dictionary<string, object>) state;
            foreach (SaveableEntity saveable in FindObjectsOfType<SaveableEntity>() )
            {
                saveable.RestoreState(stateDict[saveable.GetUniqeIdentifier()]);
            }
        }


        private string GetSaveFileFromPath(string saveFile)
        {
            return Path.Combine(Application.persistentDataPath, saveFile + ".sav");
        }
    }
}