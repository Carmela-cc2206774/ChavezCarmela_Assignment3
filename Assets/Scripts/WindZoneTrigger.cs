using UnityEngine;

public class WindZoneTrigger : MonoBehaviour
{
    [Header("Wind Audio")]
    [SerializeField] private AudioSource windAudio;

    private void Start()
    {
        if (windAudio != null)
        {
            windAudio.loop = true;
            windAudio.playOnAwake = false;
            windAudio.spatialBlend = 1f;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && windAudio != null && !windAudio.isPlaying)
        {
            windAudio.Play();
        }

        FireWindSwap fire = other.GetComponentInParent<FireWindSwap>();
        if (fire != null)
        {
            fire.SetWindy(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && windAudio != null)
        {
            windAudio.Stop();
        }

        FireWindSwap fire = other.GetComponentInParent<FireWindSwap>();
        if (fire != null)
        {
            fire.SetWindy(false);
        }
    }
}