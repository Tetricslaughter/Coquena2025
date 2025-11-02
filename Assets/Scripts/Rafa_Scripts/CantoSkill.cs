using UnityEngine;
using System.Collections;

public class CantoSkill : MonoBehaviour
{
    public float range = 12f;
    public float cooldown = 10f;
    public LayerMask hunterLayer;
    private bool onCooldown = false;

    public void TryUse()
    {
        if (onCooldown) return;
        StartCoroutine(DoCanto());
    }

    IEnumerator DoCanto()
    {
        onCooldown = true;

        Collider[] hits = Physics.OverlapSphere(transform.position, range, hunterLayer);
        foreach (var col in hits)
        {
            HunterAI2 hunter = col.GetComponent<HunterAI2>();
            if (hunter == null) continue;

            if (hunter.CurrentState == HunterAI2.State.Stunned ||
                hunter.CurrentState == HunterAI2.State.Huir ||
                hunter.CurrentState == HunterAI2.State.Attacking)
            {
                continue;
            }

            hunter.EnterDistractedState(); // método en HunterAI
        }

        yield return new WaitForSeconds(cooldown);
        onCooldown = false;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
