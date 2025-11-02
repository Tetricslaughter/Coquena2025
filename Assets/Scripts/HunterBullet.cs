using UnityEngine;

public class HunterBullet : MonoBehaviour
{
    public float speed = 20f;
    public float lifeTime = 5f;
    

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Si impacta contra un animal
        if (other.CompareTag("Animal"))
        {
            //Daño al animal

            Destroy(gameObject);
        }
    }
        
}   
