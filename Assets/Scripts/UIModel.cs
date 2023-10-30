using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class UIModel : MonoBehaviour
{
    public GameObject[] view;
    public GameObject topBt;
    public float slideToUnderSpeed;
    private static Action<GameObject> _onChangeView;

    public void ChangeView(GameObject obj)
    {
        _onChangeView?.Invoke(obj);
    }

    private void OnEnable()
    {
        _onChangeView += ActivateOneAndDeactivateOthersViews;
    }

    private void OnDisable()
    {
        _onChangeView -= ActivateOneAndDeactivateOthersViews;
    }

    public void TopImageActive(bool isActive)
    {
        topBt.SetActive(!isActive);
    }
    public void GoUnder(bool isUnder)
    {
        if (isUnder)
        {
            foreach (var t in view)
            {
                t.transform.localPosition = new Vector3(0, 0, 0);
            }
        }
        else

            foreach (var t in view)
            {
                t.transform.localPosition = new Vector3(0, -950, 0);
            }
    }

    private void ActivateOneAndDeactivateOthersViews(GameObject activatedView)
    {
        if (activatedView.activeSelf) return;
        Debug.Log("active");

        foreach (var t in view)
        {
            t.SetActive(false);
        }

        activatedView.SetActive(true);
    }
}