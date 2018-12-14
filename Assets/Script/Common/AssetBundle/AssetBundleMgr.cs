using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetBundleMgr : Singleton<AssetBundleMgr> {

    public GameObject Load(string path, string name)
    {
        using (AssetBundleLoader loader=new AssetBundleLoader(path))
        {
            return loader.LoadAsset<GameObject>(name);
        }
    }

    /// <summary>
    /// ¼ÓÔØ²¢¿ËÂ¡
    /// </summary>
    /// <param name="path"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public GameObject LoadAndClone(string path, string name)
    {
        using (AssetBundleLoader loader = new AssetBundleLoader(path))
        {
            return Object.Instantiate(loader.LoadAsset<GameObject>(name));
        }
    }


    public AssetBundleLoaderAsync LoadAsync(string path, string name)
    {
        GameObject obj=new GameObject("AssetBundleLoaderAsync");
        AssetBundleLoaderAsync async = obj.GetOrCreatComponent<AssetBundleLoaderAsync>();
        async.Init(path,name);
        return async;
    }
}
