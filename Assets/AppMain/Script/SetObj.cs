using Fungus;
using System.Collections;
using UnityEngine;

public class SetObj : MonoBehaviour
{
    [SerializeField] private GameObject setObjectAppear;
    [SerializeField] private GameObject setObjectDisappear;
    [SerializeField] private Item.Type userItem;
    [SerializeField] private GameObject Tanker;
    [SerializeField] private GameObject panel3;
    [SerializeField] private GameObject panel4;

    public static int count = 0; // インスタンスごとに分離
    private void Start()
    {
        if (this.gameObject.CompareTag("Tanker"))
        {
            if (keepordestroy.isClear == true)
            {
                this.gameObject.SetActive(true);
            }
            else
            {
                this.gameObject.SetActive(false);
            }
        }
    }

    public void OnClickAppear()
    {
        if (ItemBox.instance.TryUseItem(userItem))
        {
            setObjectAppear.SetActive(true);
        }
    }

    public void OnClickDisappear()
    {
        bool hasItem = ItemBox.instance.TryUseItem(userItem);
        if (hasItem)
        {
            if (count == 0)
            {
                StartCoroutine(HandleEvent(panel3, 10, true));
                keepordestroy.isClear = true;
            }
            else if (count == 1)
            {
                StartCoroutine(HandleEvent(panel4, 10, false));
                keepordestroy2.istanker = true;
            }
        }
    }



    private IEnumerator HandleEvent(GameObject panel, int scoreIncrement, bool activateTanker)
    {
        count += 1;

        if (activateTanker)
        {
            Tanker.SetActive(true);
        }

        panel.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        Destroy(panel);

        PlayerController.score += scoreIncrement;
        setObjectDisappear.SetActive(false);
    }
}
