using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccountCtrl : Singleton<AccountCtrl> {

    public void OpenLogOnWindow()
    {
        WindowUIMgr.Instance.OpenWindow(WindowUIType.LogOn);
    }
}
