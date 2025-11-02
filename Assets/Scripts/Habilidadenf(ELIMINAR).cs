using UnityEngine;

public class Habilidadenf : MonoBehaviour
{
    public UIManager uiManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (uiManager == null) 
        {
            uiManager = FindAnyObjectByType<UIManager>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // Aquí puedes agregar el código para aplicar daño al jugador
            Debug.Log("El jugador uso habilidad.");
            uiManager.UseSkill(0); // Usar habilidad 1
        }
    }
}
