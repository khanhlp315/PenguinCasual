using PenguinCasual.Scripts.Utilities;
using UnityEngine;

namespace Penguin
{
    enum GameState
    {
        Waiting,
        Playing,
        EndByTimeout,
        EndByDead
    }

    public class MainGameLogic : MonoBehaviour
    {
        [SerializeField] private GameSetting _gameSetting;
        [SerializeField] private CameraFollower _cameraFollower;
        [SerializeField] private ScoreSetting _scoreSetting;
        [SerializeField] private Character _character;
        [SerializeField] private Platform _platform;

        private IScoreCaculator _scoreCaculator;

        private float _roundTime;
        private GameState _gameState;
        private int _remainPowerupBreakFloor = 0;
        private int _floorCombo = 0;

        void Awake()
        {
            EventHub.Bind<EventStartGame>(OnStartGame);
            EventHub.Bind<EventRevive>(OnRevive);

            _scoreCaculator = new SimpleScoreCalculator(_scoreSetting);
            _scoreCaculator.OnScoreUpdate += OnScoreUpdate;


            _roundTime = _gameSetting.roundDuration;
            _gameState = GameState.Waiting;

            _platform.Setup(_gameSetting);
            _platform.OnCharacterPassedThoughPedestalLayer += OnCharacterPassThoughLayer;

            _cameraFollower.Setup(_gameSetting);
            
            _character.Setup(_gameSetting);
            _character.OnCollideWithPedestal += OnCharacterCollideWithPedestal;
            _character.OnStuckInPedestal += OnCharacterStuckInPedestal;
        }

        void OnDestroy()
        {
            EventHub.Unbind<EventStartGame>(OnStartGame);
        }

        private void OnStartGame(EventStartGame e)
        {
            _platform.CanInteract = true;
            _gameState = GameState.Playing;
        }

        private void Update()
        {
            _character.CustomUpdate();
            _cameraFollower.CustomUpdate();

            _platform.UpdatePenguinPosition(_character.transform.position);
            _scoreCaculator.Update(Time.deltaTime);

            if (_gameState == GameState.Playing)
            {
                _roundTime -= Time.deltaTime;
                if (_roundTime <= 0)
                {
                    ProcessEndGame(false);
                }

                EventHub.Emit(new EventGameTimeUpdate() {
                    time = _roundTime
                });
            }
        }

        private void OnRevive(EventRevive e)
        {
            if (_gameState == GameState.EndByDead)
            {
                _platform.ForceDestroyNextLayer(true);
            }

            _character.Revive();
            _character.Jump();

            _gameState = GameState.Playing;
            _platform.CanInteract = true;
            _roundTime = _gameSetting.reviveRoundDuration;
            EventHub.Emit(new EventGameTimeUpdate()
            {
                time = _roundTime
            });
        }

        private void OnCharacterCollideWithPedestal(Pedestal pedestal)
        {
            if (pedestal.type == PedestalType.Squid_01)
            {
                _roundTime += _gameSetting.squidBonusDuration;
                pedestal.Destroy();
                EventHub.Emit(new EventGameTimeUpdate() {
                    time = _roundTime
                });

                EventHub.Emit(new EventGetBonusTime() {
                    bonusTime = _gameSetting.squidBonusDuration,
                    squid = pedestal
                });
                
                return;
            }
            else if (HasPowerUp())
            {
                UsePowerUp(pedestal);
                _floorCombo = 0;
                return;
            }
            else if (HasCombo())
            {
                Debug.LogError("Has combo");
                // Special case, should give player powerup.
                if (pedestal.type == PedestalType.Pedestal_04_Powerup)
                {
                    _character.ActivePowerup();
                    _character.SetBoostEffect(true);
                    _remainPowerupBreakFloor = _gameSetting.powerUpBreakFloors;
                }
                else
                {
                    _character.Jump();
                    _platform.ForceDestroyNextLayer();
                }

                _floorCombo += 1;
                _scoreCaculator.OnLandingLayer(HasCombo(), _floorCombo, pedestal);
                _scoreCaculator.PreventUpdateScoreOnNextLanding();
                _floorCombo = 0;
                return;
            }
            else
            {
                if (pedestal.type == PedestalType.Pedestal_01 ||
                pedestal.type == PedestalType.Pedestal_01_1_Fish ||
                pedestal.type == PedestalType.Pedestal_01_3_Fish)
                {
                    _character.Jump();
                    EventHub.Emit(new EventReadyToStartGame());
                }
                else if (pedestal.type == PedestalType.Pedestal_04_Powerup)
                {
                    _character.ActivePowerup();
                    _character.SetBoostEffect(true);
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

            _scoreCaculator.OnLandingLayer(HasCombo(), _floorCombo, pedestal);
            _floorCombo = 0;
        }

        void OnCharacterStuckInPedestal(Pedestal pedestal)
        {
            Debug.Log($"Detect character stuck in pedestal: lastAngle {_platform.LastAngle} - angle {_platform.Angle}");
            
            float diffAngle = _platform.Angle - _platform.LastAngle;

            if (pedestal.type == PedestalType.Wall_01)
            {
                if (diffAngle > 0)
                {
                    float diff = pedestal.transform.eulerAngles.y + 14;
                    _platform.SetAngle(_platform.Angle - diff, true);
                }
                else
                {
                    float diff = pedestal.transform.eulerAngles.y - 14;
                    _platform.SetAngle(_platform.Angle - diff, true);
                }
            }
            else if (pedestal.type == PedestalType.DeadZone_01)
            {
                if (diffAngle > 0)
                {
                    float diff = pedestal.transform.eulerAngles.y + 60;
                    _platform.SetAngle(_platform.Angle - diff, true);
                }
                else
                {
                    float diff = pedestal.transform.eulerAngles.y - 14;
                    _platform.SetAngle(_platform.Angle - diff, true);
                }
            }
        }

        void ProcessEndGame(bool endGameByDead)
        {
            _gameState = endGameByDead ? GameState.EndByDead : GameState.EndByTimeout;
            _character.OnDie();
            _character.SetDieEffect(endGameByDead);
            _platform.CanInteract = false;
            EventHub.Emit<EventEndGame>(new EventEndGame(endGameByDead));
        }

        void OnCharacterPassThoughLayer(PedestalLayer layer)
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

            // Special case, character has powerup and pass through the layer
            if (!layer.hasDestroyed && HasPowerUp())
                UsePowerUp(null);

            if (!layer.hasDestroyed)
                _platform.DestroyLayer(layer, HasPowerUp());

            _scoreCaculator.OnPassingLayer(HasPowerUp(), layer);
        }

        private bool HasPowerUp()
        {
            return _remainPowerupBreakFloor > 0;
        }

        private bool HasCombo()
        {
            return _floorCombo >= _gameSetting.comboToBreakFloor;
        }

        private void UsePowerUp(Pedestal collidePedestal)
        {
            _remainPowerupBreakFloor -= 1;
            _platform.ForceDestroyNextLayer(true);

            if (_remainPowerupBreakFloor == 0)
            {
                if (collidePedestal != null)
                {
                    _character.Jump();
                    _scoreCaculator.OnLandingLayer(HasCombo(), _floorCombo, collidePedestal);
                    _scoreCaculator.PreventUpdateScoreOnNextLanding();
                }
                    
                _character.SetBoostEffect(false);
            }
        }

        private void OnScoreUpdate(long score, long increase)
        {
            EventHub.Emit<EventUpdateScore>(new EventUpdateScore(score, increase));
            PlayerPrefsHelper.AddToTotalScore((int)increase);
        }
    }
}

