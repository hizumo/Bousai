using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class keepordestroy : MonoBehaviour
{
    public static bool isClear = false;

    private void Start()
    {
        if (isClear)
        {
            Destroy(this.gameObject);
        }
    }
}
