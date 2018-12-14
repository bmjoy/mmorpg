using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonMono<T> : MonoBehaviour where T:MonoBehaviour {
    #region µ¥Àý
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject obj = new GameObject(typeof(T).Name);
                DontDestroyOnLoad(obj);
                _instance = obj.GetOrCreatComponent<T>();
            }

            return _instance;
        }
    }
    #endregion

    void Awake()
    {
        OnAwake();
    }

    void Start ()
    {
        OnStart();
    }
	
	void Update ()
	{
	    OnUpdate();
	}

    void OnDestroy()
    {
        BeforeOnDestroy();
    }

    protected virtual void OnStart() { }
    protected virtual void OnAwake() { }
    protected virtual void OnUpdate() { }
    protected virtual void BeforeOnDestroy() { }
}
