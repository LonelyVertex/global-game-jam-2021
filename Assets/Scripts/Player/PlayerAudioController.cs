using UnityEngine;

namespace Player
{
    public class PlayerAudioController : MonoBehaviour
    {
        [SerializeField] AudioSource audioSource;

        [SerializeField] AudioClip drillAudio;

        [SerializeField] AudioClip dashAudio;

        public void DrillStarted()
        {
            Invoke(nameof(RealDrillStarted), 0.1f);
        }

        void RealDrillStarted()
        {
            audioSource.clip = drillAudio;
            audioSource.loop = true;
            audioSource.Play();
        }

        public void DrillStopped()
        {
            audioSource.Stop();
        }

        public void Dash()
        {
            Invoke(nameof(RealDash), 0.1f);
        }

        void RealDash()
        {
            audioSource.clip = dashAudio;
            audioSource.loop = false;
            audioSource.Play();
        }
    }
}