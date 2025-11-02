using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Collider))]
public class WoolHand : MonoBehaviour
{
    public float healRange = 0.5f; // opcional, si quieres verificar distancia adicional
    private HashSet<Collider> overlapping = new HashSet<Collider>();

    void Awake()
    {
        Collider c = GetComponent<Collider>();
        c.isTrigger = true;
    }

    void OnTriggerEnter(Collider other)
    {
        overlapping.Add(other);
    }

    void OnTriggerExit(Collider other)
    {
        overlapping.Remove(other);
    }

    // Retorna true si se aplicó la acción (para que player consuma energía)
    public bool TryUse()
    {
        AnimalAI2 target = GetClosestAnimal();
        if (target != null)
        {
            // Heal: restaura un golpe y devuelve velocidad normal
            if (target.HealByPlayer())
            {
                // efecto visual/sonoro opcional
                return true;
            }
        }
        return false;
    }

    AnimalAI2 GetClosestAnimal()
    {
        AnimalAI2 best = null;
        float minD = float.MaxValue;
        foreach (var col in overlapping)
        {
            if (col == null) continue;
            AnimalAI2 a = col.GetComponent<AnimalAI2>();
            if (a == null) continue;

            // Si el animal ya tiene vida normal o está muerto o en safe zone, no tiene efecto
            if (a.IsDead || a.IsInSafeZone) continue;
            if (a.CurrentHealth >= a.maxHealth) continue;

            float d = Vector3.Distance(transform.position, a.transform.position);
            if (d < minD)
            {
                minD = d; best = a;
            }
        }
        return best;
    }
}
