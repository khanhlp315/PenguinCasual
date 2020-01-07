using System;
using System.Collections;
using System.Collections.Generic;
using Penguin;
using Penguin.Dialogues;
using Penguin.Network;
using Penguin.Sound;
using Penguin.UI;
using pingak9;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Penguin.Scenes
{
    public class CharacterScene : MonoBehaviour
{
    [SerializeField]
    private SkinSetting _skinSetting;
    
    [SerializeField]
    private BackgroundSetting _backgroundSetting;

    [SerializeField]
    private CharacterItemController _characterItem;

    [SerializeField]
    private RectTransform _characterList;
    
    [SerializeField]
    private BackgroundItemController _backgroundItem;

    [SerializeField]
    private RectTransform _backgroundList;

    [SerializeField] private GameObject _characterLayer;
    [SerializeField] private GameObject _backgroundLayer;

    [SerializeField] private SwitchButtonGroup _switchButtonGroup;

    [SerializeField] private GameObject _loadingLayer;
    
    private List<CharacterItemController> _characterItemControllers;

    [SerializeField]
    private CharacterInfoPanel _characterInfoPanel;

    [SerializeField] private MissionPanel _missionPanel;

    private void Start()
    {
        _characterLayer.SetActive(true);
        _backgroundLayer.SetActive(false);
        _characterInfoPanel.OnCharacterSelect += OnSkinTapped;
        _switchButtonGroup.OnButtonSelected += (buttonName)=>
        {
            if (buttonName == "Character")
            {
                _characterLayer.SetActive(true);
                _backgroundLayer.SetActive(false);
            }
            else
            {
                _characterLayer.SetActive(false);
                _backgroundLayer.SetActive(true);
            }
        };
        _characterItemControllers = new List<CharacterItemController>();
        
        CallToNetwork();
        _backgroundSetting.backgroundDataList.ForEach((background) =>
        {
            var backgroundItem = Instantiate(_backgroundItem, _backgroundList, false);
            backgroundItem.Avatar = background.skinAvatar;
            backgroundItem.IsLocked = !background.IsUnlocked();
        });
    }

    private void CallToNetwork()
    {
        NetworkCaller.Instance.GetAllSkins((skins, unlocks) =>
        {
            var currentSkin = NetworkCaller.Instance.PlayerData.SkinId;
            foreach (var skinData in skins)
            {
                var skin = _skinSetting.GetSkinById(skinData.Id);
                var skinItem = Instantiate(_characterItem, _characterList, false);
                skinItem.Avatar = skin.skinAvatar;
                skinItem.IsSelected = skin.id == currentSkin;
                bool isLocked;
                skinItem.Id = skin.id;
                if (skinData.UnlockId <= 0)
                {
                    isLocked = false;
                }
                else
                {
                    var unlock = unlocks.Find(x => x.Id == skinData.UnlockId);
                    isLocked = !unlock.IsUnlocked();
                }
                

                skinItem.IsLocked = isLocked;

                skinItem.OnSelected += () =>
                {
                    if (!skinItem.IsLocked)
                    {
                        _characterInfoPanel.SkinData = skinData;
                        _characterInfoPanel.SetAvatar(skin.skinAvatar);
                        _characterInfoPanel.Show();
                    }
                    else
                    {
                        var unlock = unlocks.Find(x => x.Id == skinData.UnlockId);
                        _missionPanel.SetMission(unlock.Mission);
                        _missionPanel.Show();
                    }
                };
                
                _characterItemControllers.Add(skinItem);
            }
            _loadingLayer.SetActive(false);
        }, () =>
        {
            NativeDialogManager.Instance.ShowConnectionErrorDialog(CallToNetwork, () =>
            {
                SceneManager.LoadScene("HomeScene"); 
            });
        });
    }


    public void GoToHomeScene()
    {
        SceneManager.LoadScene("HomeScene");
    }

    public void GoToSettingsScene()
    {
        SceneManager.LoadScene("SettingsScene");

    }

    private void OnSkinTapped(int skinId)
    {
        NetworkCaller.Instance.SelectSkin(skinId, () =>
        {
            var updatedSkinId = NetworkCaller.Instance.PlayerData.SkinId;
            _characterItemControllers.ForEach(x =>
            {
                x.IsSelected = x.Id == updatedSkinId;
                x.Reload();
            });
            _characterInfoPanel.Hide();
        }, () =>
        {
            NativeDialogManager.Instance.ShowConnectionErrorDialog(() =>
            {
                OnSkinTapped(skinId);
            }, () =>
            {
                
            });
        });
    }
}

}