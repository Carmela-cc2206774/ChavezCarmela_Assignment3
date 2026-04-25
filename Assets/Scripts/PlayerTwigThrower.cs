using UnityEngine;

public class PlayerTwigThrower : MonoBehaviour
{
    [Header("Throw Settings")]
    [SerializeField] private float throwForce = 8f;
    [SerializeField] private float upwardForce = 2f;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            ThrowHeldTwig();
        }
    }

    private void ThrowHeldTwig()
    {
        TwigPickupWithRange[] allTwigs = FindObjectsOfType<TwigPickupWithRange>();

        foreach (TwigPickupWithRange twig in allTwigs)
        {
            if (twig != null && twig.IsHeld)
            {
                twig.ThrowTwig(throwForce, upwardForce);
                return;
            }
        }
    }
}