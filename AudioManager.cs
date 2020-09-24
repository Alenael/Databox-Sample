using UnityEngine;
using UnityEngine.Audio;

namespace Audio
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField]
        private AudioMixer MasterMixer = null;

        private void Start()
        {
            GameStatics.AudioManager = this;
        }

        public void SetMasterVolume(float volume)
        {
            MasterMixer.SetFloat("MasterVolume", volume);
        }

        public void SetMusicVolume(float volume)
        {
            MasterMixer.SetFloat("MusicVolume", volume);
        }

        public void SetSFXVolume(float volume)
        {
            MasterMixer.SetFloat("SFXVolume", volume);
        }
    }
}
