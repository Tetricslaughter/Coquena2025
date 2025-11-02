using UnityEngine;

public class SafeZone : MonoBehaviour
{
    void Awake()
    {
        Collider c = GetComponent<Collider>();
        c.isTrigger = true;
    }

    /*void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Entro algo"+ other.name);
        AnimalAI2 a = other.GetComponent<AnimalAI2>();
        if (a != null)
        {
            //Debug.Log("kjakskasksk");
            a.EnterSafeZone();
        }
    }*/
    void OnTriggerStay(Collider other)
    {
        //Debug.Log("Entro algo"+ other.name);
        AnimalAI2 a = other.GetComponent<AnimalAI2>();
        if (a != null)
        {
            //Debug.Log("kjakskasksk");
            a.EnterSafeZone();
        }
    }

}
