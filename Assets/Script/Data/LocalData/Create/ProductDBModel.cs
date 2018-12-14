using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 商品数据管理类
/// </summary>
public partial class ProductDBModel: AbstractDBModel<ProductDBModel, ProductEntity>
{
    protected override string FileName
    {
        get { return "Item.data"; }
    }

    protected override ProductEntity MakeEntity(GameDataTableParser parser)
    {
        ProductEntity entity = new ProductEntity();
        entity.Id = parser.GetFieldValue("Id").ToInt();
        entity.Name = parser.GetFieldValue("Name");
        entity.Price = parser.GetFieldValue("Price").ToFloat();
        entity.PicName = parser.GetFieldValue("PicName");
        entity.Desc = parser.GetFieldValue("Desc");
        return entity;
    }
}
