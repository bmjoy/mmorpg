using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetWorkHttp : SingletonMono<NetWorkHttp>
{

    private Action<CallBackArgs> callBackAction;
    private CallBackArgs callBackArgs;
    public bool IsBusy { get; private set; }

    protected override void OnStart()
    {
        base.OnStart();
        callBackArgs=new CallBackArgs();
    }
    /// <summary>
    /// 请求Http数据
    /// </summary>
    /// <param name="url"></param>
    /// <param name="callBack"></param>
    /// <param name="isPost"></param>
    /// <param name="json"></param>
    public void SendData(string url, Action<CallBackArgs> callBack,bool isPost=false,string json="")
    {
        if(IsBusy)return;
        IsBusy = true;
        callBackAction = callBack;
        if (!isPost)
        {
            Get(url);
        }
        else
        {
            Post(url,json);
        }
        
    }
    /// <summary>
    /// Get请求
    /// </summary>
    /// <param name="url"></param>
    private void Get(string url)
    {
        WWW data=new WWW(url);
        StartCoroutine(Request(data));
    }
    /// <summary>
    /// Post请求
    /// </summary>
    /// <param name="url"></param>
    /// <param name="json"></param>
    private void Post(string url, string json)
    {
        WWWForm form=new WWWForm();
        form.AddField("", json);
        WWW data = new WWW(url,form);
        StartCoroutine(Request(data));
    }

    /// <summary>
    /// Request数据
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    private IEnumerator Request(WWW data)
    {
        yield return data;
        IsBusy = false;
        if (string.IsNullOrEmpty(data.error))
        {
            if (data.text == "null")
            {
                if (callBackAction != null)
                {
                    callBackArgs.HasError = true;
                    callBackArgs.ErrorMsg = "data is null";
                    callBackAction(callBackArgs);
                }
            }
            else
            {
                if (callBackAction != null)
                {
                    callBackArgs.HasError = false;
                    callBackArgs.Json = data.text;
                    callBackAction(callBackArgs);
                }
            }
        }
        else
        {
            if (callBackAction != null)
            {
                callBackArgs.HasError = true;
                callBackArgs.ErrorMsg = data.error;
                callBackAction(callBackArgs);
            }
        }
    }

   

    public class CallBackArgs
    {
        public bool HasError;
        public string ErrorMsg;
        public string Json;
    }
}
