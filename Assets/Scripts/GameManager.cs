using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    int animalCount;
    float elapsedTime;
    int timeLimit = 300; // 5 minutos
    void Start()
    {
        elapsedTime = 0f;
        animalCount = GameObject.FindGameObjectsWithTag("Animal").Length;
        StartCoroutine(StartTime());
    }

    
    void Update()
    {
      
        if(elapsedTime >= timeLimit)
        {
            
            Debug.Log("Victory! You have protected the animals.");
            //ir a pantalla de victoria
            SceneManager.LoadScene("Victoria");
        }

    }
    IEnumerator StartTime() 
    {
       
        while (true)
        {
            Debug.Log("Elapsed Time: " + elapsedTime);
            yield return new WaitForSeconds(1);
            elapsedTime += 1;
        }
    }
        public IEnumerator TriggerDefeat()
    {
        StopCoroutine(StartTime());
        yield return new WaitForSeconds(2);
        //ir a pantalla de derrota
        SceneManager.LoadScene("Derrota");
    }
}
