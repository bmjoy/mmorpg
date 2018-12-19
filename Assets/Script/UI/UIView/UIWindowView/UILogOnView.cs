using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILogOnView : UIWindowViewBase
{
    protected override void OnBtnClick(GameObject go)
    {
        base.OnBtnClick(go);
        switch (go.name)
        {
            case "btnLogOn":
                EnventDispather.Instance.DispachBtn("UILogOnView_btnLogOn");
            break;
            case "btnReg":
                EnventDispather.Instance.DispachBtn("UILogOnView_btnReg");
            break;
        }
    }
}
