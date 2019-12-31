﻿using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Penguin
{
    public class EndGamePanel : MonoBehaviour
    {
        [SerializeField]
        private ShadowText _labelScore;

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

        public void ShowAsNormal()
        {
            _isShowAsNormal = true;

            _buttonWatchAd.SetActive(false);
            _panelRestart.SetActive(true);
            _panelWatchAd.SetActive(false);
            _labelWatchAdDesc.gameObject.SetActive(false);
        }

        public void ShowWithWatchAd()
        {
            _isShowAsNormal = false;

            _buttonWatchAd.SetActive(true);
            _panelRestart.SetActive(false);
            _panelWatchAd.SetActive(true);
            _labelWatchAdDesc.gameObject.SetActive(true);

            _watchAdDuration.DOFillAmount(0, 5)
                .OnComplete(ShowAsNormal);
        }

        public void OnButtonWatchAd()
        {
            //TODO
            if (_isShowAsNormal)
                return;
        }

        public void OnRevived()
        {
            if (_isShowAsNormal)
                return;
            
            EventHub.Emit(new EventRevive());
            DOTween.Kill(_watchAdDuration);
        }

        public void OnRestart()
        {
            SceneManager.LoadScene("PlatformTestScene");
        }

        public void OnHome()
        {
            SceneManager.LoadScene("HomeScene");
        }
    }
}