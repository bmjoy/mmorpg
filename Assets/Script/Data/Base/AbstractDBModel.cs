using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 数据管理基类
/// </summary>
/// <typeparam name="T">子类类型</typeparam>
/// <typeparam name="TP">实体类类型</typeparam>
public abstract class AbstractDBModel<T, TP> where T : class, new() where TP : AbstractEntity
{
    //
    protected List<TP> ProductEntities;
    protected Dictionary<int, TP> Dic;
    protected AbstractDBModel()
    {
        ProductEntities = new List<TP>();
        Dic = new Dictionary<int, TP>();
        LoadData();
    }

    #region 单例
    private static T _instance;
    public static T Instance
    {
        get { return _instance ?? (_instance = new T()); }
    }
    #endregion

    #region 需要子类实现的属性和方法
    /// <summary>
    /// 数据文件名
    /// </summary>
    protected abstract string FileName { get; }
    /// <summary>
    /// 构造实体
    /// </summary>
    /// <param name="parser"></param>
    /// <returns></returns>
    protected abstract TP MakeEntity(GameDataTableParser parser);
    #endregion

    /// <summary>
    /// 加载数据
    /// </summary>
    private void LoadData()
    {
        using (GameDataTableParser parser = new GameDataTableParser(String.Format(@"E:\unityProject\MMORPG\www\Data\{0}", FileName)))
        {
            while (!parser.Eof)
            {
                TP entity = MakeEntity(parser);
                ProductEntities.Add(entity);
                Dic[entity.Id] = entity;
                parser.Next();
            }
        }
    }

    /// <summary>
    /// 根据Id获取实体
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public TP GetEntityById(int id)
    {
        if (Dic.ContainsKey(id))
        {
            return Dic[id];
        }
        throw new Exception("字典里找不到该物品");

    }

    /// <summary>
    /// 获取实体列表
    /// </summary>
    /// <returns></returns>
    public List<TP> GetList()
    {
        return ProductEntities;
    }
}
