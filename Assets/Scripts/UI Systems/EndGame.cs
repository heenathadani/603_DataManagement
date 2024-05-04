using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGame : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject endScreen;
    void Start()
    {
        endScreen.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void activateEndScreen()
    {
        endScreen.SetActive(true);
    }
}
