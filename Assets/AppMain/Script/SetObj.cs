using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SetObj : MonoBehaviour
{
    [SerializeField] GameObject setObjectAppear;
    [SerializeField] GameObject setObjectDisappear;
    [SerializeField] Item.Type userItem;
    [SerializeField] GameObject Tanker;
    [SerializeField] GameObject panel3;
    [SerializeField] GameObject panel4;

    public static int count = 0;

    private void Start()
    {
        count = 0;
    }

    // 適切なアイテムを選択した状態で
    // このオブジェクトをクリックしたら
    private void Update()
    {
        Debug.Log(count);
    }
    public void OnClickAppear()
    {
        // 適切なアイテムを選択した状態で
        bool hasItem = ItemBox.instance.TryUseItem(userItem);
        if (hasItem)
        {
            // アイテムを表示する
            setObjectAppear.SetActive(true);
        }
    }
    public void OnClickDisappear()
    {
        // 適切なアイテムを選択した状態で
        bool hasItem = ItemBox.instance.TryUseItem(userItem);
        if (hasItem)
        {
            if (count == 0)
            {
                // アイテムを表示する
                StartCoroutine(Events2());
                IEnumerator Events2()
                {
                    count += 1;
                    Tanker.SetActive(true);
                    panel3.SetActive(true);
                    yield return new WaitForSeconds(1.5f);
                    Destroy(panel3);
                    PlayerController.score += 10;
                    setObjectDisappear.SetActive(false);
                }
            }
            else if (count == 1)
            {
                StartCoroutine(Events3());
                IEnumerator Events3()
                {
                    count += 1;
                    panel4.SetActive(true);
                    yield return new WaitForSeconds(1.5f);
                    Destroy(panel4);
                    PlayerController.score += 10;
                    setObjectDisappear.SetActive(false);

                }
            }
        }

    }
}
