using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetBundleEntity
{


    public string Key;

    public string Name;

    public string Tag;

    public int Version;

    public long Size; 

    public string ToPath;

    private List<string> m_PathList=new List<string>();

    public List<string> PathList
    {
        get { return m_PathList; }
    }
}
