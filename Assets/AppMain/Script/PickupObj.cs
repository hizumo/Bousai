using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupObj : MonoBehaviour
{
    [SerializeField] Item.Type itemType;
    Item item;
    [SerializeField] GameObject explain;

    private void Start()
    {
        item = ItemGenerater.instance.Spawn(itemType);
    }

    public void OnClickObj()
    {
        Debug.Log(item);
        if (item.type == Item.Type.Rope)
        {
            explain.SetActive(true);

        }

        ItemBox.instance.SetItem(item);
        gameObject.SetActive(false);
    }
}
