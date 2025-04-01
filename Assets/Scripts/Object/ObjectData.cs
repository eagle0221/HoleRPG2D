using UnityEngine;

[System.Serializable]
public class ObjectData
{
    //public AbsorbableObjectData objects; // この行を削除
    public string objectName; // 追加
    public string objectSpriteName; // 追加
    public float moveSpeed; // 追加
    public float shrinkSpeed; // 追加
    public float minScale; // 追加
    public float destroyDistance; // 追加
    public float exp; // 追加
    public Vector3 position;
    public bool isAbsorbed;
}
