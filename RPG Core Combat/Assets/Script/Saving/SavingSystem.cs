using UnityEngine;

namespace RPG.Saving
{
    public class SavingSystem : MonoBehaviour 
    {
        public void Save(string saveFile)
        {
            print("Save to " + saveFile);
        }

        public void Load(string saveFile)
        {
            print("Load from " + saveFile);
        }
    }
}