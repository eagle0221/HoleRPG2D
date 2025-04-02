using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using System;

public class AdmobUnitBanner : AdmobUnitBase
{
    private BannerView bannerView;

    protected override void Initialize()
    {
        ShowBanner();
    }

    public void ShowBanner()
    {
        UnitDestroy();

        AdSize adaptiveSize = AdSize.GetCurrentOrientationAnchoredAdaptiveBannerAdSizeWithWidth(AdSize.FullWidth);

        bannerView = new BannerView(
            UnitID,
            adaptiveSize,
            AdPosition.Bottom);

        bannerView.OnBannerAdLoaded += () =>
        {
            Debug.Log("ロードされました - 表示します");
            bannerView.Show();
        };
        bannerView.OnBannerAdLoadFailed += (LoadAdError error) =>
        {
            Debug.LogError("ロード失敗しました");
        };

        //リクエストを生成
        var adRequest = new AdRequest();
        bannerView.LoadAd(adRequest);
    }

    private void UnitDestroy()
    {
        if (bannerView != null)
        {
            Debug.Log("Destroying banner view.");
            bannerView.Destroy();
            bannerView = null;
        }
    }

    private void OnDestroy()
    {
        UnitDestroy();
    }
}