using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetTent : MonoBehaviour
{
    [SerializeField] GameObject setObjectAppear;
    [SerializeField] GameObject setObjectDisappear;
    [SerializeField] Item.Type userItem;
    [SerializeField] GameObject panel;
    // 適切なアイテムを選択した状態で
    // このオブジェクトをクリックしたら
    public void OnClick()
    {
        // 適切なアイテムを選択した状態で
        bool hasItem = ItemBox.instance.TryUseItem(userItem);
        if(hasItem)
        {
            // アイテムを表示する
            StartCoroutine(Events());
            IEnumerator Events()
            {
                panel.SetActive(true);
                yield return new WaitForSeconds(1.5f);
                Destroy(panel);
                PlayerController.score += 10;
                setObjectDisappear.SetActive(false);
                setObjectAppear.SetActive(true);
                setObjectDisappear.SetActive(false);

            }


        }
    }



}


