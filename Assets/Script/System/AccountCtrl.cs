using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccountCtrl : Singleton<AccountCtrl>
{
    public AccountCtrl()
    {
        EnventDispather.Instance.RegisterListenerBtn("UILogOnView_btnLogOn", OnLogOnViewBtnLogOnClickCallBack);
        EnventDispather.Instance.RegisterListenerBtn("UILogOnView_btnReg", OnLogOnViewBtnToRegClickCallBack);
    }

    private void OnLogOnViewBtnToRegClickCallBack(object[] param)
    {
        Debug.Log("去注册按钮");
    }

    private void OnLogOnViewBtnLogOnClickCallBack(object[] param)
    {
        Debug.Log("登录按钮");
    }

    public void OpenLogOnWindow()
    {
        WindowUIMgr.Instance.OpenWindow(WindowUIType.LogOn).GetComponent<UILogOnView>();
    }

    public override void Dispose()
    {
        base.Dispose();
        EnventDispather.Instance.RemoveListenerBtn("UILogOnView_btnLogOn", OnLogOnViewBtnLogOnClickCallBack);
        EnventDispather.Instance.RemoveListenerBtn("UILogOnView_btnReg", OnLogOnViewBtnToRegClickCallBack);
    }
}
