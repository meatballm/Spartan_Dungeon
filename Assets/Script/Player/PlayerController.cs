using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    private Vector2 curMovementInput;
    public float jumpPower;
    public LayerMask groundLayerMask;
    public float sprintSpeed = 1.5f;       // 달릴 때 속도 배수
    public float sprintStamina = 10f;   // 초당 소비량
    private bool isSprint=false;

    [Header("Look")]
    public Transform cameraContainer;
    public float minXLook;
    public float maxXLook;
    private float camCurXRot;
    public float lookSensitivity;
    public float cameraHeight;

    [Header("ThirdPerson")]
    public float camDistance;
    private Camera MainCamera;
    private Camera EquipCamera;
    private Vector2 mouseDelta;

    [HideInInspector]
    public bool canLook = true;
    public Action inventory;
    private Rigidbody rigidbody;

    public static PlayerController instance = null;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        if (instance == null)
        {
            instance = this;
        }
    }
    public static PlayerController Instance
    {
        get
        {
            if (null == instance)
            {
                return null;
            }
            return instance;
        }
    }
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        Transform camTf = cameraContainer.Find("MainCamera");
        if (camTf != null)
            MainCamera = camTf.GetComponent<Camera>();
        if (MainCamera == null)
            Debug.LogError("MainCamera를 camContainer 하위에서 찾지 못했습니다!");

        camTf = cameraContainer.Find("EquipCamera");
        if (camTf != null)
            EquipCamera = camTf.GetComponent<Camera>();
        if (EquipCamera == null)
            Debug.LogError("EquipCamera를 camContainer 하위에서 찾지 못했습니다!");
    }

    private void FixedUpdate()
    {
        Move();
    }
    private void Update()
    {
        if (isSprint && curMovementInput != Vector2.zero)
        {
            bool ok = CharacterManager.Instance.Player.condition.UseSprint(sprintStamina);
            if (!ok)
                isSprint = false;
        }
    }
    private void LateUpdate()
    {
        if (canLook)
        {
            CameraLook();
        }
    }

    public void OnLookInput(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();
    }

    public void OnMoveInput(InputAction.CallbackContext context)
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

    public void OnJumpInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && IsGrounded())
        {
            rigidbody.AddForce(Vector2.up * jumpPower, ForceMode.Impulse);
        }
    }

    public void OnSprintStarted(InputAction.CallbackContext context)
    {
        if(context.started)
            isSprint = true;
        else if(context.canceled)
            isSprint = false;
    }

    private void Move()
    {
        Vector3 dir = transform.forward * curMovementInput.y + transform.right * curMovementInput.x;
        dir *= (curMovementInput != Vector2.zero && isSprint)
            ? sprintSpeed*moveSpeed
            : moveSpeed;
        dir.y = rigidbody.velocity.y;

        rigidbody.velocity = dir;
    }

    void CameraLook()
    {
        camCurXRot += mouseDelta.y * lookSensitivity;
        camCurXRot = Mathf.Clamp(camCurXRot, minXLook, maxXLook);
        MainCamera.transform.localEulerAngles = new Vector3(-camCurXRot, 0, 0);
        MainCamera.transform.localPosition = new Vector3(0, camDistance * Mathf.Sin(-camCurXRot * Mathf.Deg2Rad) + cameraHeight, -camDistance * Mathf.Cos(-camCurXRot * Mathf.Deg2Rad));
        EquipCamera.transform.localEulerAngles = new Vector3(-camCurXRot, 0, 0);
        EquipCamera.transform.localPosition = new Vector3(0, cameraHeight, 0);

        transform.eulerAngles += new Vector3(0, mouseDelta.x * lookSensitivity, 0);
    }

    bool IsGrounded()
    {
        Ray[] rays = new Ray[4]
        {
            new Ray(transform.position + (transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.right * 0.2f) +(transform.up * 0.01f), Vector3.down)
        };

        for (int i = 0; i < rays.Length; i++)
        {
            if (Physics.Raycast(rays[i], 0.1f, groundLayerMask))
            {
                return true;
            }
        }

        return false;
    }
    public void OnInventoryButton(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.phase == InputActionPhase.Started)
        {
            inventory?.Invoke();
            ToggleCursor();
        }
    }

    void ToggleCursor()
    {
        bool toggle = Cursor.lockState == CursorLockMode.Locked;
        Cursor.lockState = toggle ? CursorLockMode.None : CursorLockMode.Locked;
        canLook = !toggle;
    }
}