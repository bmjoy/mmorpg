//===================================================
//作    者：边涯  http://www.u3dol.com  QQ群：87481002
//创建时间：2015-11-16 21:55:34
//备    注：
//===================================================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 
/// </summary>
public class TestScene : MonoBehaviour 
{
    void Start()
    {
        //AssetBundleMgr.Instance.LoadAndClone("Role/role_mainplayer.assetbundle", "Role_MainPlayer");
        AssetBundleMgr.Instance.LoadAsync("Role/role_mainplayer.assetbundle", "Role_MainPlayer").OnLoadComplete =
            obj => { Instantiate((GameObject)obj); };
    }

    //每帧执行
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.A))
        {
            SceneMgr.Instance.LoadToCity();
        }
        else if (Input.GetKeyUp(KeyCode.B))
        {
        }
    }

    //Update之后执行
    void LateUpdate()
    {
        //Debug.Log("LateUpdate");
    }

    //固定时间间隔执行
    void FixedUpdate()
    {
        //Debug.Log("FixedUpdate");
    }

    //销毁的时候执行
    void OnDestroy()
    {
        //Debug.Log("OnDestroy");
    }

    //脚本可用的时候执行 在Start之前
    void OnEnable()
    {
        //Debug.Log("OnEnable");
    }

    //禁用的时候执行
    void OnDisable()
    {
        //Debug.Log("OnDisable");
    }
}