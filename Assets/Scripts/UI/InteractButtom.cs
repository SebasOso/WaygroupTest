using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "InteractButtom", menuName = "UI/Make New Interact Buttom", order = 1)]
public class InteractButtom : ScriptableObject
{
    [Header("InteractButtoms")]
    [SerializeField] GameObject imagePrefab;

    const string interactButtomName = "InteractButtom";
    public void Spawn(Transform imageSocket)
    {
        DestroyOldUIButtom(imageSocket);
        if(imagePrefab == null){return;}
        GameObject interactButtom = Instantiate(imagePrefab, imageSocket);
        interactButtom.name = interactButtomName;
    }
    private void DestroyOldUIButtom(Transform imageSocket)
    {
        Transform oldInteractButtom = imageSocket.Find(interactButtomName);
        if(oldInteractButtom == null){return;}

        oldInteractButtom.name = "Destroying";
        Destroy(oldInteractButtom.gameObject);
    }
}
