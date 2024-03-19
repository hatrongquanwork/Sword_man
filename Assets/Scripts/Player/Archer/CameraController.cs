using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    //Rotate camera
    private Transform center_Camera;
    private Transform target;
    private Camera mainCamera;

    public float zoomSpeed = 5f;
    public float moveSpeed_Camera = 5f;
    public float rotationSpeed = 5f;
    public float Sensivity = 5f;
    private Vector2 deafault_Look_Limits = new Vector2(-30f, 90f);
    float cameraXrotation = 0;
    float cameraYrotation = 0;

    //Camera collision
    public Transform camPosition;
    Vector3 IntitalCamPos;
    RaycastHit hit;
    public LayerMask CamCollisionLayer;

    //Status
    [HideInInspector] public bool lockCursor = true;
    [HideInInspector] public bool canMove = true;

    void Start()
    {
        mainCamera = Camera.main;
        center_Camera = transform.GetChild(0);
        FindPlayer();
        IntitalCamPos = mainCamera.transform.localPosition;
    }

    void Update()
    {
        if (!target)
            return;

        if (!Application.isPlaying)
            return;

        if (lockCursor)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        RotateCamera();
        HandleCamCollision();
    }

    void LateUpdate()
    {
        if (target)
        {
            followPlayer();
        }
        else
        {
            FindPlayer();
        }
    }

    void FindPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if(player != null) 
        {
            target = player.transform;
        }

    } // Find tag Player

    void followPlayer()
    {
        Vector3 moveVector = Vector3.Lerp(transform.position, target.transform.position, moveSpeed_Camera * Time.deltaTime);

        transform.position = moveVector;
    } // Camera move to Player

    void RotateCamera()
    {
        if (!canMove)
            return;

        cameraXrotation += Input.GetAxis("Mouse Y") * Sensivity * -1f;
        cameraYrotation += Input.GetAxis("Mouse X") * Sensivity;

        cameraXrotation = Mathf.Clamp(cameraXrotation, deafault_Look_Limits.x, deafault_Look_Limits.y);
        cameraYrotation = Mathf.Repeat(cameraYrotation, 360);

        Vector3 rotatingAngle = new Vector3(cameraXrotation, cameraYrotation, 0f);

        Quaternion rotation = Quaternion.Slerp(center_Camera.transform.localRotation, Quaternion.Euler(rotatingAngle), rotationSpeed * Time.deltaTime);

        center_Camera.transform.localRotation = rotation;
    } // Rotate camera

    void HandleCamCollision()
    {
        if(Physics.Linecast(target.transform.position + target.transform.up, camPosition.position, out hit, CamCollisionLayer))
        {
            Vector3 newCamPos = new Vector3(hit.point.x + hit.normal.x * 0.2f, hit.point.y + hit.normal.y * 0.8f, hit.point.z + hit.normal.z * 0.2f);
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, newCamPos, Time.deltaTime * moveSpeed_Camera);
        }
        else
        {
            mainCamera.transform.localPosition = Vector3.Lerp(mainCamera.transform.localPosition, IntitalCamPos, Time.deltaTime * moveSpeed_Camera);
        }
    } // Camera to character if face to wall
}