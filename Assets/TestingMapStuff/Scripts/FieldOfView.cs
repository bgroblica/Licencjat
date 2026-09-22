using System.Collections;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    [Header("FOV Stats")]
    public float radius;
    [Range(0, 360)]
    public float angle;

    [Header("Refs")]

    public GameObject playerRef;

    [Header("Layers Masks")]

    public LayerMask playerMask;
    public LayerMask obstructionMask;

    [Header("Gizmos")]
    public bool showGizmos;

    public bool canSeePlayer;

    public Vector3 PlayerPosition
    {
        get
        {
            if (playerRef == null)
                return transform.position;

            return playerRef.transform.position;
        }
    }

    private void Start()
    {
        StartCoroutine(FOVRoutine());
    }
    private IEnumerator FOVRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(0.1f);

        while (true)
        {
            yield return wait;
            FieldOfViewCheck();
        }
    }

    private void FieldOfViewCheck()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, playerMask);

        if (rangeChecks.Length != 0)
        {
            Transform target = rangeChecks[0].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask))
                    canSeePlayer = true;
                else
                    canSeePlayer = false;
            }
            else
                canSeePlayer = false;
        }
        else if (canSeePlayer)
            canSeePlayer = false;
    }
}
