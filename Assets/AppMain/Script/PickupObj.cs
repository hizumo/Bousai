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
        // アイテムを生成
        item = ItemGenerater.instance.Spawn(itemType);

        // アイテムがすでに所持されている場合、非表示にする
        if (ItemManager.Instance != null && ItemManager.Instance.HasItem(itemType))
        {
            gameObject.SetActive(false);
        }
    }

    public void OnClickObj()
    {
        Debug.Log(item);
        if (item.type == Item.Type.Rope ||
            item.type == Item.Type.Bar ||
            item.type == Item.Type.Tanker)
        {
            explain.SetActive(true);
        }

        // アイテムを ItemManager に追加
        ItemManager.Instance.AddItem(item);

        // アイテムを ItemBox に登録
        ItemBox.instance.SetItem(item);

        gameObject.SetActive(false);
    }
}
