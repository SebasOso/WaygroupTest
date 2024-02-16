using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateWeapon : MonoBehaviour
{
    public Vector3 ejeDeRotacion = Vector3.up; 
    public float velocidadRotacion = 100.0f; 
    void Update()
    {
        transform.Rotate(ejeDeRotacion, velocidadRotacion * Time.deltaTime);
    }
}
