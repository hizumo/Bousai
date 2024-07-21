using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class eraseText : MonoBehaviour
{
    int click = 0;
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
                transform.GetChild(1).gameObject.SetActive(false);
                click = 1;
            }
            else if (click == 1)
            {
                transform.GetChild(1).gameObject.SetActive(true);
                click = 0;

            }
        }
    }
}
