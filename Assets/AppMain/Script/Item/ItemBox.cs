using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBox : MonoBehaviour
{
    [SerializeField] Slot[] slots = default;
    [SerializeField] Slot selectedSlot = null;

    public static ItemBox instance;

    private void Start()
    {
        if (ItemManager.Instance == null) return;

        // 所持アイテムを取得し、スロットに設定
        foreach (Item item in ItemManager.Instance.GetOwnedItems())
        {
            SetItem(item);
        }
    }

    private void Awake()
    {
        instance = this;
        slots = GetComponentsInChildren<Slot>();

        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].position = i;
        }
    }
    
    public void SetItem(Item item)
    {
        foreach(Slot slot in slots)
        {
            if(slot.IsEmpty())
            {
                slot.SetItem(item);
                break;
            }
        }
    }

    public void OnSelectSlot(int position)
    {
        Debug.Log($"OnSelectSlot called with position = {position}");
        // いったんすべてのスロットの選択パネルを表示する
        foreach (Slot slot in slots)
        {
            slot.HideBgPanel();
        }
        selectedSlot = null;

        if(slots[position].OnSelected())
        {
            selectedSlot = slots[position];
            Debug.Log($"selectedSlot set: {selectedSlot.GetItem()?.type}");
        }
    }

    public bool TryUseItem(Item.Type type)
    {
        // 選択アイテムスロットがあるかどうか
        if (selectedSlot == null)
        {
            return false;
        }
        if (selectedSlot.GetItem().type == type)
        {
            // スロット内のアイテムを削除
            ItemManager.Instance.RemoveItem(type);
            selectedSlot.SetItem(null);
            selectedSlot.HideBgPanel();
            selectedSlot = null;
            return true;
        }
        return false;
    }


    public Item GetSelectedItem()
    {
        if(selectedSlot == null)
        {
            return null;
        }
        return selectedSlot.GetItem();
    }
}
