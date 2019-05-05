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

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            audioSource.clip = sounds[0].clip;
            audioSource.Play();
            Debug.Log("Played: " + audioSource.clip);
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            audioSource.clip = sounds[1].clip;
            audioSource.Play();
            Debug.Log("Played: " + audioSource.clip);
        }
    }

    public void SelectSound(bool success)
    {
        int i = 0;

        if (!success) i = 1;
        

        audioSource.clip = sounds[i].clip;
        audioSource.pitch = sounds[i].pitch;
        audioSource.Play();
    }
    public void UISound(bool success)
    {
        audioSource.clip = sounds[0].clip;
        audioSource.pitch -= 0.5f;
        audioSource.Play();
    }

    public void PlaySound(string soundName)
    {
        foreach(Sound sound in sounds)
        {
            if(sound.name == soundName)
            {
                audioSource.clip = sound.clip;
                audioSource.pitch = sound.pitch;
                audioSource.volume = sound.volume;
                audioSource.Play();
                break;
            }
        }
    }



}
