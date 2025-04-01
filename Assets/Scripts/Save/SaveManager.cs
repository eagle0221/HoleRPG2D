using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;
using JetBrains.Annotations;

public class SaveManager : MonoBehaviour
{
    public PlayerController player;
    public GameObject objectToSpawn; // 配置するオブジェクトのプレハブ
    public int numberOfObjects = 10; // 配置するオブジェクトの数
    public Vector2 spawnAreaMin = new Vector2(-5, -5); // 配置範囲の最小座標
    public Vector2 spawnAreaMax = new Vector2(5, 5); // 配置範囲の最大座標
    public List<ObjectData> spawnedObjects = new List<ObjectData>();
    private string saveFilePath;
    public Button saveButton;
    //public float spawnInterval = 2f;
    //private float timer = 0f;

    void Start()
    {
        saveFilePath = Path.Combine(Application.persistentDataPath, "saveData.json");
        DataLoad();
        if (spawnedObjects.Count == 0)
        {
            //SpawnObjects();
        }
        saveButton.onClick.AddListener(DataSave);
    }

    void DataSave()
    {
        // オブジェクトの配置
        //ObjectData objectData = new();
        //spawnedObjects.Add(objectData);
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




        PlayerStatus playerStatus = player.status;

        // Inventoryの情報をInventoryDataに変換
        InventoryData inventoryData = new InventoryData(inventory);

        // セーブデータを作成
        SaveData saveData = new SaveData
        {
            objects = spawnedObjects,
            player = playerStatus,
            inventoryData = inventoryData, // inventoryDataを保存
            equipments = equipmentDatas
        };

        // JSON形式に変換して保存
        string json = JsonUtility.ToJson(saveData);
        File.WriteAllText(saveFilePath, json);
        Debug.Log(saveFilePath);
        Debug.Log("セーブデータが保存されました。");
        Debug.Log(json);
    }

    void DataLoad()
    {
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
            SaveData saveData = JsonUtility.FromJson<SaveData>(json);
            spawnedObjects = saveData.objects;

            foreach (ObjectData data in spawnedObjects)
            {
                if (!data.isAbsorbed)
                {
                    GameObject obj = Instantiate(objectToSpawn, data.position, Quaternion.identity);
                    AbsorbableObject absorbableObject = obj.GetComponent<AbsorbableObject>();
                    absorbableObject.objectData = Resources.Load<AbsorbableObjectData>("Absorbable Object Data/" + data.objectName);
                    absorbableObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/" + data.objectSpriteName);
                }
            }

            // プレイヤー、インベントリ、装備のデータをロード
            player.status = saveData.player;
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
                    Debug.LogError("EquipmentItem not found: " + itemData.itemName);
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
                        Debug.LogError("EquipmentItem not found: " + saveData.equipments[i].itemName);
                    }
                }
            }

            Debug.Log(saveFilePath);
            Debug.Log("セーブデータがロードされました。");
            Debug.Log(JsonUtility.ToJson(saveData));
        }
    }
}
