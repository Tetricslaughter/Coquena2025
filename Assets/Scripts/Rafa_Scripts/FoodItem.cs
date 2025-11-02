using UnityEngine;
using System.Collections;

public class FoodItem : MonoBehaviour
{
    [Header("Referencias de objetos visuales")]
    public GameObject tallo;
    public GameObject hoja;

    [Header("Configuración del alimento")]
    public float refillTime = 15f; // segundos para regenerarse
    public bool isEmpty = false;

    void Start()
    {
        // Por seguridad: si no se asignan manualmente, se buscan
        if (tallo == null) tallo = transform.Find("Tallo")?.gameObject;
        if (hoja == null) hoja = transform.Find("Hoja")?.gameObject;
    }

    /// <summary>
    /// Llamado por el animal cuando se alimenta.
    /// </summary>
    public void Consume()
    {
        if (isEmpty) return;

        isEmpty = true;

        // Ocultamos la hoja
        if (hoja != null)
            hoja.SetActive(false);

        // Iniciamos recarga
        StartCoroutine(RefillAfterDelay());
    }

    IEnumerator RefillAfterDelay()
    {
        yield return new WaitForSeconds(refillTime);

        isEmpty = false;

        // Mostramos nuevamente la hoja
        if (hoja != null)
            hoja.SetActive(true);
    }
}
