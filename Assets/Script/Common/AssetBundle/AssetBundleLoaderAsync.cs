using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class AssetBundleLoaderAsync : MonoBehaviour
{
    private string m_FullPath;
    private string m_Name;

    private AssetBundleCreateRequest request;
    private AssetBundle bundle;

    public Action<Object> OnLoadComplete;

    public void Init(string path,string name)
    {
        m_FullPath =LocalFileMgr.Instance.LocalFilePath+path;
        m_Name = name;
    }
	void Start ()
	{
	    StartCoroutine(Load());
	}

    private IEnumerator Load()
    {
        request=AssetBundle.LoadFromFileAsync(m_FullPath);
        yield return request;
        bundle = request.assetBundle;
        if (OnLoadComplete != null)
        {
            OnLoadComplete(bundle.LoadAsset(m_Name));
            Destroy(gameObject);
        }
    }

    void OnDestroy()
    {
        if(bundle!=null)bundle.Unload(false);
        name = null;
        m_FullPath = null;
    }
}
