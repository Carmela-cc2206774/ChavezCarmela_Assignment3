using UnityEngine;

public class FireWindSwap : MonoBehaviour
{
    [SerializeField] private GameObject normalFire;
    [SerializeField] private GameObject windyFire;

    private void Start()
    {
        if (normalFire != null) normalFire.SetActive(true);
        if (windyFire != null) windyFire.SetActive(false);
    }

    public void SetWindy(bool windy)
    {
        if (normalFire != null) normalFire.SetActive(!windy);
        if (windyFire != null) windyFire.SetActive(windy);
    }
}