using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class HunterAI2 : MonoBehaviour
{
    public enum State { Patrol, Chasing, Attacking, Distracted, Stunned, Huir }
    public State CurrentState { get; private set; } = State.Patrol;

    public NavMeshAgent agent;
    public Animator animator;

    public float distractedDuration = 4f;
    public float stunnedDuration = 3f;

    void Awake()
    {
        if (agent == null) agent = GetComponent<NavMeshAgent>();
        if (animator == null) animator = GetComponentInChildren<Animator>();
    }

    public void EnterDistractedState()
    {
        if (CurrentState == State.Stunned || CurrentState == State.Huir || CurrentState == State.Attacking) return;
        StopAllCoroutines();
        StartCoroutine(DoDistracted());
    }

    IEnumerator DoDistracted()
    {
        CurrentState = State.Distracted;
        agent.isStopped = true;
        // anim
        if (animator != null) animator.SetTrigger("Distracted");
        yield return new WaitForSeconds(distractedDuration);
        agent.isStopped = false;
        CurrentState = State.Patrol; // vuelve a su comportamiento
    }

    public void EnterStunnedState()
    {
        StopAllCoroutines();
        StartCoroutine(DoStunned());
    }

    IEnumerator DoStunned()
    {
        CurrentState = State.Stunned;
        agent.isStopped = true;
        if (animator != null) animator.SetTrigger("Stunned");
        yield return new WaitForSeconds(stunnedDuration);
        CurrentState = State.Patrol;
        agent.isStopped = false;
    }
}
