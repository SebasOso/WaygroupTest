using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG.Core
{
    public class PersistantObject : MonoBehaviour
    {
        [SerializeField] private GameObject player;
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.buildIndex == 1)
            {
                transform.position = new Vector3(25.74219f, 3.715f, -56.02815f);
                player.transform.position = Vector3.zero;
            }
        }
        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }
}
