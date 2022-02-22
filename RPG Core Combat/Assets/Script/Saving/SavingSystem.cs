using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System;

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
                Transform playerPosition = GetPlayerPosition();
                byte[] buffer = SerializeVector(playerPosition.position);
                stream.Write(buffer, 0, buffer.Length);
            }
        }

        

        public void Load(string saveFile)
        {
            string path = GetSaveFileFromPath(saveFile);
            print("Load from " + GetSaveFileFromPath(saveFile) );
            using (FileStream stream = File.Open(path, FileMode.Open))
            {
                byte[] buffer = new byte[stream.Length];
                stream.Read(buffer,0,buffer.Length);

                Transform playerPosition = GetPlayerPosition();
                playerPosition.position = DeserializeVector(buffer);
                
            }
        }

        private Transform GetPlayerPosition()
        {
            return GameObject.FindWithTag("Player").transform;
        }
        private byte[] SerializeVector(Vector3 vector)
        {
            byte [] vectorBytes = new byte[3 * 4];
            BitConverter.GetBytes(vector.x).CopyTo(vectorBytes, 0);
            BitConverter.GetBytes(vector.y).CopyTo(vectorBytes, 4);
            BitConverter.GetBytes(vector.z).CopyTo(vectorBytes, 8);
            return vectorBytes;
        }

        private Vector3 DeserializeVector(byte[] buffer)
        {
            Vector3 result = new Vector3();
            result.x = BitConverter.ToSingle(buffer, 0);
            result.y = BitConverter.ToSingle(buffer, 4);
            result.x = BitConverter.ToSingle(buffer, 8);
            return result;

        }
        private string GetSaveFileFromPath(string saveFile)
        {
            return Path.Combine(Application.persistentDataPath, saveFile + ".sav");
        }
    }
}