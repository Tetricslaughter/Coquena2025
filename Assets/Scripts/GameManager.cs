using System.Collections;
using UnityEngine;

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
      //  Debug.Log("Elapsed Time: " + elapsedTime);
        if(elapsedTime >= timeLimit)
        {
            Debug.Log("Victory! You have protected the animals.");
            //ir a pantalla de victoria
        }

    }
    IEnumerator StartTime() 
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            elapsedTime += 1;
        }
    }
    public void TriggerDefeat()
    {
        Debug.Log("Defeat! An animal was caught by a hunter.");
        //ir a pantalla de derrota
    }
}
