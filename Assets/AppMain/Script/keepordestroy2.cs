using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class keepordestroy2 : MonoBehaviour
{
    public static bool istanker = false;

    private void Start()
    {
        if (istanker)
        {
            Destroy(this.gameObject);
        }
    }
}
