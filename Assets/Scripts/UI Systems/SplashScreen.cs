using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashScreen : MonoBehaviour
{
    public GameObject gameObjectToHide;  // Assign the GameObject to hide in the Inspector

    void Start()
    {
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            gameObjectToHide.SetActive(false);
        }
    }
}
