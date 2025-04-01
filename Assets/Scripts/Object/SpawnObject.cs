using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class SpawnObject : MonoBehaviour
{
    public GameObject objectToSpawn; // 配置するオブジェクトのプレハブ
    [SerializeField] private SaveManager saveManager;
    public List<Vector3> positions;
    public List<AbsorbableObjectData> objectDataLists; // オブジェクトのデータ

    void Start()
    {
        string saveFilePath = Path.Combine(Application.persistentDataPath, "saveData.json");
        Debug.Log(saveManager.spawnedObjects.Count > 0);
        Debug.Log(File.Exists(saveFilePath));
        if(saveManager.spawnedObjects.Count > 0 || File.Exists(saveFilePath))
        {
            foreach (ObjectData data in saveManager.spawnedObjects)
            {
                if (!data.isAbsorbed)
                {
                    GameObject obj = Instantiate(objectToSpawn, data.position, Quaternion.identity);
                    AbsorbableObject absorbableObject = obj.GetComponent<AbsorbableObject>();
                    absorbableObject.objectData = Resources.Load<AbsorbableObjectData>("Absorbable Object Data/" + data.objectName);
                    absorbableObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/" + data.objectSpriteName);
                }
            }
        }
        else
        {
            positionInitialize();
            foreach (Vector3 position in positions)
            {
                // objectDataから情報を初期化
                int rnd = Random.Range(0, objectDataLists.Count);

                GameObject obj = Instantiate(objectToSpawn, position, Quaternion.identity);
                AbsorbableObject absorbableObject = obj.GetComponent<AbsorbableObject>();
                absorbableObject.GetComponent<SpriteRenderer>().sprite = objectDataLists[rnd].objectSprite;
                absorbableObject.objectData = objectDataLists[rnd];
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
                saveManager.spawnedObjects.Add(objectData);
            }
            Debug.Log("positionInitialize End");
        }
    }

    void positionInitialize()
    {
        float x = -12.0f;
        float y = -9.0f;
        for(int iY = 0; iY < 3; iY++)
        {
            x = -12.0f;
            for(int iX = 0; iX < 15; iX++)
            {
                positions.Add(new Vector3(x, y, -1));
                x += 0.5f;
            }
            y += 1.5f;
        }

        x = 4.0f;
        y = -9.0f;
        for(int iY = 0; iY < 3; iY++)
        {
            x = 4.0f;
            for(int iX = 0; iX < 15; iX++)
            {
                positions.Add(new Vector3(x, y, -1));
                x += 0.5f;
            }
            y += 1.5f;
        }
    }

}
