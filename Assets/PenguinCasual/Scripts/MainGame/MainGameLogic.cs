using UnityEngine;

namespace Penguin
{
    public class MainGameLogic : MonoBehaviour
    {
        [SerializeField] GameSetting _gameSetting;
        [SerializeField] CameraFollower _cameraFollower;
        [SerializeField] ScoreSetting _scoreSetting;
        [SerializeField] Character _character;
        [SerializeField] Platform _platform;
        [SerializeField] GameObject _endGamePanel;

        private IScoreCaculator _scoreCaculator;

        private bool _isTimeOut = false;

        void Awake()
        {
            _character.OnCollideWithPedestal += OnPlayerCollideWithPedestal;
            EventHub.Bind<EventCharacterPassLayer>(OnCharacterPassLayer, true);
            EventHub.Bind<EventStartGame>(OnWaitForStartGame);
            EventHub.Bind<EventTimeout>(OnTimeout);

            _scoreCaculator = new SimpleScoreCalculator(_scoreSetting);
            _scoreCaculator.OnScoreUpdate += OnScoreUpdate;
            _scoreCaculator.OnComboActive += OnComboActive;

            _platform.Setup(_gameSetting);
            _cameraFollower.Setup(_gameSetting);
            _character.Setup(_gameSetting);


        }

        void OnDestroy()
        {
            EventHub.Unbind<EventCharacterPassLayer>(OnCharacterPassLayer);
            EventHub.Unbind<EventStartGame>(OnWaitForStartGame);
            EventHub.Unbind<EventTimeout>(OnTimeout);
        }

        private void OnWaitForStartGame(EventStartGame e)
        {
            _platform.CanInteract = true;
        }

        private void Update()
        {
            _character.CustomUpdate();
            _cameraFollower.CustomUpdate();

            _platform.UpdatePenguinPosition(_character.transform.position);
            _scoreCaculator.Update(Time.deltaTime);
        }

        private void OnPlayerCollideWithPedestal(Pedestal pedestal)
        {
            if (_isTimeOut)
            {
                ProcessEndGame(false);
                return;
            }

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
                ProcessEndGame(true);
            }
            else if (pedestal.type == PedestalType.Wall_01)
            {
                Debug.Log("Dead by touching wall");
                ProcessEndGame(true);
            }
        }

        void ProcessEndGame(bool endGameByDead)
        {
            _character.OnDie();
            _platform.UnregisterEvent();
            EventHub.Emit<EventEndGame>(new EventEndGame(endGameByDead));
        }

        void OnCharacterPassLayer(EventCharacterPassLayer eventData)
        {
            _scoreCaculator.OnPassingLayer(eventData.layer);
        }

        void OnTimeout(EventTimeout eventData)
        {
            _isTimeOut = true;
        }

        private void OnComboActive()
        {
            //TODO: active character combo effect
        }

        private void OnScoreUpdate(long score)
        {
            EventHub.Emit<EventUpdateScore>(new EventUpdateScore(score));
        }
    }
}

