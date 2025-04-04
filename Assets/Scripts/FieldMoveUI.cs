using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections.Generic;

public class FieldMoveUI : MonoBehaviour
{

    private Vector3 moveInitPos = new Vector3(0, -9, -1);
    
    [SerializeField] private GameObject windowPanel;
    public GameObject mainField; // MainFieldのGameObject
    public GameObject anotherWorld; // AnotherWorldのGameObject
    [SerializeField] private Button yesButton;
    [SerializeField] private Button noButton;

    void Start()
    {
        yesButton.onClick.AddListener(OnYesButtonClicked);
        noButton.onClick.AddListener(OnNoButtonClicked);
    }

    // コリジョン開始イベント
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Test:OnTriggerEnter2D:" + other.gameObject.name);
            // 確認ウィンドウ表示
            windowPanel.SetActive(true);
        }
    }

    // コリジョン終了イベント
    void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log("Test:OnTriggerExit2D:" + other.gameObject.name);
    }

    private void OnYesButtonClicked()
    {
        // 既に出現していた敵を削除
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            Destroy(obj);
        }
        LoadField();
        windowPanel.SetActive(false);
    }

    public void LoadField()
    {
        anotherWorld.SetActive(false);
        mainField.SetActive(true);
        // プレイヤーの位置を初期化
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            player.transform.position = moveInitPos;
        }
    }

    private void OnNoButtonClicked()
    {
        windowPanel.SetActive(false);
    }
}
