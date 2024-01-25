using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Panel1 : MonoBehaviour
{
    [SerializeField] private Sprite[] motorImages;
    [SerializeField] private string[] motorString;
    private int currentImageIndex = 0;
    [SerializeField] private TMP_Text texts;
    void Start()
    {
        ShowImage(currentImageIndex);
    }

    void ShowImage(int index)
    {
      GetComponent<Image>().sprite = motorImages[index];
        texts.text = motorString[index];
        Debug.Log(index);
        ConnectUDP.CmdData.motorType = (MotorType)index;
    }

    public void NextImage()
    {
        currentImageIndex = (currentImageIndex + 1) % motorImages.Length;
        ShowImage(currentImageIndex);
    }

    public void PreviousImage()
    {
        currentImageIndex = (currentImageIndex - 1 + motorImages.Length) % motorImages.Length;
        ShowImage(currentImageIndex);
    }
}