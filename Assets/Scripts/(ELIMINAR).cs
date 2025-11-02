using UnityEngine;

public class eliminar : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    // Update is called once per frame


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("El jugador uso habilidad");
            FindAnyObjectByType<UIManager>().ChangeEnergy(-50); // Recupera energía
        }
    }
}
