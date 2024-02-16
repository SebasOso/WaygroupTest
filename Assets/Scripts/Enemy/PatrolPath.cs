using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolPath : MonoBehaviour
{
    [SerializeField] private float markSize = 0.5f;
    private void OnDrawGizmos() 
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            int j = GetNextIndex(i);
            Gizmos.DrawSphere(GetMark(i), markSize);
            Gizmos.DrawLine(GetMark(i), GetMark(j));
        }
    }

    public int GetNextIndex(int i)
    {
        if(i + 1 == transform.childCount)
        {
            return 0;
        }
        return i + 1;
    }

    public Vector3 GetMark(int i)
    {
        return transform.GetChild(i).position;
    }
}
