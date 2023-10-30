using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPresenter : MonoBehaviour
{
    public bool isUnderView;
    private UIModel _uiModel;
    private UIView _uiView;

    private void Awake()
    {
        _uiModel = FindObjectOfType<UIModel>();
        _uiView = FindObjectOfType<UIView>();
    }

    private void Start()
    {
        _uiView.UpdateViewActive(_uiModel.view[0]);
    }

    public void SubjectButton_1()
    {
        GameObject obj = _uiModel.view[0];
        if (obj.activeSelf)
        {
            _uiModel.GoUnder(isUnderView);
            isUnderView = !isUnderView;
        }

        _uiModel.ChangeView(obj);
    }

    public void SubjectButton_2()
    {
        GameObject obj = _uiModel.view[1];
        if (obj.activeSelf)
        {
            _uiModel.GoUnder(isUnderView);
            isUnderView = !isUnderView;
        }

        _uiModel.ChangeView(obj);
    }

    public void SubjectButton_3()
    {
        GameObject obj = _uiModel.view[2];
        if (obj.activeSelf)
        {
            _uiModel.GoUnder(isUnderView);
            isUnderView = !isUnderView;
        }

        _uiModel.ChangeView(obj);
    }

    public void SubjectButton_4()
    {
        GameObject obj = _uiModel.view[3];
        if (obj.activeSelf)
        {
            _uiModel.GoUnder(isUnderView);
            isUnderView = !isUnderView;
        }

        _uiModel.ChangeView(obj);
    }

    public void TopButton()
    {
        bool objActive = _uiModel.topBt.activeSelf;
        _uiModel.TopImageActive(objActive);
    }
}