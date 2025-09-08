using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    [Header("타겟  설정")]
    public Transform target;

    [Header("카메라 거리 설정")]
    public float distance = 8.0f;
    public float height = 5.0f;

    [Header("마우스 설정")]
    public float mouseSonsitivity = 2.0f;
    public float minVerticalAngel = -30.0f;
    public float maxVerticalAngel = 60.0f;

    [Header("부드러움 설정")]
    public float positionSmoothTime = 0.2f;
    public float rotationSmoothTime = 0.1f;

    private float horzizontalAngle = 0f;
    private float verticalAnge = 0f;

    private Vector3 currentVelocity;
    private Vector3 currentPosition;
    private Quaternion currentRotation;

    void HandleMouseInput()
    {
        if (Cursor.lockState != CursorLockMode.Locked) return;

        float mouseX = Input.GetAxis("Mouse X") * mouseSonsitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSonsitivity;

        horzizontalAngle += mouseX;
        verticalAnge -= mouseY;

        verticalAnge = Mathf.Clamp(verticalAnge , minVerticalAngel, maxVerticalAngel);
    }

    void UpdateCameraSmooth()
    {
        Quaternion rotation = Quaternion.Euler(verticalAnge, horzizontalAngle, 0);
        Vector3 rotatedOffset = rotation * new Vector3(0, height, -distance);
        Vector3 targetPosition = target.position + rotatedOffset;

        Vector3 lookTarget = target.position + Vector3.up * height;
        Quaternion targetRotation = Quaternion.LookRotation(lookTarget - targetPosition);

        currentPosition = Vector3.SmoothDamp(currentPosition, targetPosition,ref currentVelocity,positionSmoothTime);

        currentRotation = Quaternion.Slerp(currentRotation, targetRotation, Time.deltaTime / rotationSmoothTime);

        transform.position = currentPosition;
        transform.rotation = currentRotation;
    
    }
    void ToggleCursor()
    {
        if(Cursor.lockState == CursorLockMode.Locked)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if(target == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
                target = player.transform;
            
        }
        currentPosition = transform.position;
        currentRotation = transform.rotation;


    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)) { ToggleCursor(); }

    }
    private void LateUpdate()
    {
        if (target == null) return;

        HandleMouseInput();
        UpdateCameraSmooth();

    }
}
