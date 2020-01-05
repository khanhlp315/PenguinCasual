using System;
using System.Collections;
using System.Collections.Generic;
using Penguin;
using Penguin.Network;
using Penguin.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    
    private List<CharacterItemController> _characterItemControllers;

    private void Start()
    {
        _characterLayer.SetActive(true);
        _backgroundLayer.SetActive(false);
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
                skinItem.OnSelected += OnSkinSelected;
                
                _characterItemControllers.Add(skinItem);
            }
        }, () =>
        {
            
        });
        
        _backgroundSetting.backgroundDataList.ForEach((background) =>
        {
            var backgroundItem = Instantiate(_backgroundItem, _backgroundList, false);
            backgroundItem.Avatar = background.skinAvatar;
            backgroundItem.IsLocked = !background.IsUnlocked();
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

    public void OnSkinSelected(int skinId)
    {
        NetworkCaller.Instance.SelectSkin(skinId, () =>
        {
            var updatedSkinId = NetworkCaller.Instance.PlayerData.SkinId;
            _characterItemControllers.ForEach(x =>
            {
                x.IsSelected = x.Id == updatedSkinId;
                x.Reload();
            });
        }, () =>
        {
            
        });
    }
}
