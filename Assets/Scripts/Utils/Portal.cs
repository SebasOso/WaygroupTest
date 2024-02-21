using UnityEngine;
using UnityEngine.SceneManagement;
using Waygroup;

public class Portal : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            InputManager.Instance.Unsuscribe();
            SceneManager.LoadSceneAsync(1);
        }
    }
}
