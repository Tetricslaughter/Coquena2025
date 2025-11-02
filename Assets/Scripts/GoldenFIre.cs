using UnityEngine;

public class GoldenFIre : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // Aquí puedes agregar el código para aplicar daño al jugador
            Debug.Log("El jugador ha sido alcanzado por el fuego dorado.");
            FindAnyObjectByType<UIManager>().RecoverEnergy(10f); // Recupera energía
        }
    }
}
