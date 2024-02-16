using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class MoveTarget : MonoBehaviour
{
    [SerializeField] InputReader inputReader;
    public float velocity = 1.5f;
    private Rigidbody rb;
    private void Start() 
    {
        rb = GetComponent<Rigidbody>();    
    }
    private void Update() 
    {
        
    }
}
