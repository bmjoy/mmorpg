using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using LitJson;

public class TestMemory : MonoBehaviour {

	void Start ()
	{
	    //NetWorkHttp.Instance.SendData(GlobalInit.WebAccountUrl+ "api/account?id=100",GetCallBack);
     //   JsonData jsonData=new JsonData();
	    //jsonData["Type"] = 0;
	    //jsonData["UserName"] = "xxx";
     //   jsonData["Pwd"] = "123";

     //   NetWorkHttp.Instance.SendData(GlobalInit.WebAccountUrl + "api/account", PostCallBack, true, jsonData.ToJson());

        NetWorkSocket.Instance.Connect("192.168.1.111",1011);
	    using (MMO_MemoryStream ms=new MMO_MemoryStream())
	    {
	        ms.WriteUTF8String("你好啊");
	        NetWorkSocket.Instance.SendMsg(ms.ToArray());
        }
        
    }

    private void GetCallBack(NetWorkHttp.CallBackArgs obj)
    {
        
        if (obj.HasError)
        {
            Debug.Log(obj.ErrorMsg);
        }
        else
        {
            AccountEntity entity = LitJson.JsonMapper.ToObject<AccountEntity>(obj.Json);
            Debug.Log(entity.UserName);
        }
        
    }
    private void PostCallBack(NetWorkHttp.CallBackArgs obj)
    {

        if (obj.HasError)
        {
            Debug.Log(obj.ErrorMsg);
        }
        else
        {
            RetValue entity = LitJson.JsonMapper.ToObject<RetValue>(obj.Json);
            Debug.Log(entity.RetData);
        }

    }
}
