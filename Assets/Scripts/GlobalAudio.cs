using UnityEngine;

public class GlobalAudio : MonoBehaviour
{
    [SerializeField] 
    AudioSource audioSource;
    
    [SerializeField]
    AudioClip collectAudio;

    public void PlayCollectSound()
    {
        audioSource.clip = collectAudio;
        audioSource.Play();
    }
}
