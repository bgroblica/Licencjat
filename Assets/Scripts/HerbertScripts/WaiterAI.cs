using UnityEngine;
using UnityEngine.AI;

public class WaiterAI : MonoBehaviour
{
    [SerializeField] private float minWaitTime = 1f;
    [SerializeField] private float maxWaitTime = 5f;

    private NavMeshAgent agent;
    private Table[] tables;

    private float timer;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        tables = FindObjectsByType<Table>(FindObjectsSortMode.None);
    }

    private void Start()
    {
        ChooseDestination();
    }

    private void Update()
    {
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            float waitingTime = Random.Range(minWaitTime, maxWaitTime);

            timer += Time.deltaTime;

            if (timer >= waitingTime)
            {
                timer = 0f;
                ChooseDestination();
            }
        }
    }

    private void ChooseDestination()
    {
        if (tables.Length == 0)
            return;

        Table randomTable = tables[Random.Range(0, tables.Length)];

        agent.SetDestination(randomTable.transform.position);
    }
}