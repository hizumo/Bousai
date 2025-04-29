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
        // �A�C�e���𐶐�
        item = ItemGenerater.instance.Spawn(itemType);

        // �A�C�e�������łɏ�������Ă���ꍇ�A��\���ɂ���
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

        // �A�C�e���� ItemManager �ɒǉ�
        ItemManager.Instance.AddItem(item);

        // �A�C�e���� ItemBox �ɓo�^
        ItemBox.instance.SetItem(item);

        gameObject.SetActive(false);
    }
}
