using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class Fire_extinguisher : MonoBehaviour
{
    int click = 0;

    // Start is called before the first frame update
    void Start()
    {
        click = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (click == 0)
            {
                transform.GetChild(0).gameObject.SetActive(false);
                transform.GetChild(1).gameObject.SetActive(true);
                click = 1;
            }
            else if (click == 1)
            {
                SceneManager.LoadScene("FireExtingusherScene");
            }
        }
    }
    
}
