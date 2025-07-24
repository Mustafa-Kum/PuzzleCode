using System;
using com.adjust.sdk;
using UnityEngine;

namespace _Game.Scripts.Managers.Core.Ads.AdMediations
{
    public class AppLovinMaxService : IAdService
    {
        private Action _onAdSuccessful;

        #region CONST VARIABLES

        private const string InterstitialAdUnitId = "a1787bc42a6bb64a";
        private const string RewardedAdUnitId = "4728d51a332815c3";
        private const string BannerAdUnitId = "5c9f81d92402a84e";

        #endregion

        #region PUBLIC METHODS

        public void Initialize()
        {
            // Setup listeners
            MaxSdkCallbacks.Banner.OnAdRevenuePaidEvent += OnAdRevenuePaidEvent;
            MaxSdkCallbacks.Rewarded.OnAdRevenuePaidEvent += OnAdRevenuePaidEvent;
            MaxSdkCallbacks.Interstitial.OnAdRevenuePaidEvent += OnAdRevenuePaidEvent;
            MaxSdkCallbacks.Rewarded.OnAdReceivedRewardEvent += OnRewardedAdReceivedReward;
            MaxSdkCallbacks.Rewarded.OnAdDisplayedEvent += OnRewardedAdDisplayed;

            MaxSdkCallbacks.Rewarded.OnAdLoadedEvent += OnRewardedAdLoaded;
            MaxSdkCallbacks.Interstitial.OnAdLoadedEvent += OnInterstitialAdLoaded;
            MaxSdkCallbacks.Banner.OnAdLoadedEvent += OnBannerAdLoaded;

            LoadRewardedAd();
            LoadInterstitialAd();
        }

        public void Uninitialize()
        {
            // Unhook listeners
            MaxSdkCallbacks.Banner.OnAdRevenuePaidEvent -= OnAdRevenuePaidEvent;
            MaxSdkCallbacks.Rewarded.OnAdRevenuePaidEvent -= OnAdRevenuePaidEvent;
            MaxSdkCallbacks.Interstitial.OnAdRevenuePaidEvent -= OnAdRevenuePaidEvent;
            MaxSdkCallbacks.Rewarded.OnAdReceivedRewardEvent -= OnRewardedAdReceivedReward;
            MaxSdkCallbacks.Rewarded.OnAdDisplayedEvent -= OnRewardedAdDisplayed;

            MaxSdkCallbacks.Rewarded.OnAdLoadedEvent -= OnRewardedAdLoaded;
            MaxSdkCallbacks.Interstitial.OnAdLoadedEvent -= OnInterstitialAdLoaded;
            MaxSdkCallbacks.Banner.OnAdLoadedEvent -= OnBannerAdLoaded;
        }

        public void ShowRewardedAd(Action onAdSuccessful)
        {
            _onAdSuccessful = onAdSuccessful;
            
            Debug.Log("AppLovinMaxService: ShowRewardedAd");

            if (MaxSdk.IsRewardedAdReady(RewardedAdUnitId))
            {
                MaxSdk.ShowRewardedAd(RewardedAdUnitId);
            }
        }

        public void ShowInterstitialAd()
        {
            if (MaxSdk.IsInterstitialReady(InterstitialAdUnitId))
            {
                MaxSdk.ShowInterstitial(InterstitialAdUnitId);
            }
        }

        public void ShowBannerAd()
        {
            MaxSdk.ShowBanner(BannerAdUnitId);
        }

        #endregion

        #region PRIVATE METHODS

        private void LoadRewardedAd()
        {
            MaxSdk.LoadRewardedAd(RewardedAdUnitId);
        }

        private void LoadInterstitialAd()
        {
            MaxSdk.LoadInterstitial(InterstitialAdUnitId);
        }

        private void LoadBannerAd()
        {
            MaxSdk.LoadBanner(BannerAdUnitId);
        }

        private void OnAdRevenuePaidEvent(string arg1, MaxSdkBase.AdInfo arg2)
        {
            AdjustAdRevenue adjustAdRevenue = new AdjustAdRevenue(AdjustConfig.AdjustAdRevenueSourceAppLovinMAX);
            adjustAdRevenue.setRevenue(arg2.Revenue, "USD");
            adjustAdRevenue.setAdRevenueNetwork(arg2.NetworkName);
            adjustAdRevenue.setAdRevenueUnit(arg2.AdUnitIdentifier);
            adjustAdRevenue.setAdRevenuePlacement(arg2.Placement);

            Adjust.trackAdRevenue(adjustAdRevenue);
        }
        
        private void OnRewardedAdReceivedReward(string arg1, MaxSdkBase.Reward arg2, MaxSdkBase.AdInfo arg3)
        {
            _onAdSuccessful?.Invoke();
        }
        
        private void OnRewardedAdDisplayed(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            Debug.Log("Rewarded ad displayed");
            LoadRewardedAd();
        }

        private void OnRewardedAdLoaded(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            Debug.Log("Rewarded ad loaded");
        }

        private void OnInterstitialAdLoaded(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            Debug.Log("Interstitial ad loaded");
        }

        private void OnBannerAdLoaded(string adUnitId, MaxSdkBase.AdInfo adInfo)
        {
            Debug.Log("Banner ad loaded");
        }

        #endregion
    }
}
