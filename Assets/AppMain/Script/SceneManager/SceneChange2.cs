using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange2 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (ItemManager.Instance.HasItem(Item.Type.Rope))
        {
            Debug.Log("ロープを持っています！");
        }
        else
        {
            Debug.Log("ロープはありません！");
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.P))
        {
            SceneManager.LoadScene("CrossRoadsScene");
        }
    }
}
