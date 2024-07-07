using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public AudioSource audioSource;

    // Dictionary to track the last played time of each clip
    private Dictionary<AudioClip, float> clipLastPlayedTime = new Dictionary<AudioClip, float>();
    public float cooldownTime = 1.0f; // Cooldown time in seconds

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

        audioSource = GetComponent<AudioSource>();
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
}
