using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnventDispather : Singleton<EnventDispather>
{

    public delegate void OnActionHandler(byte[] buffer);

    public Dictionary<ushort,List<OnActionHandler>> dic=new Dictionary<ushort, List<OnActionHandler>>();

    /// <summary>
    /// 分发消息
    /// </summary>
    public void Dispach(ushort protoCode,byte[] buffer)
    {
        if (dic.ContainsKey(protoCode))
        {
            if (dic[protoCode]!=null&&dic[protoCode].Count > 0)
            {
                for (int i = 0; i < dic[protoCode].Count; i++)
                {
                    if (dic[protoCode][i] != null)
                    {
                        dic[protoCode][i](buffer);
                    }
                }
            }
        }
    }

    public void RegisterListener(ushort protoCode, OnActionHandler handler)
    {
        if (dic.ContainsKey(protoCode))
        {
            dic[protoCode].Add(handler);
        }
        else
        {
            List<OnActionHandler> handlers=new List<OnActionHandler>();
            handlers.Add(handler);
            dic[protoCode] = handlers;
        }
    }

    public void RemoveListener(ushort protoCode, OnActionHandler handler)
    {
        if (dic.ContainsKey(protoCode))
        {
            dic[protoCode].Remove(handler);
            if (dic[protoCode].Count == 0)
            {
                dic.Remove(protoCode);
            }
        }
    }
}
