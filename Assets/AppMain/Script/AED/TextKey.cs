using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static PlayerController;

public class TextKey : MonoBehaviour
{
    [SerializeField] Text KeyText;
    [SerializeField] GameObject AED;
    [SerializeField] GameObject Clothes;
    [SerializeField] GameObject Clothesimage;
    [SerializeField] GameObject Pad1;
    [SerializeField] GameObject Pad2;
    [SerializeField] GameObject Person1;
    [SerializeField] GameObject Person2;
    [SerializeField] GameObject Person1image;
    [SerializeField] GameObject Person2image;

    private void Start()
    {
        KeyText.text = "AEDを起動してください";
    }
    void Update()
    {
        if (Input.GetKey(KeyCode.Alpha1))
        {
            KeyText.text = "１が押された";
        }
    }
    public void AEDClick()
    {
        KeyText.text = "服を脱がせてください";
        Clothes.SetActive(true);
        AED.SetActive(false);
    }
    public void ClothesClick()
    {
        KeyText.text = "AEDを操作してください";
        Clothes.SetActive(false);
        Clothesimage.SetActive(false);
        Pad1.SetActive(true);
        Pad2.SetActive(true);
    }

    public void Pad1Clck()
    {
        Person1.SetActive(true);
        Pad1.SetActive(false);
    }

    public void Pad2Clck()
    {
        Person2.SetActive(true);
        Pad2.SetActive(false);
    }

    public void Person1Clck()
    {
        Person1.SetActive(false);
        Person1image.SetActive(true);

    }
    public void Person2Clck()
    {
        Person2.SetActive(false);
        Person2image.SetActive(true);

    }
}
