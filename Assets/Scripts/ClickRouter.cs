// using UnityEngine;
// using UnityEngine.InputSystem;

// public class ClickRouter : MonoBehaviour
// {
//     [SerializeField] private Camera cam;
//     [SerializeField] private float reach = 10f;

//     private InputAction click;
//     private PickupTarget held;

//     private void Awake()
//     {
//         if (cam == null)
//             cam = Camera.main;

//         click = new InputAction(type: InputActionType.Button, binding: "<Mouse>/leftButton");
//         click.started += _ => OnClickPressed();
//         click.Enable();
//     }

//     private void OnDisable()
//     {
//         click.Disable();
//     }

//     private void OnClickPressed()
//     {
//         Debug.Log("Left click detected");

//         if (held == null)
//             TryPick();
//         else
//         {
//             held.Drop();
//             held = null;
//         }
//     }

//     private void TryPick()
//     {
//         if (cam == null)
//         {
//             Debug.LogWarning("Camera not assigned");
//             return;
//         }

//         Ray ray = cam.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));
//         Debug.DrawRay(ray.origin, ray.direction * reach, Color.red, 2f);

//         if (Physics.Raycast(ray, out RaycastHit hit, reach))
//         {
//             Debug.Log("Ray hit: " + hit.collider.name);

//             PickupTarget target = hit.collider.GetComponent<PickupTarget>();

//             if (target == null)
//                 target = hit.collider.GetComponentInParent<PickupTarget>();

//             if (target == null)
//                 target = hit.collider.GetComponentInChildren<PickupTarget>();

//             if (target != null)
//             {
//                 Debug.Log("PickupTarget found on: " + target.name);
//                 held = target;
//                 held.Pick();
//             }
//             else
//             {
//                 Debug.LogWarning("Hit object has no PickupTarget");
//             }
//         }
//         else
//         {
//             Debug.LogWarning("Raycast hit nothing");
//         }
//     }
// }