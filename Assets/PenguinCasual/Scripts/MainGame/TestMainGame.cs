using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Penguin
{
    public class TestMainGame : MonoBehaviour
    {
        [SerializeField] ScoreSetting _scoreSetting;

        [SerializeField] Character _character;
        [SerializeField] Platform _platform;
        [SerializeField] GameObject _endGamePanel;

        private IScoreCaculator _scoreCaculator;

        private int _lastPassLayer = -1;

        void Awake()
        {
            _character.OnCollideWithPedestal += OnPlayerCollideWithPedestal;
            EventHub.Bind<EventCharacterPassLayer>(OnCharacterPassLayer, true);

            _scoreCaculator = new SimpleScoreCalculator(_scoreSetting);
            _scoreCaculator.OnScoreUpdate += OnScoreUpdate;
            _scoreCaculator.OnComboActive += OnComboActive;
        }

        void OnDestroy()
        {
            EventHub.Unbind<EventCharacterPassLayer>(OnCharacterPassLayer);
        }

        void Update()
        {
            _platform.UpdatePenguinPosition(_character.transform.position);
            _scoreCaculator.Update(Time.deltaTime);
        }

        void OnPlayerCollideWithPedestal(Pedestal pedestal)
        {
            bool shouldReturn = false;
            if (_scoreCaculator.HasActiveCombo)
            {
                _platform.DestroyNextLayer();
                _character.Jump();
                shouldReturn = true;
            }

            _scoreCaculator.OnLandingLayer(pedestal);

            if (shouldReturn)
                return;

            if (pedestal.type == PedestalType.Pedestal_01 ||
                pedestal.type == PedestalType.Pedestal_01_1_Fish ||
                pedestal.type == PedestalType.Pedestal_01_3_Fish)
            {
                _character.Jump();
            }
            else if (pedestal.type == PedestalType.DeadZone_01)
            {
                Debug.Log("Dead by touching deadzone");
                ProcessEndGame();
            }
            else if (pedestal.type == PedestalType.Wall_01)
            {
                Debug.Log("Dead by touching wall");
                ProcessEndGame();
            }
        }

        void ProcessEndGame()
        {
            _character.OnDie();
            _endGamePanel.SetActive(true);
            _platform.UnregisterEvent();
        }

        void OnCharacterPassLayer(EventCharacterPassLayer eventData)
        {
            _scoreCaculator.OnPassingLayer(eventData.layer);
        }

        private void OnComboActive()
        {
            //TODO active character combo effect
        }

        private void OnScoreUpdate(long score)
        {
            EventHub.Emit<EventUpdateScore>(new EventUpdateScore(score));
        }

        public void OnRestartGame()
        {
            SceneManager.LoadScene("PlatformTestScene");
        }
    }
}

