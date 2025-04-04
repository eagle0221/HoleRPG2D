using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;
using JetBrains.Annotations;

public class SaveManager : MonoBehaviour
{
    public PlayerController player;
    public List<ObjectData> spawnedObjects = new List<ObjectData>();
    private string saveFilePath;
    public Button saveButton;

    void Start()
    {
        saveFilePath = Path.Combine(Application.persistentDataPath, "saveData.json");
        Load();
        if (spawnedObjects.Count == 0)
        {
            //SpawnObjects();
        }
        saveButton.onClick.AddListener(Save);
    }

    void Save()
    {
        // オブジェクトの配置
        spawnedObjects.Clear();
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("AbsorbableObject"))
        {
            AbsorbableObject absorbableObject = obj.GetComponent<AbsorbableObject>();
            ObjectData objectData = new ObjectData
            {
                objectName = absorbableObject.objectData.objectName,
                objectSpriteName = absorbableObject.objectData.objectSprite.name,
                moveSpeed = absorbableObject.objectData.moveSpeed,
                shrinkSpeed = absorbableObject.objectData.shrinkSpeed,
                minScale = absorbableObject.objectData.minScale,
                destroyDistance = absorbableObject.objectData.destroyDistance,
                exp = absorbableObject.objectData.exp,
                position = obj.transform.position,
                isAbsorbed = false
            };
            spawnedObjects.Add(objectData);
        }
        
        // プレイヤー、インベントリ、装備のデータを取得
        Inventory inventory = player.inventory;
        EquipmentItem[] equipmentItems = player.equipments;

        // EquipmentItemの情報をEquipmentDataに変換
        EquipmentData[] equipmentDatas = new EquipmentData[equipmentItems.Length];
        for (int i = 0; i < equipmentItems.Length; i++)
        {
            if (equipmentItems[i] != null)
            {
                equipmentDatas[i] = new EquipmentData(equipmentItems[i]); // EquipmentDataのコンストラクタを使用
                player.UnEquip(equipmentItems[i]);
            }
            else
            {
                equipmentDatas[i] = null; // 装備されていない場合はnullを保存
            }
        }

        PlayerStatus lPlayerStatus = player.status;

        // Inventoryの情報をInventoryDataに変換
        InventoryData inventoryData = new InventoryData(inventory);

        // セーブデータを作成
        SaveData saveData = new SaveData
        {
            objects = spawnedObjects,
            playerStatus = lPlayerStatus,
            inventoryData = inventoryData,
            equipments = equipmentDatas,
            trackRecord = GameManager.Instance.trackRecord,
            resourceInfo = GameManager.Instance.resourceInfo
        };

        // JSON形式に変換して保存
        string json = JsonUtility.ToJson(saveData);
        File.WriteAllText(saveFilePath, json);
        Debug.Log(saveFilePath);
        Debug.Log("セーブデータが保存されました。");
        Debug.Log(json);
    }

    void Load()
    {
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
            SaveData saveData = JsonUtility.FromJson<SaveData>(json);
            spawnedObjects = saveData.objects;

            // 実績情報ロード
            GameManager.Instance.trackRecord = saveData.trackRecord;

            // リソース情報ロード
            GameManager.Instance.resourceInfo = saveData.resourceInfo;            

            // プレイヤー、インベントリ、装備のデータをロード
            player.status = saveData.playerStatus;
            //player.inventory = saveData.inventory; // この行を削除

            // Inventoryをロード
            player.inventory.items.Clear(); // 既存のアイテムをクリア
            foreach (EquipmentData itemData in saveData.inventoryData.items)
            {
                // ResourcesフォルダからEquipmentItemをロード
                EquipmentItem item = Resources.Load<EquipmentItem>("Equipment/" + itemData.itemName);
                if (item != null)
                {
                    player.inventory.AddItem(item);
                }
                else
                {
                    Debug.Log("Inventory not found: " + itemData.itemName);
                }
            }

            // 装備をロード
            for (int i = 0; i < saveData.equipments.Length; i++)
            {
                if (saveData.equipments[i] != null)
                {
                    // ResourcesフォルダからEquipmentItemをロード
                    EquipmentItem item = Resources.Load<EquipmentItem>("Equipment/" + saveData.equipments[i].itemName);
                    if (item != null)
                    {
                        player.Equip(item);
                    }
                    else
                    {
                        Debug.Log("EquipmentItem not found: " + saveData.equipments[i].itemName);
                    }
                }
            }

            Debug.Log(saveFilePath);
            Debug.Log("セーブデータがロードされました。");
            Debug.Log(JsonUtility.ToJson(saveData));
        }
    }
}
