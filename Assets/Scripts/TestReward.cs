using UnityEngine;
using UnityEngine.UI;

public class TestReward : MonoBehaviour
{
    public Button showRewardButton;

    public AdmobUnitReward admobUnitReward;

    private void Start()
    {
        showRewardButton.interactable = false;

        showRewardButton.onClick.AddListener(() =>
        {
            admobUnitReward.ShowRewardAd((reward) =>
            {
                if (reward != null)
                {
                    Debug.Log("Reward type: " + reward.Type);
                    Debug.Log("Reward received: " + reward.Amount);
                }
            });
            showRewardButton.interactable = false;
        });
    }

    private void Update()
    {
        showRewardButton.interactable = admobUnitReward.IsReady;
    }
}