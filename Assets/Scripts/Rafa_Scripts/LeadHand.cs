using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Collider))]
public class LeadHand : MonoBehaviour
{
    [Header("Detector de colisiones externo (opcional)")]
    public Collider detectionCollider; // Collider externo asignable desde el editor

    private HashSet<Collider> overlapping = new HashSet<Collider>();

    void Awake()
    {
        Collider c = GetComponent<Collider>();
        c.isTrigger = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Animal") || other.CompareTag("Hunter"))
        overlapping.Add(other);
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Animal") || other.CompareTag("Hunter"))
            overlapping.Remove(other);
    }

    // Retorna true si se aplicó la acción (para que player consuma energía)
    public bool TryUse()
    {
        // Prioriza cazador si existe, sino animal.
        Collider hunterCol = GetClosestOfType<HunterAI2>();
        if (hunterCol != null)
        {
            HunterAI2 h = hunterCol.GetComponent<HunterAI2>();
            if (h != null)
            {
                h.EnterStunnedState();
                return true;
            }
        }

        Collider animalCol = GetClosestOfType<AnimalAI2>();
        if (animalCol != null)
        {
            AnimalAI2 a = animalCol.GetComponent<AnimalAI2>();
            if (a != null)
            {
                a.ReceiveShot(); // implementa la lógica de reducir velocidad / muerte
                return true;
            }
        }

        return false;
    }

    Collider GetClosestOfType<T>() where T : MonoBehaviour
    {
        Collider best = null;
        float minD = float.MaxValue;
        foreach (var col in overlapping)
        {
            if (col == null) continue;
            if (col.GetComponent<T>() == null) continue;
            float d = Vector3.Distance(transform.position, col.transform.position);
            if (d < minD)
            {
                minD = d; best = col;
            }
        }
        return best;
    }
}
