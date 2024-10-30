using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Player Movement")]
    public float moveSpeed = 5f;
    public float moveSpeedMultiplier = 2;
    public float accelerationTime = 5f;
    public float accelCooldownTime = 5f;
    public float jumpForce = 5f;
    public int accelerationManaCost = 10;
    private Vector2 curMovementInput;
    private float lastAccelerationTime = float.MinValue;
    private bool isAccelerating;

    public LayerMask groundLayerMask;

    [Header("Look")]
    public Transform cameraContainer;
    public float maxXLook;
    public float minXLook;
    private float camCurXRot;
    [Range(0, 1)] public float lookSensitivity = 1f;
    private Vector2 mouseDelta;

    private Rigidbody _rigidbody;
    private PlayerCondition condition;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        _rigidbody = GetComponent<Rigidbody>();
        condition = GetComponent<PlayerCondition>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move();
    }

    private void LateUpdate()
    {
        if (UIManager.Instance.CanLook)
        {
            CameraLook();
        }
    }

    private void Move()
    {
        Vector3 dir = transform.forward * curMovementInput.y + transform.right * curMovementInput.x;
        if (isAccelerating)
        {
            if (Time.time - lastAccelerationTime > accelerationTime)
            {
                isAccelerating = false;
            }

            dir *= moveSpeed * moveSpeedMultiplier;
        }
        else
        {
            dir *= moveSpeed;
        }
        dir.y = _rigidbody.velocity.y;

        _rigidbody.velocity = dir;
    }

    private void CameraLook()
    {
        camCurXRot += mouseDelta.y * lookSensitivity;
        camCurXRot = Mathf.Clamp(camCurXRot, minXLook, maxXLook);
        cameraContainer.localEulerAngles = new Vector3(-camCurXRot, 0, 0);

        transform.eulerAngles += new Vector3(0, mouseDelta.x * lookSensitivity, 0);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            curMovementInput = context.ReadValue<Vector2>();
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            curMovementInput = Vector2.zero;
        }
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && IsGrounded())
        {
            _rigidbody.AddForce(Vector2.up * jumpForce, ForceMode.Impulse);
        }
    }

    private bool IsGrounded()
    {
        Ray[] ray = new Ray[4]
        {
            new Ray(transform.position + (transform.forward * 0.2f) + (transform.up * 0.1f), Vector3.down),
            new Ray(transform.position + (-transform.forward * 0.2f) + (transform.up * 0.1f), Vector3.down),
            new Ray(transform.position + (transform.right * 0.2f) + (transform.up * 0.1f), Vector3.down),
            new Ray(transform.position + (-transform.right * 0.2f) + (transform.up * 0.1f), Vector3.down),
        };

        for (int i = 0; i < ray.Length; i++)
        {
            if (Physics.Raycast(ray[i], 0.1f, groundLayerMask))
            {
                return true;
            }
        }

        return false;
    }

    public void OnInventory(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            UIManager.Instance.Toggle<UIInventory>(true);
        }
    }

    public void OnEscape(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            UIManager.Instance.Toggle<UISetting>(true);
        }
    }
    public void OnAcceleration(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            if (Time.time - lastAccelerationTime >= accelCooldownTime && condition.UseMana(accelerationManaCost))
            {
                isAccelerating = true;
                lastAccelerationTime = Time.time;
            }
        }
    }

    public void SetLookSensitivity(float value)
    {
        lookSensitivity = value;
    }
}
