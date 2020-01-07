using System.Collections.Generic;
using Penguin.Utilities;
using UnityEngine;

namespace Penguin.Sound
{
    /// <summary>
    /// Internal class for handling load and playing sounds.
    /// </summary>
    class Sound2DManager : MonoSingleton<Sound2DManager, SoundConfig>
    {
        
        #region [ Constant ]

        /// <summary>
        /// PlayerPref save key for Mute sound.
        /// </summary>
        private const string KEY_MUTE_SOUND = "MuteSound";
        /// <summary>
        /// PlayerPref save key for Mute BGM.
        /// </summary>
        private const string KEY_MUTE_BGM = "MuteBg";

        #endregion

        #region [ Fields ]

        /// <summary>
        /// The sound object pools.
        /// </summary>
        private List<AudioSource> _soundObjectPools = new List<AudioSource>(10);

        /// <summary>
        /// Check if sound is muted or not
        /// </summary>
        private bool _isMuteSound = false;

        /// <summary>
        /// Check if bgm is muted or not
        /// </summary>
        private bool _isMuteBGM = false;
        
        #endregion

        #region [ Public Properties ]

        /// <summary>
        /// Gets a value indicating whether this <see cref="T:Assets.Scripts.Sound.Sound2DManager"/> is mute sound.
        /// </summary>
        /// <value><c>true</c> if is mute sound; otherwise, <c>false</c>.</value>
        public bool IsMuteSound => _isMuteSound;

        /// <summary>
        /// Gets a value indicating whether this <see cref="T:Assets.Scripts.Sound.Sound2DManager"/> is mute bgm.
        /// </summary>
        /// <value><c>true</c> if is mute bgm; otherwise, <c>false</c>.</value>
        public bool IsMuteBGM => _isMuteBGM;

        #endregion

        #region [ Public Methods ]
        
        public override void Initialize()
        {
            _isMuteSound = PlayerPrefs.GetInt(KEY_MUTE_SOUND, 0) == 1;
            _isMuteBGM = PlayerPrefs.GetInt(KEY_MUTE_BGM, 0) == 1;
            _instance.gameObject.AddComponent<Sound2DManager>();
            OnInitializeDone?.Invoke();
        }

        /// <summary>
        /// Set mute/un-mute for sound.
        /// </summary>
        /// <param name="isMute">If set to <c>true</c> is mute.</param>
        public void SetMuteSound(bool isMute)
        {
            _isMuteSound = isMute;
            PlayerPrefs.SetInt(KEY_MUTE_SOUND, _isMuteSound ? 1 : 0);
            PlayerPrefs.Save();
        }

        /// <summary>
        /// Set mute/un-mute for BGM.
        /// </summary>
        /// <param name="isMute">If set to <c>true</c> is mute.</param>
        public void SetMuteBGM(bool isMute)
        {
            _isMuteBGM = isMute;
            PlayerPrefs.SetInt(KEY_MUTE_BGM, _isMuteBGM ? 1 : 0);
            PlayerPrefs.Save();

            foreach (var item in _soundObjectPools)
            {
                if (item.gameObject.activeInHierarchy)
                {
                    if (isMute)
                        item.Pause();
                    else
                        item.UnPause();
                }
            }
        }

        /// <summary>
        /// Play BGM
        /// </summary>
        /// <param name="sound">Sound Name/Path</param>
        public void PlayBgm()
        {
            var sound = _config.BGM;
            var playingAudio = GetPlayingAudio(sound);
            if (playingAudio != null)
            {
                if (!playingAudio.isPlaying)
                {
                    playingAudio.UnPause();
                }
                return;
            }
            var unuseAudio = GetUnuseAudio();
            if (unuseAudio != null)
            {
                var clip = Resources.Load<AudioClip>(sound);
                unuseAudio.clip = clip;
                unuseAudio.gameObject.name = sound;
                unuseAudio.loop = true;
                unuseAudio.Play();
                if (_isMuteBGM)
                {
                    unuseAudio.Pause();
                    ////Debug.Log(string.Format("SoundManager:{0}:message = \"Cant play {1} since BGM had been muted\"", "PlayBgm", sound));
                    return;
                }
            }
        }

        /// <summary>
        /// Stop BGM
        /// </summary>
        /// <param name="sound">Sound Name/Path</param>
        public void StopBgm()
        {
            var sound = _config.BGM;
            var playingAudio = GetPlayingAudio(sound);
            if (playingAudio != null)
            {
                playingAudio.Stop();
            }
        }

        /// <summary>
        /// Pause BGM
        /// </summary>
        /// <param name="sound">Sound Name/Path</param>
        private void PauseBgm()
        {
            var sound = _config.BGM;
            var playingAudio = GetPlayingAudio(sound);
            if (playingAudio != null)
            {
                playingAudio.Pause();
            }
        }

        /// <summary>
        /// Resume BGM
        /// </summary>
        /// <param name="sound">Sound Name/Path</param>
        private void ResumeBgm()
        {
            if (_isMuteBGM)
                return;
            var sound = _config.BGM;
            var playingAudio = GetPlayingAudio(sound);
            if (playingAudio != null)
            {
                playingAudio.UnPause();
            }
        }

        public void PlayJumpSound()
        {
            PlaySound(_config.Jump);
        }

        public void PlayPenguinHitAndDieSound()
        {
            PlaySound(_config.PenguinHitAndDie);
        }

        public void PlayBreakFloorSound()
        {
            PlaySound(_config.BreakFloor);
        }
        
        public void PlayFishMoveEndGameSound()
        {
            PlaySound(_config.FishMoveEndGame);
        }

        /// <summary>
        /// Play effect sound
        /// </summary>
        /// <param name="sound">Sound Name/Path</param>
        private void PlaySound(string sound)
        {
            if (_isMuteSound)
            {
                ////Debug.Log(string.Format("SoundManager:{0}:message = \"Cant play {1} since sound had been muted\"", "PlaySound", sound));
                return;
            }

            var audioSource = transform.GetComponent<AudioSource>();
            if (audioSource == null)
            {
                audioSource = transform.gameObject.AddComponent<AudioSource>();
            }
            var clip = Resources.Load<AudioClip>(sound);

            if (clip == null)
            {
                ////Debug.LogError("Clip name " + sound + " doesnt exists");
            }

            //audio.clip = clip;
            //audio.loop = false;
            audioSource.playOnAwake = false;
            audioSource.PlayOneShot(clip);
        }

        private void PlaySoundLoop(string sound)
		{
			if (_isMuteSound || string.IsNullOrEmpty(sound))
			{
				return;
			}

			var unuseAudio = GetUnuseAudio();
			var clip = Resources.Load<AudioClip>(sound);
            unuseAudio.gameObject.name = sound;
            unuseAudio.playOnAwake = false;
            unuseAudio.clip = clip;
            unuseAudio.loop = true;
            unuseAudio.Play();
		}

		private void StopSound(string sound)
		{
			if (string.IsNullOrEmpty(sound))
				return;
			
			var audio = GetActiveAudio(sound);
			if (audio != null)
			{
				audio.Stop();
				audio.gameObject.SetActive(false);
			}
		}

        /// <summary>
        /// Get current sound is playing.
        /// </summary>
        /// <returns>The playing audio.</returns>
        /// <param name="sound">Sound.</param>
        private AudioSource GetPlayingAudio(string sound)
        {
            var activeAudio = GetActiveAudio(sound);

            if (activeAudio != null)
            {
                return activeAudio;
            }

            return null;
        }

        /// <summary>
        /// Get current active AudioSource
        /// </summary>
        /// <returns>The active audio.</returns>
        /// <param name="sound">Sound.</param>
        private AudioSource GetActiveAudio(string sound)
        {
            foreach (var item in _soundObjectPools)
            {
                if (item.gameObject.activeInHierarchy && item.gameObject.name == sound)
                {
                    return item;
                }
            }

            return null;
        }

        #endregion

        #region [ Private Methods ]

        /// <summary>
        /// Gets the unuse audio.
        /// </summary>
        /// <returns>The unuse audio.</returns>
        private AudioSource GetUnuseAudio()
        {
            foreach (var item in _soundObjectPools)
            {
                if (!item.isPlaying && !item.gameObject.activeInHierarchy)
                {
                    item.gameObject.SetActive(true);
                    return item;
                }
            }

            var unuseAudio = CreateAudio();
            unuseAudio.gameObject.SetActive(true);
            return unuseAudio;
        }

        /// <summary>
        /// Create new Audio sources
        /// </summary>
        /// <returns>The audio.</returns>
        private AudioSource CreateAudio()
        {
            GameObject go = new GameObject();
            go.transform.SetParent(this.transform);
            go.transform.localPosition = Vector3.zero;
            go.transform.localScale = Vector3.one;
            var audioSource = go.GetComponent<AudioSource>();
            if (audioSource == null)
            {
                audioSource = go.AddComponent<AudioSource>();
            }
            go.SetActive(false);
            audioSource.playOnAwake = false;

            _soundObjectPools.Add(audioSource);

            return audioSource;
        }
        #endregion
    }
}