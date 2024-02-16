using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class Targeter : MonoBehaviour
{
    [SerializeField] private CinemachineTargetGroup cinemachineTargetGroup;

    private List<Target> targets = new List<Target>();

    private Camera mainCamera;

    public Target currentTarget {get; private set;}
    [SerializeField] private MusicHandler musicHandler;
    private void Start() 
    {
        mainCamera = Camera.main;
    }
    private void OnTriggerEnter(Collider other) 
    {
        if (!other.TryGetComponent<Target>(out Target target))
        {
           return;
        }
        targets.Add(target);
        target.OnDestroyed += RemoveTarget;
    }
    private void OnTriggerExit(Collider other) 
    {
        if (!other.TryGetComponent<Target>(out Target target))
        {
            return;
        } 
        RemoveTarget(target);
    }
    public bool SelectTarget()
    {
        if(targets.Count == 0){return false;}

        Target closestTarget = null;
        float closestTargetDistance = Mathf.Infinity;

        foreach(Target target in targets)
        {
            Vector2 viewPos = mainCamera.WorldToViewportPoint(target.transform.position);
            if (viewPos.x < 0 || viewPos.x > 1 || viewPos.y < 0 || viewPos.y > 1)
            {
                continue;
            }
            Vector2 toCenter = viewPos - new Vector2(0.5f,0.5f);
            if(toCenter.sqrMagnitude < closestTargetDistance)
            {
                closestTarget = target;
                closestTargetDistance = toCenter.sqrMagnitude;
            }
        }
        if(closestTarget == null){return false;}
        currentTarget = closestTarget;
        cinemachineTargetGroup.AddMember(currentTarget.transform, 1f, 2f);

        return true;
    }
    public void Cancel()
    {
        if(currentTarget == null){return;}
        cinemachineTargetGroup.RemoveMember(currentTarget.transform);
        currentTarget = null;
    }
    private void RemoveTarget(Target targerToRemove)
    {
        if(currentTarget == targerToRemove)
        {
            cinemachineTargetGroup.RemoveMember(currentTarget.transform);
            currentTarget = null;
        }
        targerToRemove.OnDestroyed -= RemoveTarget;
        targets.Remove(targerToRemove);
    }
}
