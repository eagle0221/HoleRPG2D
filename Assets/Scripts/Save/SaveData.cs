using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public List<ObjectData> objects;    // 配置オブジェクトのリスト
    public PlayerStatus playerStatus;   // プレイヤー情報
    public InventoryData inventoryData; // 所持アイテム
    public EquipmentData[] equipments;  // 装備アイテム
    public TrackRecord trackRecord;     // 実績情報
    public ResourceInfo resourceInfo;   // リソース情報 
}
