using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class AnimalAI2 : MonoBehaviour
{
    public enum State { Pastorear, Huir, SeguirSilbido, Muerto, Quieto }
    public enum PastorearSub { Deambular, AcercarseAlAlimento, Alimentarse }

    [Header("Components")]
    public NavMeshAgent agent;
    public Animator animator;
    public SphereCollider detectionCollider; // para detectar cazador y comida (configurar isTrigger)

    [Header("Speeds")]
    public float walkSpeed = 3f;
    public float runSpeed = 6f;

    [Header("Health")]
    public int maxHealth = 2; // recibir 2 disparos = muerte
    public int CurrentHealth { get; private set; }
    public bool IsDead => CurrentHealth <= 0;

    [Header("Huir")]
    public float runDuration = 3f;

    [HideInInspector]
    public bool IsInSafeZone = false;

    public State CurrentState { get; private set; } = State.Pastorear;
    public PastorearSub currentSub = PastorearSub.Deambular;

    // Para controlar quién está siguiendo la quena (solo uno globalmente)
    private static AnimalAI2 quenaFollower = null;
    public static void SetQuenaFollower(AnimalAI2 a)
    {
        if (quenaFollower != null && quenaFollower != a)
            quenaFollower.ExitFollowWhistle();
        quenaFollower = a;
    }

    private Transform followTargetForQuena = null;
    private float originalWalkSpeed;
    private float originalRunSpeed;

    void Awake()
    {
        CurrentHealth = maxHealth;
        if (agent == null) agent = GetComponent<NavMeshAgent>();
        if (animator == null) animator = GetComponentInChildren<Animator>();
        originalWalkSpeed = walkSpeed;
        originalRunSpeed = runSpeed;
        ApplySpeeds();
    }

    void Update()
    {
        if (IsInSafeZone || IsDead) return;

        switch (CurrentState)
        {
            case State.Pastorear:
                UpdatePastorear();
                break;
            case State.Huir:
                // se maneja por corrutina StartHuir
                break;
            case State.SeguirSilbido:
                UpdateSeguirSilbido();
                break;
        }
    }

    #region Pastorear
    void UpdatePastorear()
    {
        // Implementación de deambular simple: si no tiene objetivo, camina a un punto aleatorio
        if (currentSub == PastorearSub.Deambular)
        {
            //animator.SetBool("isWalking", true);
            if (!agent.hasPath || agent.remainingDistance < 0.5f)
            {
                Vector3 randomPoint = transform.position + Random.insideUnitSphere * 10f;
                NavMeshHit hit;
                if (NavMesh.SamplePosition(randomPoint, out hit, 10f, NavMesh.AllAreas))
                {
                    agent.SetDestination(hit.position);
                }
            }
        }
        else if (currentSub == PastorearSub.AcercarseAlAlimento)
        {
            //animator.SetBool("isWalking", true);
            // Asumimos que hubo set de destino hacia el alimento antes
            if (agent.remainingDistance <= 1.0f)
            {
                currentSub = PastorearSub.Alimentarse;
                StartCoroutine(DoFeed());
            }
        }
    }

    IEnumerator DoFeed()
    {
        //animator.SetTrigger("Feed");
        agent.isStopped = true;
        yield return new WaitForSeconds(2f); // duración anim alimentarse
        agent.isStopped = false;
        // Aquí se debería avisar al objeto alimento que quedó vacío (no implementado)
        currentSub = PastorearSub.Deambular;
    }
    #endregion

    #region Huir
    public void StartHuir(Vector3 fromPosition)
    {
        if (IsDead || IsInSafeZone) return;
        StopAllCoroutines(); // salir de otros subestados
        CurrentState = State.Huir;
        StartCoroutine(DoHuir(fromPosition));
    }

    IEnumerator DoHuir(Vector3 fromPosition)
    {
        //animator.SetBool("isRunning", true);
        // dirección opuesta
        Vector3 dir = (transform.position - fromPosition).normalized;
        Vector3 runTarget = transform.position + dir * 8f;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(runTarget, out hit, 8f, NavMesh.AllAreas))
        {
            agent.speed = runSpeed;
            agent.SetDestination(hit.position);
        }
        yield return new WaitForSeconds(runDuration);

        agent.speed = walkSpeed;
        //animator.SetBool("isRunning", false);
        CurrentState = State.Pastorear;
        currentSub = PastorearSub.Deambular;
    }
    #endregion

    #region Seguir Silbido
    public void EnterFollowWhistle(Transform playerTransform)
    {
        if (IsDead || IsInSafeZone) return;
        if (CurrentState == State.Huir) return;

        // establece este animal como follower global
        SetQuenaFollower(this);
        followTargetForQuena = playerTransform;
        CurrentState = State.SeguirSilbido;
        agent.isStopped = false;
        agent.speed = walkSpeed;
        //animator.SetBool("isWalking", true);
    }

    public void ExitFollowWhistle()
    {
        if (quenaFollower == this) quenaFollower = null;
        followTargetForQuena = null;
        if (!IsDead && !IsInSafeZone)
        {
            CurrentState = State.Pastorear;
            currentSub = PastorearSub.Deambular;
        }
    }

    void UpdateSeguirSilbido()
    {
        if (followTargetForQuena == null)
        {
            ExitFollowWhistle();
            return;
        }

        agent.SetDestination(followTargetForQuena.position);
        // si llega a zona segura, Freeze (handled por safezone)
    }
    #endregion

    #region Damage / Heal
    public void ReceiveShot()
    {
        if (IsDead || IsInSafeZone) return;

        // si recibe primer disparo: reducir velocidades
        CurrentHealth -= 1;
        ApplySpeedMultiplier(2f / 3f); // reducir a 2/3
        // sale de cualquier subestado y pasa a Huir
        StartHuir(GetLastKnownThreatPosition());

        if (CurrentHealth <= 0)
        {
            Die();
        }
    }

    Vector3 GetLastKnownThreatPosition()
    {
        // simplificación: usamos transform hacia atrás (no hay info del cazador aquí)
        return transform.position - transform.forward * 2f;
    }

    public bool HealByPlayer()
    {
        if (IsDead || IsInSafeZone) return false;
        if (CurrentHealth >= maxHealth) return false;

        CurrentHealth = Mathf.Min(CurrentHealth + 1, maxHealth);
        // restaurar velocidades
        RestoreOriginalSpeeds();
        return true;
    }

    void ApplySpeedMultiplier(float multiplier)
    {
        walkSpeed = originalWalkSpeed * multiplier;
        runSpeed = originalRunSpeed * multiplier;
        ApplySpeeds();
    }

    void RestoreOriginalSpeeds()
    {
        walkSpeed = originalWalkSpeed;
        runSpeed = originalRunSpeed;
        ApplySpeeds();
    }

    void ApplySpeeds()
    {
        if (agent != null)
        {
            // por simplicidad asumimos que agent.speed corresponde a walkSpeed cuando no corre
            agent.speed = (CurrentState == State.Huir) ? runSpeed : walkSpeed;
        }
    }

    void Die()
    {
        CurrentHealth = 0;
        IsInSafeZone = false;
        CurrentState = State.Muerto;
        agent.isStopped = true;
        //animator.SetTrigger("Die");
        // Opcional: desactivar collider, reputación, etc.
    }
    #endregion

    #region Safe Zone
    public void EnterSafeZone()
    {
        if (CurrentState != State.SeguirSilbido)
        {
            IsInSafeZone = true;
            //CurrentState = State.Pastorear;
            CurrentState = State.Quieto;
            agent.isStopped = true;
            //animator.SetBool("isIdle", true);
            //Debug.Log("En La Zona Segura");
            // El animal se queda en idle y no cambia de estado
        }

    }
    #endregion
}
