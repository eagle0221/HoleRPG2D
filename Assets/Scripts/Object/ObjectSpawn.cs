using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ObjectSpawn : MonoBehaviour
{
    public GameObject objectToSpawn; // 配置するオブジェクトのプレハブ
    [SerializeField] private Transform spawnTransform;
    // HoleTown: x: -12.0f, y: -9.0f x: 5.0f, y: -9.0f
    // Sweets  : x: -12.0f, y: -9.0f x: 5.0f, y: -9.0f
    [SerializeField] private List<Vector2> fieldsPositionStart = new List<Vector2>();
    [SerializeField] private List<Vector2> fieldsPositionEnd = new List<Vector2>();
    
    private List<Vector3> positions = new List<Vector3>();
    public List<AbsorbableObjectData> objectDataLists; // オブジェクトのデータ

    public void OnEnable()
    {
        if(positions != null)
        {
            if(positions.Count() == 0)
            {
                positionInitialize();
            }
        }
        foreach (Vector3 position in positions)
        {
            // objectDataから情報を初期化
            int rnd = Random.Range(0, objectDataLists.Count);

            GameObject obj = Instantiate(objectToSpawn, position, Quaternion.identity, spawnTransform);
            AbsorbableObject absorbableObject = obj.GetComponent<AbsorbableObject>();
            absorbableObject.GetComponent<SpriteRenderer>().sprite = objectDataLists[rnd].objectSprite;
            absorbableObject.objectData = objectDataLists[rnd];
        }
    }
    public void positionInitialize()
    {
        float x = 0f;
        float y = 0f;
        for(int i = 0; i < fieldsPositionStart.Count(); i++)
        {
            x = 0f;
            for(int j = 0; j < 15; j++)
            {
                positions.Add(new Vector3(fieldsPositionStart[i].x + x, fieldsPositionStart[i].y + y, -1f));
                x += 0.5f;
            }
            //y += 0.8f;
        }
    }
}
