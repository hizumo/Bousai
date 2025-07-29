using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    Item item = default;
    [SerializeField]Image image = default;
    [SerializeField]GameObject backgroundPanel = default;

    public int position; // slots�z����̈ʒu�i�蓮�ݒ�܂��͏��������Ɋ��蓖�āj

    private void Awake()
    {
        // image = GetComponent<Image>();
    }

    private void Start()
    {
        backgroundPanel.SetActive(false);
    }

    public bool IsEmpty()
    {
        if(item == null)
        {
            return true;
        }
        return false;
    }

    public void SetItem(Item item)
    {
        this.item = item;
        UpdateImage(item);
    }

    public Item GetItem()
    {
        return item;
    }

    void UpdateImage(Item item)
    {
        if(item == null)
        {
            image.sprite = null;
        }
        else
        {
            image.sprite = item.sprite;
        }
    }

    public bool OnSelected()
    {
        if(item==null)
        {
            return false;
        }
        backgroundPanel.SetActive(true);
        return true;
    }

    public void HideBgPanel()
    {
        backgroundPanel.SetActive(false);
    }


    // �N���b�N���ɌĂ΂��֐�
    public void OnClick()
    {
        Debug.Log($"Slot {position} clicked, item = {item?.type}");
        ItemBox.instance.OnSelectSlot(position);
    }
}
