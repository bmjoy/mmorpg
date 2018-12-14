using UnityEngine;
using System.Collections;

public class LogOnSceneCtrl : MonoBehaviour {

    GameObject obj;

    void Awake()
    {
        SceneUIMgr.Instance.LoadSceneUI(SceneUIMgr.SceneUIType.LogOn);
    }
	
}