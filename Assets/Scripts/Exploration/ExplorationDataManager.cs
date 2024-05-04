using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExplorationDataManager : MonoBehaviour
{
    public GameObject player;
    public List<GameObject> npcList;


    private void OnEnable()
    {
        if (ExplorationData.aliveEnemy.Count == 0){
            ExplorationData.SetUpNPCs(npcList.Count);
        }
        //cam = Camera.main;
        Vector3 loadedPosition = ExplorationData.LoadPlayerLocation();

        // If a position is saved in the Exploration Data, we should load it back in
        if (loadedPosition != Vector3.zero)
        {
            //Debug.Log("it no worky");
            player.transform.position = loadedPosition;

            for(int i = 0; i < ExplorationData.aliveEnemy.Count; i++)
            {
                if(!ExplorationData.aliveEnemy[i])
                {
                    //GameObject npc = npcList[i];
                    //npcList.RemoveAt(i);
                    Destroy(npcList[i]);
                }
            }

        }

        // Other enable stuff to update the scene back to where it was prior to combat



        // Ensure the exploration data knows where we are from the get-go
        ExplorationData.SaveExplorationSceneIndex(SceneManager.GetActiveScene().buildIndex);
    }
}
