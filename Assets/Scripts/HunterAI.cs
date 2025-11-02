using UnityEngine;
using UnityEngine.AI;

public class HunterAI : MonoBehaviour
{
    public enum HunterState { Patrol, Chase, Distracted, Scared,Aim,Shoot,Stunned }
    public HunterState currentState = HunterState.Patrol;
    Animator animator;
    public Transform[] patrolPoints;
    public float visionRange = 15f;
    public float hearingRange = 20f;
    public float fleeDuration = 5f;
    public float patrolSpeed = 3.5f;
    public float chaseSpeed = 1.5f;
    private int currentPoint = 0;
    private NavMeshAgent agent;
    private float stateTimer;
    private Transform targetAnimal;
    public float stunDuration = 3f;

    public float shootRange = 7f;
    public Transform shootPoint;
    public Transform aimTarget;
    public GameObject bulletPrefab;       
    public float aimDuration = 3f;
    private float aimTimer;

    Transform playerPos;
    private void Awake()
    {

        playerPos = GameObject.FindGameObjectWithTag("Player").transform;
    }
    void Start()
    {
        
        agent = GetComponent<NavMeshAgent>();
      //  GoToNextPoint();
    }

    void Update()
    {
        
        switch (currentState)
        {
            case HunterState.Patrol: Patrol(); break;
            case HunterState.Chase: Chase(); break;
            case HunterState.Distracted: Distracted(); break;
            case HunterState.Scared: Scared(); break;  
            case HunterState.Aim: Aim(); break;
            case HunterState.Shoot:Shoot(); break;
            case HunterState.Stunned:Stunned(); break;
        }
        //if(Input.GetMouseButtonDown(0))
        //{
        //   EnterDistractedState();
        //}
    }
    void Aim() 
    {
        if (currentState != HunterState.Stunned && currentState != HunterState.Scared) 
        {
            Debug.Log("Aiming");
            agent.isStopped = true;
            Turn();

            aimTimer -= Time.deltaTime;
            if (aimTimer <= 0)
            {

                agent.isStopped = false;
                Debug.Log("Shooting");
                currentState = HunterState.Shoot;

            }
            if (Vector3.Distance(transform.position, targetAnimal.position) > shootRange)
            {
                aimTimer = aimDuration;
                agent.isStopped = false;
                aimTimer = aimDuration;
                currentState = HunterState.Patrol;
            }
            if (!targetAnimal)
            {
                aimTimer = aimDuration;
                agent.isStopped = false;
                aimTimer = aimDuration;
                currentState = HunterState.Patrol;
            }
        }
        
       
    }
    void Shoot() 
    {
        aimTimer = aimDuration;
        GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation);
        bullet.transform.LookAt(aimTarget);        
        currentState = HunterState.Patrol;
    }
   
    void Patrol()
    {
        if (agent.remainingDistance < 0.4f)
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
        if (distance <= shootRange)
        {
            aimTimer = stunDuration;
            aimTarget = targetAnimal;
            currentState = HunterState.Aim;
            agent.isStopped = true;           
        }
        
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
    void Turn()
    {
        
            Vector3 objective = targetAnimal.position;
            objective.y = transform.position.y;
            Vector3 lookDirection = objective - transform.position;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookDirection), Time.deltaTime * 6);     
    }
    void Stunned()
    {
        stateTimer -= Time.deltaTime;
        if (stateTimer <= 0) 
        {
            agent.isStopped = false;
            currentState = HunterState.Patrol; 
        }
    }
    public void EnterStunnedState()
    {
        currentState = HunterState.Stunned;
        stateTimer = stunDuration;
        agent.isStopped = true;
    }
    
    public void EnterDistractedState()
    {
      if (currentState == HunterState.Scared) return;
      if (currentState == HunterState.Distracted) return;
      agent.isStopped = false;
      agent.destination = playerPos.position;
      stateTimer = 2f;
        currentState = HunterState.Distracted;
    }
    
    void Distracted()
    {
        
        if (agent.remainingDistance <= 1) 
        {
            agent.isStopped = true;
            if (stateTimer <= 0) 
            {
                agent.isStopped = false;
                currentState = HunterState.Patrol;
            }
            else
            {
                stateTimer -= Time.deltaTime;
            }
                
        }
    }
}
    
