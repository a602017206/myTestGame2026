using UnityEngine;

namespace Gameplay
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float rotationSpeed = 720f;
        [SerializeField] private float jumpForce = 6f;
        [SerializeField] private float groundCheckDistance = 0.25f;

        private Rigidbody body;
        private Vector3 moveInput;
        private bool jumpRequested;

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

            if (Input.GetButtonDown("Jump"))
            {
                jumpRequested = true;
            }
        }

        private void FixedUpdate()
        {
            if (moveInput.sqrMagnitude <= 0.001f)
            {
                moveInput = Vector3.zero;
            }

            if (moveInput.sqrMagnitude > 0.001f)
            {
                Vector3 direction = moveInput.normalized;
                Vector3 targetPosition = body.position + direction * moveSpeed * Time.fixedDeltaTime;
                body.MovePosition(targetPosition);

                Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
                Quaternion rotation = Quaternion.RotateTowards(body.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
                body.MoveRotation(rotation);
            }

            if (jumpRequested)
            {
                jumpRequested = false;
                if (IsGrounded())
                {
                    body.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                }
            }
        }

        private bool IsGrounded()
        {
            Vector3 origin = body.position + Vector3.up * 0.1f;
            return Physics.Raycast(origin, Vector3.down, groundCheckDistance + 0.1f);
        }
    }
}
