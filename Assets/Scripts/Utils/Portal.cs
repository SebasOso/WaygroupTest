using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    [SerializeField] private GameObject Persistant;
    [SerializeField] private GameObject player;
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.LoadSceneAsync(1);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex == 1)
        {
            Persistant.transform.position = new Vector3(25.74219f, 3.715f, -56.02815f);
            player.transform.position = Vector3.zero;
        }
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
