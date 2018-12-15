using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIWindowViewBase : UIViewBase {

    /// <summary>
    /// �ҵ�����
    /// </summary>
    [SerializeField]
    public WindowUIContainerType containerType = WindowUIContainerType.Center;

    /// <summary>
    /// �򿪷�ʽ
    /// </summary>
    [SerializeField]
    public WindowShowStyle showStyle = WindowShowStyle.Normal;

    /// <summary>
    /// �򿪻�رն���Ч������ʱ��
    /// </summary>
    [SerializeField]
    public float duration = 0.2f;

    /// <summary>
    /// ��ǰ��������
    /// </summary>
    [HideInInspector]
    public WindowUIType CurrentUIType;

    /// <summary>
    /// ��һ��Ҫ�򿪵Ĵ���
    /// </summary>
    protected WindowUIType NextOpenWindow = WindowUIType.None;

    protected override void OnBtnClick(GameObject go)
    {
        base.OnBtnClick(go);
        if (go.name.Equals("btn_Close",StringComparison.CurrentCultureIgnoreCase))
        {
            Close();
        }
    }

    /// <summary>
    /// �رմ���
    /// </summary>
    protected virtual void Close()
    {
        WindowUIMgr.Instance.CloseWindow(CurrentUIType);
    }

    /// <summary>
    /// ����֮ǰִ��
    /// </summary>
    protected override void BeforeOnDestroy()
    {
        LayerUIMgr.Instance.CheckOpenWindow();
        if (NextOpenWindow == WindowUIType.None) return;
        WindowUIMgr.Instance.OpenWindow(NextOpenWindow);
    }
}
