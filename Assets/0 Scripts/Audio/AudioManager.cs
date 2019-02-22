using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    #region Singleton
    public static AudioManager Instance;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        audioSource = GetComponent<AudioSource>();
        audioSource.loop = false;
    }
    #endregion

    [System.Serializable]
    public class Sound
    {
        public string name;
        public AudioClip clip;
        [Range(0f, 1f)]
        public float volume;
        [Range(-3f, 3f)]
        public float pitch;
    }

    public Sound[] sounds;
    private AudioSource audioSource;
    

    public void PlaySound(string name)
    {
        foreach(Sound sound in sounds)
        {
            if(sound.name == name)
            {
                audioSource.clip = sound.clip;
                break;
            }
        }

        if (audioSource.clip)
            audioSource.Play();
    }


}
