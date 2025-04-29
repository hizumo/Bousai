// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// [CreateAssetMenu(fileName = "ItemDataBase", menuName = "CreateItemDataBase")]
// public class ItemDataBase : ScriptableObject{
//     [SerializeField] private List<Item> itemLists = new List<Item>();
//     // アイテムリストを返す
//     public List<Item> GetItemLists(){
//         return itemLists;
//     }
// }

// public class ItemManager : MonoBehaviour
// {
//     static ItemManager instance;

//     public static ItemManager GetInstance()
//     {
//         return instance;
//     }

//     void Start()
//     {
//         instance = this;
//     }

//     public bool HasItem(string searchName)
//     {
//         return itemDataBase.GetItemLists().Exists(item => item.GetItemName() == searchName);
//     }
// }
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public static ItemManager Instance { get; private set; } // シングルトンのインスタンス

    private List<Item> ownedItems = new List<Item>(); // 所持しているアイテムのリスト

    private void Awake()
    {
        // シングルトンのインスタンスが既に存在している場合は削除
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // シーンを切り替えても破棄されない
    }

    // アイテムを追加する
    public void AddItem(Item item)
    {
        ownedItems.Add(item);
        Debug.Log($"{item.type} を追加しました！");
    }

    // 特定のアイテムを所持しているかチェック
    public bool HasItem(Item.Type itemType)
    {
        return ownedItems.Exists(item => item.type == itemType);
    }

    public List<Item> GetOwnedItems()
    {
        return new List<Item>(ownedItems); // 所持アイテムのコピーを返す
    }
    public void RemoveItem(Item.Type itemType)
    {
        // 所持アイテムリストから該当するアイテムを削除
        Item itemToRemove = ownedItems.Find(item => item.type == itemType);
        if (itemToRemove != null)
        {
            ownedItems.Remove(itemToRemove);
            Debug.Log($"{itemType} を削除しました！");
        }
        else
        {
            Debug.LogWarning($"{itemType} は所持アイテムに存在しません。");
        }
    }

}

