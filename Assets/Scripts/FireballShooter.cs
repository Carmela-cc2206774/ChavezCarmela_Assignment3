using UnityEngine;

namespace StarterAssets
{
    public class FireballShooter : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private StarterAssetsInputs input;
        [SerializeField] private GameObject fireballPrefab;
        [SerializeField] private Transform firePoint;

        [Header("Settings")]
        [SerializeField] private float fireballForce = 20f;
        [SerializeField] private float fireCooldown = 0.5f;
        [SerializeField] private float fireballLifetime = 3f;

        private float _cooldownTimer = 0f;

        private void Update()
        {
            if (_cooldownTimer > 0f)
            {
                _cooldownTimer -= Time.deltaTime;
            }

            if (input != null && input.fireball && CanFire())
            {
                Fire();
                input.fireball = false;
            }
        }

        public bool CanFire()
        {
            return _cooldownTimer <= 0f;
        }

        public void Fire()
        {
            if (!CanFire())
                return;

            if (fireballPrefab == null || firePoint == null)
                return;

            GameObject fireball = Instantiate(
                fireballPrefab,
                firePoint.position,
                firePoint.rotation
            );

            Rigidbody rb = fireball.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = firePoint.forward * fireballForce;
            }

            Destroy(fireball, fireballLifetime);

            _cooldownTimer = fireCooldown;
        }
    }
}