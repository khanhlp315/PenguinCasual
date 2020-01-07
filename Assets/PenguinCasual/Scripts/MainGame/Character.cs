using System;
using System.Collections;
using System.Collections.Generic;
using Penguin.Sound;
using UnityEngine;

namespace Penguin
{
    public enum CharacterState
    {
        Normal,
        Dead,
        Powerup
    }

    /// <summary>
    /// Handle logic for in-game main character
    /// </summary>
    public class Character : MonoBehaviour
    {
        // Note: defualt is 0
        static Dictionary<PedestalType, int> _pedestalHitPriority = new Dictionary<PedestalType, int>
        {
            {PedestalType.DeadZone_01, -2},
            {PedestalType.Pedestal_04_Powerup, -1}
        };

        #region [ Fields ]
        public Action<Pedestal> OnCollideWithPedestal;
        public Action<Pedestal> OnStuckInPedestal;
        [SerializeField] private GameObject _model;
        [SerializeField] private BoxCollider _collider;
        [SerializeField] private ParticleSystem _deathParticle;
        [SerializeField] private ParticleSystem _boostParticle;
        [SerializeField] private ParticleSystem _boostTraitParticle;

        private GameSetting _gameSetting;
        RaycastHit[] _rayCastHits = new RaycastHit[5];
		Pedestal[] _collidePedestalHits = new Pedestal[5];
        private float _velocity;
        private LayerMask _defaultLayerMask;

        private CharacterState _state;
        public CharacterState State
        {
            get
            {
                return _state;
            }
            set
            {
                _state = value;
            }
        }

        #endregion

        #region [ Methods ]
        public void Setup(GameSetting gameSetting)
        {
            _gameSetting = gameSetting;
            _defaultLayerMask = LayerMask.GetMask("Default");
            
            Vector3 startPos = transform.position;
            startPos.y = gameSetting.characterStartPosition;
            transform.position = startPos;
        }

        public void CustomUpdate()
        {
            if (State == CharacterState.Dead)
                return;

            _velocity -= _gameSetting.characterGravity * Time.deltaTime;
            _velocity = Mathf.Max(_velocity, _gameSetting.characterMaxDroppingVelocity);
            float moveDistance = _velocity * Time.deltaTime;
            int totalHit = Physics.BoxCastNonAlloc(transform.position + _collider.center, 
                                                    _collider.size / 2, 
                                                    Vector3.down,
                                                    _rayCastHits, 
                                                    Quaternion.identity, 
                                                    moveDistance < 0 ? Mathf.Abs(moveDistance) : 0,
                                                    _defaultLayerMask);
            Pedestal stuckInPedestal = null;

            Vector3 position = transform.position;
            position.y += moveDistance;

            int totalHitPedestal = 0;
            for (int i = 0; i < totalHit; i++)
            {
                Pedestal pedestal = _rayCastHits[i].collider.GetComponent<Pedestal>();
                if (pedestal == null)
                    pedestal = _rayCastHits[i].collider.GetComponentInParent<Pedestal>();
                if (pedestal == null || !pedestal.Active)
                    continue;

				_collidePedestalHits[totalHitPedestal++] = pedestal;
            }

            Array.Sort(_collidePedestalHits, 0, totalHit, Comparer<Pedestal>.Create((p1, p2) => {
                return GetPedestalHitPriority(p1) > GetPedestalHitPriority(p2) ? 1 : -1;
            }));

			for (int i = 0; i < totalHitPedestal; i++)
			{
				Pedestal pedestal = _collidePedestalHits[i];
				OnCollideWithPedestal?.Invoke(pedestal);

				if (!pedestal.CanGoThrough)
				{
					if (_rayCastHits[i].distance > 0 && position.y < _rayCastHits[i].point.y)
					{
						position.y = _rayCastHits[i].point.y;
					}
					else if (_rayCastHits[i].distance <= 0)
					{
						stuckInPedestal = pedestal;
					}
				}

                // Should handle only one
                break;
            }

            transform.position = position;

            if (stuckInPedestal != null)
            {
                OnStuckInPedestal?.Invoke(stuckInPedestal);
            }
        }

        public void Jump()
        {
            _velocity = _gameSetting.characterJumpVelocity;
            Sound2DManager.Instance.PlaySound(SoundConfig.Jump);
        }

        public void Revive()
        {
            State = CharacterState.Normal;

            _deathParticle.gameObject.SetActive(false);
        }

        public void OnDie()
        {
            State = CharacterState.Dead;
        }

        public void SetDieEffect(bool isActive)
        {
            if (isActive)
            {
                Sound2DManager.Instance.PlaySound(SoundConfig.PenguinHitAndDie);
                _deathParticle.gameObject.SetActive(true);
                _deathParticle.Play();
            }
            else
            {
                _deathParticle.gameObject.SetActive(false);
                _deathParticle.Stop();
            }
        }

        public void ActivePowerup()
        {
            _velocity = _gameSetting.characterMaxDroppingVelocity;
        }

        public void SetBoostEffect(bool isActive)
        {
            if (isActive)
            {
                _boostParticle.gameObject.SetActive(true);
                _boostTraitParticle.gameObject.SetActive(true);

                _boostParticle.Play(true);
                _boostTraitParticle.Play(true);
            }
            else
            {
                _boostParticle.gameObject.SetActive(false);
                _boostTraitParticle.gameObject.SetActive(false);

                _boostParticle.Stop(true);
                _boostTraitParticle.Stop(true);
            }
        }

        public void SetModel(GameObject characterModel)
        {
            if (_model != null)
            {
                GameObject.Destroy(_model);
            }

            _model = GameObject.Instantiate(characterModel, transform);
            _model.transform.localPosition = Vector3.zero;
            _model.transform.localRotation = Quaternion.identity;
        }

        private int GetPedestalHitPriority(Pedestal pedestal)
        {
            if (_pedestalHitPriority.ContainsKey(pedestal.type))
                return _pedestalHitPriority[pedestal.type];
            
            return 0;
        }

        #endregion
    }
}