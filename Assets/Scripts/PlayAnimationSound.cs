using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAnimationSound : MonoBehaviour
{
  
    public void PlayAnimSound(AudioClip clip)
    {
        AudioManager.instance.PlaySoundNoSkip(clip);
    }
}
