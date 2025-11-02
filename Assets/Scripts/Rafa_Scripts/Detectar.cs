using UnityEngine;
using UnityEngine.AI;
//using static AnimalAI2;

public class Detectar : MonoBehaviour
{
    public AnimalAI2 Animal;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region Detection Logic (🔍 NUEVO)
    /*private void OnTriggerEnter(Collider other)
    {
        if (Animal.IsDead || Animal.IsInSafeZone) return;
        if (Animal.CurrentState == Animal.State.Huir || Animal.CurrentState == Animal.State.SeguirSilbido) return;

        // Detectar cazador
        if (other.CompareTag("Hunter"))
        {
            Animal.lastHunterPosition = other.transform.position;
            Animal.StartHuir(Animal.lastHunterPosition);
            return;
        }

        // Detectar alimento
        if (other.CompareTag("Food"))
        {
            FoodItem food = other.GetComponent<FoodItem>();
            if (food != null && !food.isEmpty && Animal.currentSub == Animal.PastorearSub.Deambular)
            {
                Animal.currentFood = food;
                Vector3 targetPos = food.transform.position;
                NavMeshHit hit;
                if (NavMesh.SamplePosition(targetPos, out hit, 10f, NavMesh.AllAreas))
                {
                    Animal.agent.SetDestination(hit.position);
                    Animal.currentSub = Animal.PastorearSub.AcercarseAlAlimento;
                }
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        // Evita detección repetida si ya está alimentándose o huyendo
        if (Animal.CurrentState != Animal.State.Pastorear) return;
        if (Animal.currentSub != Animal.PastorearSub.Deambular) return;

        // Si aparece un cazador cerca mientras pastorea → huye
        if (other.CompareTag("Hunter"))
        {
            Animal.lastHunterPosition = other.transform.position;
            Animal.StartHuir(Animal.lastHunterPosition);
        }
    }*/
    #endregion

}
