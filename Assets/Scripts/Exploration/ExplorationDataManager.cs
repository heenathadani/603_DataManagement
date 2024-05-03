using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExplorationDataManager : MonoBehaviour
{
    public GameObject player;
    Camera cam;

    public List<GameObject> npcList;


    private void OnEnable()
    {
        //cam = Camera.main;
        Vector3 loadedPosition = ExplorationData.LoadPlayerLocation();

        // If a position is saved in the Exploration Data, we should load it back in
        if (loadedPosition != Vector3.zero)
        {
            //Debug.Log("it no worky");
            //Vector3 distance = player.transform.position - cam.transform.position;
            player.transform.position = loadedPosition;
            //cam.transform.position = player.transform.position - distance;

            for(int i = 0; i <ExplorationData.aliveEnemy.Count; i++)
            {
                if(!ExplorationData.aliveEnemy[i])
                {
                    Destroy(npcList[i]);
                }
            }

        }

        // Other enable stuff to update the scene back to where it was prior to combat



        // Ensure the exploration data knows where we are from the get-go
        ExplorationData.SaveExplorationSceneIndex(SceneManager.GetActiveScene().buildIndex);
    }
}
