using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class PersistantObject : MonoBehaviour
    {
        [SerializeField] GameObject persistentObjectPrefab;
        static bool hasSpawned = false;
        private void Awake() 
        {
            if(hasSpawned){return;}
            SpawnPersistantObjects();
            hasSpawned = true;
        }
        private void SpawnPersistantObjects()
        {
            GameObject persistantObject = Instantiate(persistentObjectPrefab);
            DontDestroyOnLoad(persistantObject);
        }
    }
}
