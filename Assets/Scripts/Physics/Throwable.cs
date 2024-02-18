using RPG.Inventories;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throwable : Interactable
{
    private bool canGrab = false;
    private void Update()
    {
        if(ThrowManager.Instance.isBeingCarried)
        {
            OnLoseFocus();
        }
    }
    private void Grab()
    {
        if (!canGrab) { return; }
        ThrowManager.Instance.Grab(this);
        OnLoseFocus();
    }
    public override void OnInteract()
    {
        Grab();
    }

    public override void OnFocus()
    {
        canGrab = true;
        GetComponent<Outline>().enabled = true;
    }

    public override void OnLoseFocus()
    {
        canGrab = false;
        GetComponent<Outline>().enabled = false;
    }
}
