using UnityEngine;

public class TwigPickupWithRange : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] private Transform onhand;
    [SerializeField] private string playerTag = "Player";

    private Rigidbody rb;
    private bool playerInRange = false;
    private bool isHeld = false;

    public bool IsHeld => isHeld;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {

        
        if (other.CompareTag("Fire"))
        {
            Destroy(gameObject);
        }
        
        if (!other.CompareTag(playerTag)) return;

        playerInRange = true;

        if (!isHeld)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag(playerTag)) return;

        playerInRange = false;

        if (!isHeld)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    private void OnMouseDown()
    {
        if (!playerInRange || isHeld) return;

        PickUp();
    }

    private void PickUp()
    {
        if (rb == null || onhand == null) return;

        isHeld = true;

        rb.useGravity = false;
        rb.isKinematic = true;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        transform.SetParent(onhand);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void ThrowTwig(float throwForce, float upwardForce)
    {
        if (!isHeld || rb == null || onhand == null) return;

        isHeld = false;

        transform.SetParent(null);

        rb.isKinematic = false;
        rb.useGravity = true;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        Vector3 throwDirection = onhand.forward;
        Vector3 force = throwDirection * throwForce + Vector3.up * upwardForce;

        rb.AddForce(force, ForceMode.Impulse);

        if (playerInRange)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }


}