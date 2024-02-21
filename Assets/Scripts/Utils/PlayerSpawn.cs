using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSpawn : MonoBehaviour
{
    [SerializeField] private GameObject player;
    private void Start()
    {
        StartCoroutine(Spawn());
    }
    private IEnumerator Spawn()
    {
        yield return new WaitForSeconds(1f);
        GameObject player = GameObject.FindGameObjectWithTag("PlayerPro");
        this.player = player;
        if (player != null)
        {
            player.transform.position = new Vector3(25.74219f, 3.715f, -56.02815f);
        }
    }
}
