using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���ݹ������
/// </summary>
/// <typeparam name="T">��������</typeparam>
/// <typeparam name="TP">ʵ��������</typeparam>
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

    #region ����
    private static T _instance;
    public static T Instance
    {
        get { return _instance ?? (_instance = new T()); }
    }
    #endregion

    #region ��Ҫ����ʵ�ֵ����Ժͷ���
    /// <summary>
    /// �����ļ���
    /// </summary>
    protected abstract string FileName { get; }
    /// <summary>
    /// ����ʵ��
    /// </summary>
    /// <param name="parser"></param>
    /// <returns></returns>
    protected abstract TP MakeEntity(GameDataTableParser parser);
    #endregion

    /// <summary>
    /// ��������
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
    /// ����Id��ȡʵ��
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public TP GetEntityById(int id)
    {
        if (Dic.ContainsKey(id))
        {
            return Dic[id];
        }
        throw new Exception("�ֵ����Ҳ�������Ʒ");

    }

    /// <summary>
    /// ��ȡʵ���б�
    /// </summary>
    /// <returns></returns>
    public List<TP> GetList()
    {
        return ProductEntities;
    }
}
