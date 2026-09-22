using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyAI : MonoBehaviour
{
    [Header("Hearing")]
    [SerializeField] private float hearingRange = 10f;
    [SerializeField] private float investigateThreshold = 0.3f;

    [Header("Speed")]
    [SerializeField] private float walkingSpeed = 2f;
    [SerializeField] private float runningSpeed = 6f;

    [Header("Patrol")]
    [SerializeField] private float minWaitTime;
    [SerializeField] private float maxWaitTime;
    [SerializeField] private float chanceToWait;
    [SerializeField] private bool isWaiting;
    private float waitTimer;

    [Header("Refs")]
    [SerializeField] private GameObject player;
    [SerializeField] private FieldOfView fov;

    private NavMeshAgent agent;
    private EnemyState currentState;
    private Vector3 investigateTarget;

    private enum EnemyState
    {
        Patrol,
        Investigate,
        Chase
    }

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        SetState(EnemyState.Patrol);
    }


    private void Update()
    {
        if (fov.canSeePlayer)
        {
            SetState(EnemyState.Chase);
            agent.speed = runningSpeed;
            Debug.Log("Player seen. Began chasing");
        }
        else
        {
            SetState(EnemyState.Patrol);
            agent.speed = walkingSpeed;
            Debug.Log("Lost Player");
        }

        switch (currentState)
        {
            case EnemyState.Patrol:
                Patrol();
                agent.speed = walkingSpeed;
                Debug.Log("Patroling");
                break;

            case EnemyState.Investigate:
                UpdateInvestigate();
                agent.speed = walkingSpeed;
                Debug.Log("Heard something. Began investigating");
                break;

            case EnemyState.Chase:
                ChasePlayer();
                agent.speed = runningSpeed;
                Debug.Log("Chasing");
                break;
        }
    }

    private void SetState(EnemyState newState)
    {
        currentState = newState;
    }

    // =========================
    // PATROL
    // =========================

    private void Patrol()
    {
        if (isWaiting)
        {
            waitTimer -= Time.deltaTime;

            if (waitTimer <= 0f)
            {
                isWaiting = false;
                SetNewPatrolPoint();
            }

            return;
        }

        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {

            if (Random.value < chanceToWait)
            {
                isWaiting = true;
                waitTimer = Random.Range(minWaitTime, maxWaitTime);
                agent.ResetPath(); // stop movement
            }
            else
            {
                SetNewPatrolPoint();
            }
        }
    }
    private void SetNewPatrolPoint()
    {
        Vector3 randomPoint =
            transform.position + Random.insideUnitSphere * 5f;

        randomPoint.y = transform.position.y;

        agent.SetDestination(randomPoint);
    }

    // =========================
    // HEARING
    // =========================

    private void HearNoise(Vector3 noisePosition, float intensity)
    {
        float distance =
            Vector3.Distance(transform.position, noisePosition);

        if (distance > hearingRange)
            return;

        float loudness =
            intensity * (1f - (distance / hearingRange));

        if (loudness >= investigateThreshold & currentState != EnemyState.Chase)
        {
            GoInvestigate(noisePosition);
        }
    }

    private void GoInvestigate(Vector3 position)
    {
        if (currentState == EnemyState.Chase)
            return;

        SetState(EnemyState.Investigate);

        investigateTarget = position;
        agent.SetDestination(investigateTarget);
    }

    private void UpdateInvestigate()
    {
        if (!agent.pathPending &&
            agent.remainingDistance < 0.5f)
        {
            SetState(EnemyState.Patrol);
        }
    }

    private void OnAlarmRaised(Vector3 playerPosition)
    {
        GoInvestigate(playerPosition);
    }

    // =========================
    // CHASE
    // =========================

    private void ChasePlayer()
    {
        if (player == null)
            return;

        agent.SetDestination(player.transform.position);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

      // PlayerDeath playerDeath = other.GetComponent<PlayerDeath>();
      //
      // if (playerDeath != null)
      // {
      //     playerDeath.Die();
      // }
    }
}