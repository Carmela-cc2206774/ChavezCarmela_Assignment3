using UnityEngine;
using System.Collections;

public class WaterAmbienceTrigger : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioSource waterAudio;

    [Header("Cameras")]
    [SerializeField] private Camera gameplayCamera;
    [SerializeField] private Camera lakeCamera;

    [Header("Timing")]
    [SerializeField] private float delayBeforeCameraSwitch = 3f;
    [SerializeField] private float lakeCameraDuration = 3f;

    [Header("Player")]
    [SerializeField] private string playerTag = "Player";

    private bool hasPlayedCameraView = false;
    private Coroutine lakeViewRoutine;

    private void Start()
    {
        if (waterAudio != null)
        {
            waterAudio.loop = true;
            waterAudio.playOnAwake = false;
            waterAudio.spatialBlend = 1f;
        }

        if (lakeCamera != null)
            lakeCamera.gameObject.SetActive(false);

        if (gameplayCamera != null)
            gameplayCamera.gameObject.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(playerTag)) return;

        if (waterAudio != null && !waterAudio.isPlaying)
            waterAudio.Play();

        if (!hasPlayedCameraView)
        {
            hasPlayedCameraView = true;
            lakeViewRoutine = StartCoroutine(LakeCameraRoutine());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag(playerTag)) return;

        if (waterAudio != null)
            waterAudio.Stop();

        if (lakeViewRoutine != null)
        {
            StopCoroutine(lakeViewRoutine);
            lakeViewRoutine = null;
            SwitchToGameplayCamera();
        }
    }

    private IEnumerator LakeCameraRoutine()
    {
        yield return new WaitForSeconds(delayBeforeCameraSwitch);

        SwitchToLakeCamera();

        yield return new WaitForSeconds(lakeCameraDuration);

        SwitchToGameplayCamera();
        lakeViewRoutine = null;
    }

    private void SwitchToLakeCamera()
    {
        if (gameplayCamera != null)
            gameplayCamera.gameObject.SetActive(false);

        if (lakeCamera != null)
            lakeCamera.gameObject.SetActive(true);
    }

    private void SwitchToGameplayCamera()
    {
        if (lakeCamera != null)
            lakeCamera.gameObject.SetActive(false);

        if (gameplayCamera != null)
            gameplayCamera.gameObject.SetActive(true);
    }
}