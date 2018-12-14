using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class AssetBundleLoader:IDisposable
{

    private AssetBundle bundle;
    public AssetBundleLoader(string path)
    {
        string fullPath = LocalFileMgr.Instance.LocalFilePath + path;
        bundle=AssetBundle.LoadFromMemory(LocalFileMgr.Instance.GetBuffer(fullPath));
    }

    public T LoadAsset<T>(string name)where T:Object
    {
        if (bundle == null) return default(T);
        return bundle.LoadAsset<T>(name);
    }

    public void Dispose()
    {
        if (bundle != null) bundle.Unload(false);
    }
}
