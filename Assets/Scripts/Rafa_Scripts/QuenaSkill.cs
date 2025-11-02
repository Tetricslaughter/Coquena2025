using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class QuenaSkill : MonoBehaviour
{
    public float range = 10f;
    public float cooldown = 10f;
    public LayerMask animalLayer;
    private bool onCooldown = false;
    private AnimalAI2 currentFollower = null; // el animal que actualmente sigue al player

    public void TryUse()
    {
        if (onCooldown) return;
        StartCoroutine(DoQuena());
    }

    IEnumerator DoQuena()
    {
        onCooldown = true;

        // Si ya hay un animal siguiéndonos, al volver a presionar Q lo soltamos
        if (currentFollower != null)
        {
            currentFollower.ExitFollowWhistle();
            currentFollower = null;
        }
        else
        {
            // No hay animal siguiendo → buscar uno nuevo
            AnimalAI2 closest = FindClosestValidAnimal();
            if (closest != null)
            {
                AnimalAI2.SetQuenaFollower(closest);
                closest.EnterFollowWhistle(transform);
                currentFollower = closest;
            }
        }

        yield return new WaitForSeconds(cooldown);
        onCooldown = false;
    }

    AnimalAI2 FindClosestValidAnimal()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, range, animalLayer);
        AnimalAI2 closest = null;
        float minDist = float.MaxValue;

        foreach (var col in hits)
        {
            AnimalAI2 a = col.GetComponent<AnimalAI2>();
            if (a == null) continue;

            // No puede ser afectado si está en estos estados
            if (a.CurrentState == AnimalAI2.State.Muerto || a.IsInSafeZone || a.CurrentState == AnimalAI2.State.Huir)
                continue;

            float d = Vector3.Distance(transform.position, a.transform.position);
            if (d < minDist)
            {
                minDist = d;
                closest = a;
            }
        }

        return closest;
    }

    // para visualización en editor
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
