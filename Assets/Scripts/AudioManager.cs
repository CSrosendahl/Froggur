using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public AudioSource audioSource;
    public AudioSource backgroundSound;
    public Image toggleAllButtonImage;
    public Image toggleSpecificAudioImage;

    // Dictionary to track the last played time of each clip
    private Dictionary<AudioClip, float> clipLastPlayedTime = new Dictionary<AudioClip, float>();
    public float cooldownTime = 1.0f; // Cooldown time in seconds

    private bool allAudioMuted = false;
    private List<AudioSource> audioSources = new List<AudioSource>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

       
    }



    private void Start()
    {
        // Find all AudioSource components in the scene and add them to the list
        AudioSource[] sources = FindObjectsOfType<AudioSource>();
        foreach (AudioSource source in sources)
        {
            audioSources.Add(source);
        }
    }

    public void PlaySound(AudioClip clip)
    {
        if (clip == null)
        {
            Debug.LogWarning("AudioClip is null, cannot play sound.");
            return;
        }

        // Check if the clip has a cooldown time set and if it has been played recently
        if (clipLastPlayedTime.ContainsKey(clip))
        {
            float lastPlayedTime = clipLastPlayedTime[clip];
            if (Time.time - lastPlayedTime < cooldownTime)
            {
                Debug.LogWarning("AudioClip is still in cooldown, skipping.");
                return;
            }
        }

        // Play the clip and update the last played time
        audioSource.PlayOneShot(clip);
        clipLastPlayedTime[clip] = Time.time;
    }

    public void ResetAudioClipCooldown(AudioClip clip)
    {
        if (clipLastPlayedTime.ContainsKey(clip))
        {
            clipLastPlayedTime.Remove(clip);
            Debug.Log("Cooldown for AudioClip reset.");
        }
        else
        {
            Debug.LogWarning("AudioClip is not in the cooldown list.");
        }
    }

    public void PlaySoundNoSkip(AudioClip clip)
    {
        if (clip == null)
        {
            Debug.LogWarning("AudioClip is null, cannot play sound.");
            return;
        }
        audioSource.PlayOneShot(clip);
    }

    public void RegisterAudioSource(AudioSource source)
    {
        if (!audioSources.Contains(source))
        {
            audioSources.Add(source);
        }
    }

    public void UnregisterAudioSource(AudioSource source)
    {
        if (audioSources.Contains(source))
        {
            audioSources.Remove(source);
        }
    }

    public void ToggleAllMute()
    {
     

        allAudioMuted = !allAudioMuted;
        foreach (AudioSource source in audioSources)
        {
        

            source.mute = allAudioMuted;
        }

        if (allAudioMuted)
        {
            toggleAllButtonImage.color = Color.black;
        }
        else
        {
            toggleAllButtonImage.color = Color.yellow;
        }
    }



    public bool AllAudioMuted()
    {
        return allAudioMuted;
    }

    public void ToggleMuteAudioSource(AudioSource source)
    {
        if (source != null)
        {
            source.mute = !source.mute;
        }
        if(source.mute)
        {
            toggleSpecificAudioImage.color = Color.black;
        }else
        {
            toggleSpecificAudioImage.color = Color.yellow;
        }
    }
}
