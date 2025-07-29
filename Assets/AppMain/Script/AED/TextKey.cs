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
        KeyText.text = "AED‚ğ‹N“®‚µ‚Ä‚­‚¾‚³‚¢";
    }
    void Update()
    {
        if (Input.GetKey(KeyCode.Alpha1))
        {
            KeyText.text = "‚P‚ª‰Ÿ‚³‚ê‚½";
        }
    }
    public void AEDClick()
    {
        KeyText.text = "•‚ğ’E‚ª‚¹‚Ä‚­‚¾‚³‚¢";
        Clothes.SetActive(true);
        AED.SetActive(false);
    }
    public void ClothesClick()
    {
        KeyText.text = "AED‚ğ‘€ì‚µ‚Ä‚­‚¾‚³‚¢";
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
