using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Penguin.Sound
{
    /// <summary>
    /// Sound Target is a mono behaviour that can play sound by it self.
    /// </summary>
    public class SoundTarget : MonoBehaviour
    {
        #region [ Fields ]
        /// <summary>
        /// The name of the target.
        /// </summary>
        public string targetName;

        /// <summary>
        /// The audio source for playing personal sound and will not be interupted by others.
        /// </summary>
        public AudioSource source;

        /// <summary>
        /// The audio source for playing one shot sound effect
        /// </summary>
        public AudioSource oneShotSource;

        private List<AudioClip> queueClip = new List<AudioClip>();

        #endregion

        #region [ Public Methods ]

        /// <summary>
        /// Play one shot
        /// </summary>
        /// <param name="sound">Sound.</param>
        public void PlayOneShot(string sound)
        {
            if (string.IsNullOrEmpty(sound))
                return;

            var clip = Resources.Load<AudioClip>(sound);
            oneShotSource.PlayOneShot(clip);
        }

        /// <summary>
        /// Play personal sound
        /// </summary>
        /// <param name="sound">Sound.</param>
        /// <param name="interrupt">If set to <c>true</c> interrupt.</param>
        public void PlayPersonal(string sound, bool interrupt = false)
        {
            if (string.IsNullOrEmpty(sound))
                return;

            var clip = Resources.Load<AudioClip>(sound);

            if (clip == null)
            {
                ////Debug.LogError("Clip name " + sound + " doesnt exists");
            }

            if (interrupt || !source.isPlaying)
            {
                source.Stop();
                source.clip = clip;
                source.loop = false;
                source.Play();
            }
            else
            {
                queueClip.Add(clip);
                if (queueClip.Count == 1)
                {
                    StartCoroutine(WaitAndPlay());
                }
            }
        }

        #endregion

        #region [ Private Methods ]

        /// <summary>
        /// Coroutine wait for current playing sound is finish then start play the next sound in queue
        /// </summary>
        /// <returns>The and play.</returns>
        IEnumerator WaitAndPlay()
        {
            bool hasSound = true;
            while (hasSound)
            {
                yield return new WaitWhile(() => source.isPlaying);

                if (queueClip.Count > 0)
                {
                    var nextClip = queueClip[0];
                    queueClip.RemoveAt(0);

                    source.clip = nextClip;
                    source.loop = false;
                    source.Play();
                }
                else
                {
                    hasSound = false;
                }
            }
        }

        #endregion
    }
}
