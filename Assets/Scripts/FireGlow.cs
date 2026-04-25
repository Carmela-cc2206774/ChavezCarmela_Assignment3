using UnityEngine;
using System.Collections;
using TMPro;

public class FireGlow : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private ParticleSystem fireParticles;
    [SerializeField] private TMP_Text twigCountText;

    [Header("Glow Settings")]
    [SerializeField] private Color normalColor = new Color(1f, 0.6f, 0.2f, 1f);
    [SerializeField] private Color glowColor = new Color(1f, 0.15f, 0.15f, 1f);
    [SerializeField] private float glowDuration = 2f;

    [Header("Twig Count")]
    [SerializeField] private int twigsNeededForEruption = 5;
    private static int globalTwigCount = 0;

    [Header("Eruption")]
    [SerializeField] private GameObject fireballPrefab;
    [SerializeField] private Transform[] fireballSpawnPoints;
    [SerializeField] private float fireballForce = 10f;
    [SerializeField] private int fireballBurstCount = 10;
    [SerializeField] private float fireballLifetime = 3f;
    [SerializeField] private float eruptionDuration = 5f;
    [SerializeField] private float fireballScaleMultiplier = 4f;

    [SerializeField] private AudioSource eruptionAudio;

    [Header("Cameras")]
    [SerializeField] private Camera gameplayCamera;
    [SerializeField] private Camera volcanoCamera;

    private Coroutine glowRoutine;
    private bool hasErupted = false;

    private void Start()
    {
        ApplyParticleColor(normalColor);
        UpdateTwigUI();

        if (volcanoCamera != null)
            volcanoCamera.gameObject.SetActive(false);

        if (gameplayCamera != null)
            gameplayCamera.gameObject.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        TwigPickupWithRange twig = other.GetComponent<TwigPickupWithRange>();
        if (twig == null) return;

        globalTwigCount++;
        UpdateTwigUI();

        if (glowRoutine != null)
            StopCoroutine(glowRoutine);

        glowRoutine = StartCoroutine(GlowRoutine());

        Destroy(other.gameObject);

        if (!hasErupted && globalTwigCount >= twigsNeededForEruption)
        {
            hasErupted = true;
            StartCoroutine(EruptionSequence());
        }
    }

    private IEnumerator GlowRoutine()
    {
        ApplyParticleColor(glowColor);

        yield return new WaitForSeconds(glowDuration);

        ApplyParticleColor(normalColor);
        glowRoutine = null;
    }

    private IEnumerator EruptionSequence()
    {
        SwitchToVolcanoCamera();

        if (eruptionAudio != null)
        {
            eruptionAudio.Stop();
            eruptionAudio.Play();
        }

        yield return StartCoroutine(EruptVolcanoRoutine());

        SwitchToGameplayCamera();
    }
    private IEnumerator EruptVolcanoRoutine()
    {
        if (fireballPrefab == null || fireballSpawnPoints == null || fireballSpawnPoints.Length == 0)
        {
            Debug.LogWarning("Fireball prefab or spawn points are missing.");
            yield break;
        }

        float delayBetweenShots = eruptionDuration / fireballBurstCount;

        for (int i = 0; i < fireballBurstCount; i++)
        {
            Transform spawnPoint = fireballSpawnPoints[i % fireballSpawnPoints.Length];
            GameObject fireball = Instantiate(fireballPrefab, spawnPoint.position, spawnPoint.rotation);

            fireball.transform.localScale *= fireballScaleMultiplier;

            Rigidbody rb = fireball.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;

                Vector3 randomDirection =
                    spawnPoint.forward +
                    new Vector3(
                        Random.Range(-0.5f, 0.5f),
                        Random.Range(0.2f, 0.8f),
                        Random.Range(-0.5f, 0.5f)
                    );

                rb.AddForce(randomDirection.normalized * fireballForce, ForceMode.Impulse);
            }

            Destroy(fireball, fireballLifetime);

            yield return new WaitForSeconds(delayBetweenShots);
        }
    }

    private void SwitchToVolcanoCamera()
    {
        if (gameplayCamera != null)
            gameplayCamera.gameObject.SetActive(false);

        if (volcanoCamera != null)
            volcanoCamera.gameObject.SetActive(true);
    }

    private void SwitchToGameplayCamera()
    {
        if (volcanoCamera != null)
            volcanoCamera.gameObject.SetActive(false);

        if (gameplayCamera != null)
            gameplayCamera.gameObject.SetActive(true);
    }

    private void ApplyParticleColor(Color color)
    {
        if (fireParticles == null) return;

        var main = fireParticles.main;
        main.startColor = color;
    }

    private void UpdateTwigUI()
    {
        if (twigCountText != null)
            twigCountText.text = globalTwigCount.ToString();
    }
}