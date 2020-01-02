using System.Collections.Generic;
using UnityEngine;

namespace Penguin.Sound
{
    /// <summary>
    /// Wrapper of Sound Manager
    /// </summary>
    public static class SoundManager
    {
        public static void PlayBGM(string pathName)
        {
            Sound2DManager.Instance.PlayBgm(pathName);
        }

        public static void PauseBGM(string pathName)
        {
            Sound2DManager.Instance.PauseBgm(pathName);
        }

        public static void ResumeBGM(string pathName)
        {
            Sound2DManager.Instance.ResumeBgm(pathName);
        }

        public static void StopBGM(string pathName)
        {
            Sound2DManager.Instance.StopBgm(pathName);
        }

        public static void PlayOneShot(string pathName)
        {
            Sound2DManager.Instance.PlaySound(pathName);
        }

		public static void PlaySound(string pathName, bool loop = false)
		{
			if (loop)
				Sound2DManager.Instance.PlaySoundLoop(pathName);
			else
				Sound2DManager.Instance.PlaySound(pathName);
		}

		public static void StopSound(string pathName)
		{
			Sound2DManager.Instance.StopSound(pathName);
		}
    }

    /// <summary>
    /// Internal class for handling load and playing sounds.
    /// </summary>
    internal sealed class Sound2DManager : MonoBehaviour
    {
        #region [ Static ]

        /// <summary>
        /// The static instance
        /// </summary>
        private static Sound2DManager _instance = new GameObject("[Sound2DManager]").AddComponent<Sound2DManager>();

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>The instance.</value>
        public static Sound2DManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    var go = new GameObject();
                    _instance.gameObject.name = "[" + typeof(Sound2DManager) + "]";

                    _instance = go.AddComponent<Sound2DManager>();

                    DontDestroyOnLoad(_instance.gameObject);
                }

                return _instance;
            }
        }

        #endregion

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
        public bool IsMuteSound
        {
            get
            {
                return _isMuteSound;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="T:Assets.Scripts.Sound.Sound2DManager"/> is mute bgm.
        /// </summary>
        /// <value><c>true</c> if is mute bgm; otherwise, <c>false</c>.</value>
        public bool IsMuteBGM
        {
            get
            {
                return _isMuteBGM;
            }
        }

        #endregion

        #region [ Public Methods ]

        /// <summary>
        /// Awake this instance.
        /// </summary>
        public void Awake()
        {
            if (_instance != null)
            {
                Destroy(this.gameObject);
                return;
            }

            _instance = this;
            _instance.gameObject.name = "[" + typeof(Sound2DManager) + "]";
            DontDestroyOnLoad(_instance.gameObject);

            _isMuteSound = PlayerPrefs.GetInt(KEY_MUTE_SOUND, 0) == 1;
            _isMuteBGM = PlayerPrefs.GetInt(KEY_MUTE_BGM, 0) == 1;

            ////Debug.Log(string.Format("SoundManager:{0}:message = \"is enable sound {1} & is enable BGM {2}\"", "Awake", !_isMuteSound, !_isMuteBGM));
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
        public void PlayBgm(string sound)
        {
            var audio = GetUnuseAudio();
            if (audio != null)
            {
                var clip = Resources.Load<AudioClip>(sound);
                audio.clip = clip;
                audio.gameObject.name = sound;
                audio.loop = true;
                audio.Play();
                if (_isMuteBGM)
                {
                    audio.Pause();
                    ////Debug.Log(string.Format("SoundManager:{0}:message = \"Cant play {1} since BGM had been muted\"", "PlayBgm", sound));
                    return;
                }
            }
        }

        /// <summary>
        /// Stop BGM
        /// </summary>
        /// <param name="sound">Sound Name/Path</param>
        public void StopBgm(string sound)
        {
            var audio = GetPlayingAudio(sound);
            if (audio != null)
            {
                audio.Stop();
            }
        }

        /// <summary>
        /// Pause BGM
        /// </summary>
        /// <param name="sound">Sound Name/Path</param>
        public void PauseBgm(string sound)
        {
            var audio = GetPlayingAudio(sound);
            if (audio != null)
            {
                audio.Pause();
            }
        }

        /// <summary>
        /// Resume BGM
        /// </summary>
        /// <param name="sound">Sound Name/Path</param>
        public void ResumeBgm(string sound)
        {
            if (_isMuteBGM)
                return;
            var audio = GetPlayingAudio(sound);
            if (audio != null)
            {
                audio.UnPause();
            }
        }

        /// <summary>
        /// Play effect sound
        /// </summary>
        /// <param name="sound">Sound Name/Path</param>
        public void PlaySound(string sound)
        {
            if (_isMuteSound)
            {
                ////Debug.Log(string.Format("SoundManager:{0}:message = \"Cant play {1} since sound had been muted\"", "PlaySound", sound));
                return;
            }

            var audio = transform.GetComponent<AudioSource>();
            if (audio == null)
            {
                audio = transform.gameObject.AddComponent<AudioSource>();
            }
            var clip = Resources.Load<AudioClip>(sound);

            if (clip == null)
            {
                ////Debug.LogError("Clip name " + sound + " doesnt exists");
            }

            //audio.clip = clip;
            //audio.loop = false;
            audio.playOnAwake = false;
            audio.PlayOneShot(clip);
        }

		public void PlaySoundLoop(string sound)
		{
			if (_isMuteSound || string.IsNullOrEmpty(sound))
			{
				return;
			}

			var audio = GetUnuseAudio();
			var clip = Resources.Load<AudioClip>(sound);
			audio.gameObject.name = sound;
			audio.playOnAwake = false;
			audio.clip = clip;
			audio.loop = true;
			audio.Play();
		}

		public void StopSound(string sound)
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
        public AudioSource GetPlayingAudio(string sound)
        {
            var audio = GetActiveAudio(sound);

            if (audio != null && audio.isPlaying)
            {
                return audio;
            }

            return null;
        }

        /// <summary>
        /// Get current active AudioSource
        /// </summary>
        /// <returns>The active audio.</returns>
        /// <param name="sound">Sound.</param>
        public AudioSource GetActiveAudio(string sound)
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

            var audio = CreateAudio();
            audio.gameObject.SetActive(true);
            return audio;
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
            var audio = go.GetComponent<AudioSource>();
            if (audio == null)
            {
                audio = go.AddComponent<AudioSource>();
            }
            go.SetActive(false);
            audio.playOnAwake = false;

            _soundObjectPools.Add(audio);

            return audio;
        }
        #endregion
    }
}