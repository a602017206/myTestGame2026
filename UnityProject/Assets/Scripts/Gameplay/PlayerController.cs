using UnityEngine;

namespace Gameplay
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float rotationSpeed = 720f;

        private Rigidbody body;
        private Vector3 moveInput;

        private void Awake()
        {
            body = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");
            Vector3 forward = Vector3.forward;
            Vector3 right = Vector3.right;

            if (Camera.main != null)
            {
                forward = Camera.main.transform.forward;
                right = Camera.main.transform.right;
                forward.y = 0f;
                right.y = 0f;
                forward.Normalize();
                right.Normalize();
            }

            moveInput = (forward * vertical + right * horizontal);
        }

        private void FixedUpdate()
        {
            if (moveInput.sqrMagnitude <= 0.001f)
            {
                return;
            }

            Vector3 direction = moveInput.normalized;
            Vector3 targetPosition = body.position + direction * moveSpeed * Time.fixedDeltaTime;
            body.MovePosition(targetPosition);

            Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
            Quaternion rotation = Quaternion.RotateTowards(body.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
            body.MoveRotation(rotation);
        }
    }
}
