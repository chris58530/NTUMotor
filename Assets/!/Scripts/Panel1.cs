using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Panel1 : MonoBehaviour
{
    [SerializeField] private Sprite[] motorImages;
    private int currentImageIndex = 0;

    void Start()
    {
        ShowImage(currentImageIndex);
    }

    void ShowImage(int index)
    {
        var sprite = GetComponent<Image>().sprite = motorImages[index];
        //這裡插入SO數值
        Debug.Log(index);
        ConnectUDP.CmdData.motorType = (MotorType)index;
    }

    public void NextImage()
    {
        // 切换到下一张图像
        currentImageIndex = (currentImageIndex + 1) % motorImages.Length;
        ShowImage(currentImageIndex);
    }

    public void PreviousImage()
    {
        // 切换到上一张图像
        currentImageIndex = (currentImageIndex - 1 + motorImages.Length) % motorImages.Length;
        ShowImage(currentImageIndex);
    }
}