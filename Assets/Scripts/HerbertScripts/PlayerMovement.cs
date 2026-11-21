
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float walkSpeed = 4.5f;
    [SerializeField] private float runSpeed = 7.5f;
    [SerializeField] private float gravity = 30f;

    [Header("Looking")]
    [SerializeField] private float lookSensitivity = 0.2f;
    [SerializeField] private float lookAngleLimit = 90f;

    [Header("Knockback")]
    [SerializeField] private float knockbackDuration = 0.2f;
    [SerializeField] private float knockbackDamping = 8f;

    [Header("Camera Knockback")]
    [SerializeField] private float cameraKnockbackAngle = 5f;
    [SerializeField] private float cameraKnockbackSpeed = 12f;
    [SerializeField] private float cameraRecoverySpeed = 5f;

    private Camera mainCamera;
    private CharacterController characterController;

    private InputAction moveInput;
    private InputAction runInput;

    private float currentMoveSpeed;
    private Vector3 moveDirection = Vector3.zero;
    private Vector3 knockbackVelocity;

    private float lookAngle;
    private float knockbackTimer;

    private float cameraRoll;
    private float targetCameraRoll;

    private float cameraPitch;
    private float targetCameraPitch;

    private float cameraYaw;
    private float targetCameraYaw;

    private void Start()
    {
        mainCamera = GetComponentInChildren<Camera>();
        characterController = GetComponent<CharacterController>();

        moveInput = InputSystem.actions.FindAction("Move");
        runInput = InputSystem.actions.FindAction("Sprint");

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        currentMoveSpeed = walkSpeed;
    }

    private void Update()
    {
        Vector2 moveVector = moveInput.ReadValue<Vector2>();

        currentMoveSpeed = runInput.IsPressed() ? runSpeed : walkSpeed;

        HandleMovement(moveVector);

        Vector2 mouseDelta = Mouse.current.delta.ReadValue();

        HandleLooking(mouseDelta);
        HandleCameraKnockback();
    }

    private void HandleMovement(Vector2 moveVector)
    {
        Vector3 forward = transform.forward;
        Vector3 right = transform.right;

        float oldY = moveDirection.y;

        moveDirection =
            (forward * moveVector.y + right * moveVector.x) * currentMoveSpeed;

        moveDirection.y = oldY;

        if (!characterController.isGrounded)
            moveDirection.y -= gravity * Time.deltaTime;

        if (knockbackTimer > 0f)
        {
            knockbackTimer -= Time.deltaTime;

            knockbackVelocity = knockbackVelocity *= Mathf.Exp(-knockbackDamping * Time.deltaTime);
        }
        else
        {
            knockbackVelocity = Vector3.zero;
        }

        Vector3 finalMovement = moveDirection + knockbackVelocity;

        characterController.Move(finalMovement * Time.deltaTime);
    }

    private void HandleLooking(Vector2 mouseDelta)
    {
        lookAngle += -mouseDelta.y * lookSensitivity;
        lookAngle = Mathf.Clamp(lookAngle, -lookAngleLimit, lookAngleLimit);

        transform.rotation *= Quaternion.Euler(0,mouseDelta.x * lookSensitivity,0);
    }

    private void HandleCameraKnockback()
    {
        cameraRoll = Mathf.Lerp(cameraRoll, targetCameraRoll ,cameraKnockbackSpeed * Time.deltaTime);

        cameraPitch = Mathf.Lerp(cameraPitch,targetCameraPitch, cameraKnockbackSpeed * Time.deltaTime);

        cameraYaw = Mathf.Lerp(cameraYaw, targetCameraYaw, cameraKnockbackSpeed * Time.deltaTime);

        targetCameraRoll = Mathf.Lerp(targetCameraRoll, 0f, cameraRecoverySpeed * Time.deltaTime);

        targetCameraPitch = Mathf.Lerp(targetCameraPitch, 0f, cameraRecoverySpeed * Time.deltaTime);

        targetCameraYaw = Mathf.Lerp(targetCameraYaw, 0f, cameraRecoverySpeed * Time.deltaTime);

        mainCamera.transform.localRotation = Quaternion.Euler(lookAngle + cameraPitch, cameraYaw, cameraRoll);
    }

    public void ApplyKnockback(Vector3 direction, float force)
    {
        direction.y = 0f;
        direction.Normalize();

        knockbackVelocity = direction * force;
        knockbackTimer = knockbackDuration;

        Vector3 localDirection = transform.InverseTransformDirection(direction);

        targetCameraRoll = -localDirection.x * cameraKnockbackAngle;

        targetCameraPitch = -cameraKnockbackAngle;

        targetCameraYaw = -localDirection.x * cameraKnockbackAngle * 0.5f;
    }
}
