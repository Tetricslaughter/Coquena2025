using UnityEngine;
using UnityEngine.AI;

public class AnimalAI : MonoBehaviour
{
    public enum AnimalState { Wander, Flee, Captured }
    public AnimalState currentState = AnimalState.Wander;

    public float wanderRadius = 10f;
    public float wanderTimer = 5f;
    public float fleeDistance = 10f;

    private NavMeshAgent agent;
    private float timer;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        timer = wanderTimer;
    }

    void Update()
    {
        switch (currentState)
        {
            case AnimalState.Wander: Wander(); break;
            case AnimalState.Flee: Flee(); break;
            case AnimalState.Captured: break;
        }
    }

    void Wander()
    {
        timer += Time.deltaTime;
        if (timer >= wanderTimer)
        {
            Vector3 newPos = RandomNavSphere(transform.position, wanderRadius);
            agent.SetDestination(newPos);
            timer = 0;
        }

        Collider[] threats = Physics.OverlapSphere(transform.position, 7f);
        foreach (Collider hit in threats)
        {
            if (hit.CompareTag("Hunter"))
            {
                Vector3 dir = (transform.position - hit.transform.position).normalized;
                agent.SetDestination(transform.position + dir * fleeDistance);
                currentState = AnimalState.Flee;
                break;
            }
        }
    }

    void Flee()
    {
        if (agent.remainingDistance < 1f)
            currentState = AnimalState.Wander;
    }

    public static Vector3 RandomNavSphere(Vector3 origin, float dist)
    {
        Vector3 randomDirection = Random.insideUnitSphere * dist;
        randomDirection += origin;
        NavMeshHit navHit;
        NavMesh.SamplePosition(randomDirection, out navHit, dist, -1);
        return navHit.position;
    }

    public void Capture()
    {
        currentState = AnimalState.Captured;
        agent.isStopped = true;
        gameObject.SetActive(false);
    }
}
