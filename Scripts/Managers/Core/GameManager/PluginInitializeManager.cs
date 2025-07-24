using System;
using System.Collections;
using _Game.Scripts.RemoteConfig.Base;
using _Game.Scripts.RemoteConfig.ScriptableObjectScripts;
using com.adjust.sdk;
using Facebook.Unity;
using Firebase;
using Firebase.Analytics;
using Firebase.Crashlytics;
using Firebase.Extensions;
using GameAnalyticsSDK;
using UnityEngine;

namespace _Game.Scripts.Managers.Core.GameManager
{
    public class PluginInitializeManager : MonoBehaviour
    {
        #region Private Variables

        private const string AppLovinSdk =
            "09mBPe6fn7Tg_xo6p4-shNiAaXlBrtK4zAFXmPKNwdK3df-td8R7o5CgUWUpH3LQb2Mxxmp8AKngmcXgROmQJV";

        private const string AdjustAppToken = "flo81ntoukn4";

        private bool _isAttAuthorized;
        private bool _isFirebaseInitialized;
        private bool _isFacebookInitialized;
        private bool _isFacebookActivated;
        public FirebaseApp App;
        private DependencyStatus _dependencyStatus = DependencyStatus.UnavailableOther;

        private RemoteConfigManager _remoteConfigManager;
        public GlobalVariablesHolderSo globalVariables;

        #endregion

        #region Unity Methods

        private static void OnHideUnity(bool isUnityShown)
        {
            Time.timeScale = !isUnityShown ? 0 : 1;
        }

        private void InitCallback()
        {
            if (FB.IsInitialized)
            {
                TryInitializeFacebook();
                Debug.Log("Facebook SDK is initialized.");
            }
            else
            {
                Debug.LogError("Failed to Initialize the Facebook SDK");
            }
        }

        private void Start()
        {
            StartCoroutine(WaitForFirebaseInitialization());

            InitializeAppLovinMax();

            FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
            {
                _dependencyStatus = task.Result;
                if (_dependencyStatus == DependencyStatus.Available)
                {
                    App = FirebaseApp.DefaultInstance;
                    InitializeFirebase();
                }
                else
                {
                    Debug.LogError("Could not resolve all Firebase dependencies: " + _dependencyStatus);
                }
            });

        }

        #endregion

        #region Private Methods

        private void InitializeAppLovinMax()
        {
            MaxSdkCallbacks.OnSdkInitializedEvent += HandleAppLovinMaxInitialization;
            MaxSdk.SetSdkKey(AppLovinSdk);
            MaxSdk.SetIsAgeRestrictedUser(false);
            MaxSdk.InitializeSdk();
        }

        private void HandleAppLovinMaxInitialization(MaxSdkBase.SdkConfiguration sdkConfiguration)
        {
            // Detach the event to prevent multiple calls
            MaxSdkCallbacks.OnSdkInitializedEvent -= HandleAppLovinMaxInitialization;

            if (sdkConfiguration.AppTrackingStatus == MaxSdkBase.AppTrackingStatus.Authorized)
            {
                _isAttAuthorized = true;
                Debug.Log("ATT Authorization Granted.");
            }
            else
            {
                _isAttAuthorized = false;
                Debug.Log("ATT Authorization Denied.");
            }

            TryInitializeFacebook();
            InitializeAdjustSDK();
            //MaxSdk.ShowMediationDebugger();
        }

        private void InitializeAdjustSDK()
        {
            var appToken = AdjustAppToken;
            var environment = AdjustEnvironment.Production;

            var config = new AdjustConfig(appToken, environment);
            Adjust.start(config);
        }

        private void InitializeFirebase()
        {
            try
            {
                FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
                _isFirebaseInitialized = true;
                Debug.Log("Firebase - Enabling data collection.");

                InitializeCrashlytics();
                InitializeRemoteConfig();

            }
            catch (Exception e)
            {
                Debug.LogError("Failed to initialize Firebase: " + e.Message);
                
            }
        }

        private void TryInitializeFacebook()
        {
            if (!FB.IsInitialized)
            {
                FB.Init(InitCallback, OnHideUnity);
            }
            try
            {
                // ATT izni kontrolü yapılıyor
                if (_isAttAuthorized)
                {
                    Debug.Log("ATT Authorization Granted. Setting advertiser ID collection enabled.");
                    FB.Mobile.SetAdvertiserIDCollectionEnabled(true);
                    FB.Mobile.SetAdvertiserTrackingEnabled(true);
                    MaxSdk.SetHasUserConsent(true);

                    var eventName = "advertiser_id_collected";
                    FB.LogAppEvent(eventName);
                }
                else
                {
                    Debug.Log("ATT Authorization Denied. Initializing Facebook.");
                }

                ActivateFacebookApp();
                ActivateGameAnalytics();
            }
            catch (Exception e)
            {
                Debug.LogError("Failed to set advertiser ID collection: " + e.Message);
            }
        }


        private void ActivateFacebookApp()
        {
            if (!_isFacebookActivated)
            {
                FB.ActivateApp();
                _isFacebookActivated = true;
                Debug.Log("Facebook app activated.");
            }
        }

        private void ActivateGameAnalytics()
        {
            GameAnalytics.Initialize();
        }

        private void InitializeRemoteConfig()
        {
            _remoteConfigManager = new RemoteConfigManager();
            _remoteConfigManager.Init(globalVariables);
            Debug.Log("Remote Config initialized.");
        }

        private void InitializeCrashlytics()
        {
            Crashlytics.ReportUncaughtExceptionsAsFatal = true;
            Debug.Log("Crashlytics initialized.");
        }
        
        private IEnumerator WaitForFirebaseInitialization()
        {
            yield return new WaitUntil(() => _isFirebaseInitialized);
            EventManager.AnalyticsEvents.AnalyticsInitialized?.Invoke();
        }

        #endregion
    }
}