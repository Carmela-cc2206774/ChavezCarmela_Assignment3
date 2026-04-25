using System.Collections;
using UnityEngine;

namespace StarterAssets
{
    public class FireballShooter : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private StarterAssetsInputs input;
        [SerializeField] private Animator animator;
        [SerializeField] private GameObject fireballPrefab;
        [SerializeField] private Transform firePoint;

        [Header("Animation")]
        [SerializeField] private string fireballTriggerName = "Fireball";
        [SerializeField] private float shootDelay = 0.35f;

        [Header("Projectile Settings")]
        [SerializeField] private float fireballForce = 20f;
        [SerializeField] private float fireballLifetime = 3f;

        [Header("Cooldown")]
        [SerializeField] private float fireCooldown = 0.8f;

        private float cooldownTimer = 0f;
        private bool isCasting = false;

        private void Update()
        {
            if (cooldownTimer > 0f)
                cooldownTimer -= Time.deltaTime;

            if (input != null && input.fireball && !isCasting && cooldownTimer <= 0f)
            {
                input.fireball = false;
                StartCoroutine(FireballRoutine());
            }
        }

        private IEnumerator FireballRoutine()
        {
            isCasting = true;

            if (animator != null)
                animator.SetTrigger(fireballTriggerName);

            yield return new WaitForSeconds(shootDelay);

            SpawnFireball();

            cooldownTimer = fireCooldown;
            isCasting = false;
        }

        private void SpawnFireball()
        {
            if (fireballPrefab == null || firePoint == null)
                return;

            GameObject fireball = Instantiate(fireballPrefab, firePoint.position, firePoint.rotation);

            Rigidbody rb = fireball.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = firePoint.forward * fireballForce;
            }

            Destroy(fireball, fireballLifetime);
        }
    }
}