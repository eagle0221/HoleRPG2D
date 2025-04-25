using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Collections;

public class SaveManager : MonoBehaviour
{
    public PlayerController player;
    private string saveFilePath;
    public Button saveButton;
    public bool autoSave = true;
    private float saveInterval = 10f; // セーブ間隔（秒）

    void Awake()
    {
        saveFilePath = Path.Combine(Application.persistentDataPath, "saveData.json");
        Debug.Log(saveFilePath);
    }

    void Start()
    {
        Load();
        saveButton.onClick.AddListener(Save);
        StartCoroutine(AutoSave()); // 自動セーブを開始
    }

    IEnumerator AutoSave()
    {
        while (autoSave)
        {
            yield return new WaitForSeconds(saveInterval); // 指定された間隔で待機
            Debug.Log("自動セーブを実行します...");
            Save(); // セーブを実行
        }
    }

    // --- アプリケーションの一時停止/終了時のセーブ処理 ---
    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            // アプリケーションがバックグラウンドに移った（一時停止した）
            Debug.Log("アプリケーションが一時停止しました。データを保存します...");
            Save(); // セーブ処理を呼び出す
        }
        // else
        // {
        //     // アプリケーションがフォアグラウンドに戻った（再開した）
        //     Debug.Log("アプリケーションが再開しました。");
        // }
    }

    private void OnApplicationQuit()
    {
        // アプリケーションが終了する直前
        Debug.Log("アプリケーションが終了します。データを保存します...");
        Save(); // ここでも念のためセーブ処理を呼び出す
    }
    // --- 追加ここまで ---

    void Save()
    {
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
                //player.UnEquip(equipmentItems[i]);
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
            playerStatus = lPlayerStatus,
            inventoryData = inventoryData,
            equipments = equipmentDatas,
            trackRecord = new TrackRecord(
                        GameManager.Instance.trackRecord.PlayerDieCount,
                        GameManager.Instance.trackRecord.EnemyAbsorbCount,
                        GameManager.Instance.trackRecord.RebirthCount,
                        GameManager.Instance.trackRecord.ObjectAbsorbCount
            ) ,
            resourceInfo = GameManager.Instance.resourceInfo
        };

        // JSON形式に変換して保存
        string json = JsonUtility.ToJson(saveData);
        string encrypted = AESUtil.Encrypt(json);
        File.WriteAllText(saveFilePath, encrypted);
        Debug.Log("Saved encrypted data: " + encrypted);
        Debug.Log(json);
    }

    void Load()
    {
        if (File.Exists(saveFilePath))
        {
            string encrypted = File.ReadAllText(saveFilePath);
            string decrypted = AESUtil.Decrypt(encrypted);
            SaveData saveData = JsonUtility.FromJson<SaveData>(decrypted);

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
                EquipmentItem item = Resources.Load<EquipmentItem>("ScritableObject/Equipment/" + itemData.itemName);
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
                    EquipmentItem item = Resources.Load<EquipmentItem>("ScritableObject/Equipment/" + saveData.equipments[i].itemName);
                    if (item != null)
                    {
                        player.equipments[i] = item;
                    }
                    else
                    {
                        Debug.Log("EquipmentItem not found: " + saveData.equipments[i].itemName);
                    }
                }
            }

            player.UpdatePlayerStatus();

            Debug.Log("セーブデータがロードされました。");
            Debug.Log(JsonUtility.ToJson(saveData));
        }
    }
}
