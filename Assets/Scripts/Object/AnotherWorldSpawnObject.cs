using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class AnotherWorldSpawnObject : MonoBehaviour
{
    public GameObject objectToSpawn; // 配置するオブジェクトのプレハブ
    [SerializeField] private Transform spawnTransform;
    public List<Vector3> positions;
    public List<AbsorbableObjectData> objectDataLists; // オブジェクトのデータ

    public void OnEnable()
    {
        
        positionInitialize();
        foreach (Vector3 position in positions)
        {
            // objectDataから情報を初期化
            int rnd = Random.Range(0, objectDataLists.Count);

            GameObject obj = Instantiate(objectToSpawn, position, Quaternion.identity, spawnTransform);
            AbsorbableObject absorbableObject = obj.GetComponent<AbsorbableObject>();
            absorbableObject.GetComponent<SpriteRenderer>().sprite = objectDataLists[rnd].objectSprite;
            Debug.Log(objectDataLists[rnd].objectSprite.name);
            Debug.Log(obj.name);
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
        }
        Debug.Log("positionInitialize End");
    }
    public void positionInitialize()
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
            y += 0.8f;
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
            y += 0.8f;
        }
    }

}
