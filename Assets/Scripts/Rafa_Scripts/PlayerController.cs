using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Resources")]
    public float maxEnergy = 100f;
    public float energy = 100f;
    public float energyCostWool = 20f;
    public float energyCostLead = 15f;
    public float energyRegenPerSecond = 5f;

    [Header("References")]
    public QuenaSkill quenaSkill;
    public CantoSkill cantoSkill;
    public WoolHand woolHand;   // attached to arm collider
    public LeadHand leadHand;   // attached to other arm collider
    public Animator animator;

    void Start()
    {
        energy = Mathf.Clamp(energy, 0, maxEnergy);
        if (animator == null) animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        HandleInput();
        //RegenerateEnergy();
    }

    void HandleInput()
    {
        // Q = Quena
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (quenaSkill != null) quenaSkill.TryUse();
            if (animator != null) animator.SetTrigger("Quena"); // configure animator trigger
        }

        // E = Canto
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (cantoSkill != null) cantoSkill.TryUse();
            if (animator != null) animator.SetTrigger("Canto");
        }

        // R = Mano de lana (wool)
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (energy >= energyCostWool && woolHand != null)
            {
                if (woolHand.TryUse())
                {
                    Debug.Log("entro");
                    energy -= energyCostWool;
                    if (animator != null) animator.SetTrigger("WoolHit");
                }
            }
            else
            {
                // opcional: feedback de falta de energía
            }
        }

        // Left Mouse = Mano de plomo (lead)
        if (Input.GetMouseButtonDown(0))
        {
            if (energy >= energyCostLead && leadHand != null)
            {
                if (leadHand.TryUse())
                {
                    energy -= energyCostLead;
                    if (animator != null) animator.SetTrigger("LeadHit");
                }
            }
            else
            {
                // falta de energía
            }
        }
    }

    void RegenerateEnergy()
    {
        if (energy < maxEnergy)
        {
            energy += energyRegenPerSecond * Time.deltaTime;
            energy = Mathf.Min(energy, maxEnergy);
        }
    }
}
