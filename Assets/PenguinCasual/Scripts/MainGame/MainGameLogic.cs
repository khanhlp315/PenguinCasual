using UnityEngine;

namespace Penguin
{
    public class MainGameLogic : MonoBehaviour
    {
        [SerializeField] private GameSetting _gameSetting;
        [SerializeField] private CameraFollower _cameraFollower;
        [SerializeField] private ScoreSetting _scoreSetting;
        [SerializeField] private Character _character;
        [SerializeField] private Platform _platform;
        [SerializeField] private GameObject _endGamePanel;

        private IScoreCaculator _scoreCaculator;

        private bool _isTimeOut = false;
        private int _remainPowerupBreakFloor = 0;
        private int _floorCombo = 0;

        void Awake()
        {
            _character.OnCollideWithPedestal += OnCharacterCollideWithPedestal;
            EventHub.Bind<EventStartGame>(OnWaitForStartGame);
            EventHub.Bind<EventTimeout>(OnTimeout);

            _scoreCaculator = new SimpleScoreCalculator(_scoreSetting);
            _scoreCaculator.OnScoreUpdate += OnScoreUpdate;
            _scoreCaculator.OnComboActive += OnComboActive;

            _platform.Setup(_gameSetting);
            _platform.OnCharacterPassedThoughPedestalLayer += OnCharacterPassLayer;
            _cameraFollower.Setup(_gameSetting);
            _character.Setup(_gameSetting);
        }

        void OnDestroy()
        {
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

            if (_isTimeOut)
            {
                ProcessEndGame(false);
                return;
            }
        }

        private void OnCharacterCollideWithPedestal(Pedestal pedestal)
        {
            if (HasPowerUp())
            {
                _remainPowerupBreakFloor -= 1;
                _platform.ForceDestroyNextLayer();

                if (_remainPowerupBreakFloor == 0)
                    _character.Jump();
            }
            else if (HasCombo())
            {
                // Special case, should give player powerup.
                if (pedestal.type == PedestalType.Pedestal_04_Powerup)
                {
                    _character.ActivePowerup();
                    _remainPowerupBreakFloor = _gameSetting.powerUpBreakFloors;
                }
                else
                {
                    _character.Jump();
                    _platform.ForceDestroyNextLayer();
                }
            }
            else
            {
                if (pedestal.type == PedestalType.Pedestal_01 ||
                pedestal.type == PedestalType.Pedestal_01_1_Fish ||
                pedestal.type == PedestalType.Pedestal_01_3_Fish)
                {
                    _character.Jump();
                }
                else if (pedestal.type == PedestalType.Pedestal_04_Powerup)
                {
                    _character.ActivePowerup();
                    _remainPowerupBreakFloor = _gameSetting.powerUpBreakFloors;
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

            _floorCombo = 0;
            _scoreCaculator.OnLandingLayer(pedestal);
        }

        void ProcessEndGame(bool endGameByDead)
        {
            _character.OnDie();
            _platform.UnregisterEvent();
            EventHub.Emit<EventEndGame>(new EventEndGame(endGameByDead));
        }

        void OnCharacterPassLayer(PedestalLayer layer)
        {
            if (!HasPowerUp() && !layer.hasDestroyed)
            {
                _floorCombo += 1;
            }

            var eventPlayerPassLayer = new EventCharacterPassLayer(layer);
            eventPlayerPassLayer.hasCombo = HasCombo();
            eventPlayerPassLayer.hasPowerup = HasPowerUp();
            eventPlayerPassLayer.hasLayerDestroyed = layer.hasDestroyed;
            EventHub.Emit(eventPlayerPassLayer);

            if (!layer.hasDestroyed)
                _platform.DestroyLayer(layer);

            _scoreCaculator.OnPassingLayer(layer);
        }

        void OnTimeout(EventTimeout eventData)
        {
            _isTimeOut = true;
        }

        private void OnComboActive()
        {
            //TODO: active character combo effect
        }

        private bool HasPowerUp()
        {
            return _remainPowerupBreakFloor > 0;
        }

        private bool HasCombo()
        {
            return _floorCombo >= _gameSetting.comboToBreakFloor;
        }

        private void OnScoreUpdate(long score)
        {
            EventHub.Emit<EventUpdateScore>(new EventUpdateScore(score));
        }
    }
}

