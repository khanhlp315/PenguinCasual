using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Penguin.Ads;
using Penguin.Utilities;
using PenguinCasual.Scripts.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Penguin
{
    public class EndGamePanel : MonoBehaviour, IAdvertisementDelegate
    {
        [SerializeField]
        private ShadowTextUGUI _labelScore;

        [SerializeField]
        private GameObject _panelWatchAd;
        [SerializeField]
        private GameObject _panelRestart;
        [SerializeField]
        private GameObject _buttonWatchAd;
        [SerializeField]
        private Text _labelWatchAdDesc;
        [SerializeField]
        private Image _watchAdDuration;

        private bool _isShowAsNormal;
        private bool _isGameEndedByDie = false;

        private void Start()
        {
            Advertiser.AdvertisementSystem.SetDelegate(this);
        }

        public void SetIsGameEndedByDie(bool isGameEndedByDie)
        {
            _isGameEndedByDie = isGameEndedByDie;
        }

        public void SetScore(long score)
        {
            _labelScore.text = ScoreUtil.FormatScore(score) + "<size=18>匹</size>";
        }

        public void ShowAsNormal()
        {
            Advertiser.AdvertisementSystem.ShowEndGameBanner();
            _isShowAsNormal = true;

            _buttonWatchAd.SetActive(false);
            _panelRestart.SetActive(true);
            _panelWatchAd.SetActive(false);
            _labelWatchAdDesc.gameObject.SetActive(false);
        }

        public void ShowWithWatchAd()
        {
            Advertiser.AdvertisementSystem.ShowEndGameBanner();
            _isShowAsNormal = false;

            _buttonWatchAd.SetActive(true);
            _panelRestart.SetActive(false);
            _panelWatchAd.SetActive(true);
            _labelWatchAdDesc.gameObject.SetActive(true);
            _labelWatchAdDesc.text =
                $"今日はあと<b><color=#F045C2><size=20>{PlayerPrefsHelper.WatchAdsTimesToday()}</size><size=14>/{PlayerPrefsHelper.WatchAdsTimesPerDay}</size><size=10>かい</size></color></b> 使えるよ";

            _watchAdDuration.DOFillAmount(0, 5)
                .OnComplete(ShowAsNormal);
        }

        public void OnButtonWatchAd()
        {
            if (_isShowAsNormal)
                return;
            if (_isGameEndedByDie)
            {
                Advertiser.AdvertisementSystem.ShowDieRewardAds("die");
            }
            else
            {
                Advertiser.AdvertisementSystem.ShowTimeUpRewardAds("time_up");
            }
        }

        public void OnRevived()
        {
            PlayerPrefsHelper.CountWatchAdsTimes();
            Advertiser.AdvertisementSystem.HideEndGameBanner();
            EventHub.Emit(new EventRevive());
            DOTween.Kill(_watchAdDuration);
        }

        public void OnRestart()
        {
            Advertiser.AdvertisementSystem.HideEndGameBanner();
            EventHub.ClearAll();
            SceneManager.LoadScene("PlatformTestScene");
        }

        public void OnHome()
        {
            SceneManager.LoadScene("HomeScene");
        }

        public void OnNormalBannerAdLoaded()
        {
        }

        public void OnNormalBannerAdDismiss()
        {
        }

        public void OnNormalBannerAdShow()
        {
        }

        public void OnNormalBannerAdFailedToLoad()
        {
        }

        public void OnEndGameBannerAdLoaded()
        {
        }

        public void OnEndGameBannerAdDismiss()
        {
        }

        public void OnEndGameBannerAdShow()
        {
        }

        public void OnEndGameBannerAdFailedToLoad()
        {
        }

        public void OnDieRewardLoaded()
        {
        }

        public void OnDieRewardShow()
        {
        }

        public void OnDieRewardSkipped()
        {
            DOTween.Kill(_watchAdDuration);
            ShowAsNormal();
        }

        public void OnDieRewardDismiss()
        {
        }

        public void OnDieRewardDidReward()
        {
            OnRevived();
        }

        public void OnDieRewardFailedToLoad()
        {
        }

        public void OnTimeUpRewardLoaded()
        {
        }

        public void OnTimeUpRewardShow()
        {
        }

        public void OnTimeUpRewardSkipped()
        {
            DOTween.Kill(_watchAdDuration);
            ShowAsNormal();
        }

        public void OnTimeUpRewardDismiss()
        {
            
        }

        public void OnTimeUpRewardDidReward()
        {
            OnRevived();
        }

        public void OnTimeUpRewardFailedToLoad()
        {
        }
    }
}