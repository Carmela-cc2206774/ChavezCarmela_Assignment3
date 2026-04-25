using UnityEngine;

public class PickupTarget3rdPOV : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] private Transform onhand; 
    [SerializeField] private PickupTarget3rdPOV twig;
    [SerializeField] private string playerTag = "Player";

    private Rigidbody rb;
    private bool playerInRange = false;
    private bool isHeld = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnMouseDown()
    {
        if (!playerInRange || isHeld) return;

        PickUp();
    }

    private void OnMouseUp()
    {
        if (!isHeld) return;

        Drop();
    }

    public void SetPlayerInRange(bool value)
    {
        playerInRange = value;

        if (!isHeld)
        {
            Cursor.visible = value;
            Cursor.lockState = value ? CursorLockMode.None : CursorLockMode.Locked;
        }
    }

    private void PickUp()
    {
        if (rb == null || onhand == null) return;

        isHeld = true;
        playerInRange = false;

        rb.useGravity = false;
        rb.isKinematic = true;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        transform.SetParent(onhand);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    private void Drop()
    {
        if (rb == null) return;

        isHeld = false;

        transform.SetParent(null);
        rb.isKinematic = false;
        rb.useGravity = true;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
            twig.SetPlayerInRange(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(playerTag))
            twig.SetPlayerInRange(false);
    }
}