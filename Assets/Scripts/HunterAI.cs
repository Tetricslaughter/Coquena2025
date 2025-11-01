using UnityEngine;
using UnityEngine.AI;

public class HunterAI : MonoBehaviour
{
    public enum HunterState { Patrol, Chase, Distracted, Scared }
    public HunterState currentState = HunterState.Patrol;

    public Transform[] patrolPoints;
    public float visionRange = 15f;
    public float hearingRange = 20f;
    public float fleeDuration = 5f;

    private int currentPoint = 0;
    private NavMeshAgent agent;
    private float stateTimer;
    private Transform targetAnimal;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        GoToNextPoint();
    }

    void Update()
    {
        switch (currentState)
        {
            case HunterState.Patrol: Patrol(); break;
            case HunterState.Chase: Chase(); break;
            case HunterState.Distracted: Distracted(); break;
            case HunterState.Scared: Scared(); break;
        }
    }

    void Patrol()
    {
        if (!agent.pathPending && agent.remainingDistance < 1f)
            GoToNextPoint();

        Collider[] hits = Physics.OverlapSphere(transform.position, visionRange);
        foreach (Collider hit in hits)
        {
            if (hit.CompareTag("Animal"))
            {
                targetAnimal = hit.transform;
                currentState = HunterState.Chase;
                break;
            }
        }
    }

    void GoToNextPoint()
    {
        if (patrolPoints.Length == 0) return;
        agent.destination = patrolPoints[currentPoint].position;
        currentPoint = (currentPoint + 1) % patrolPoints.Length;
    }

    void Chase()
    {
        if (!targetAnimal)
        {
            currentState = HunterState.Patrol;
            return;
        }

        agent.destination = targetAnimal.position;
        float distance = Vector3.Distance(transform.position, targetAnimal.position);

        if (distance > visionRange)
            currentState = HunterState.Patrol;
    }

    public void Distract(Vector3 point)
    {
        if (currentState == HunterState.Scared) return;
        currentState = HunterState.Distracted;
        agent.destination = point;
        stateTimer = 3f;
    }

    void Distracted()
    {
        stateTimer -= Time.deltaTime;
        if (stateTimer <= 0) currentState = HunterState.Patrol;
    }

    public void Scare()
    {
        currentState = HunterState.Scared;
        Vector3 fleeDir = (transform.position - Camera.main.transform.position).normalized;
        agent.destination = transform.position + fleeDir * 10f;
        stateTimer = fleeDuration;
    }

    void Scared()
    {
        stateTimer -= Time.deltaTime;
        if (stateTimer <= 0) currentState = HunterState.Patrol;
    }
}
