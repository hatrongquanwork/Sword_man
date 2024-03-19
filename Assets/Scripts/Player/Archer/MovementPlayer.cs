using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementPlayer : MonoBehaviour
{
    private Animator anim;
    [Header("Player Settings Bow")]
    private Transform camCenter;
    private Transform mainCam;

    public float lookDIstance = 5f;
    public float lookSpeed = 5f;

    bool isAiming;

    private RaycastHit hit;
    private Ray ray;
    public LayerMask aimLayers;

    public Transform spine;
    public Vector3 spineOffSet;

    public Bow bowScript;

    public bool testAim;

    CharacterController characterController;
    Vector3 moveVector;

    [Header("Sound Setting")]
    public AudioSource playerFootAudio;
    public AudioClip footClip;

    bool hitDetected;

    void Start()
    {
        anim = GetComponent<Animator>();
        camCenter = Camera.main.transform.parent;
        mainCam = Camera.main.transform;
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        // Camera rotate to cam view
        if (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0)
            RotateToCamView();

        ApplyGravity();
        movement();
    }

    private void LateUpdate()
    {
        if (isAiming)
            RotateCharacterSpine(); 
    }


    void SwordCombat()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            chartacterFireSword1();
        }
    }

    void movement()
    {
        // Archer move
        AnimateCharacter(Input.GetAxis("Vertical"), Input.GetAxis("Horizontal"));
        SprintCharacter(Input.GetButton("Fire3"));

        isAiming = Input.GetButton("Fire2");
        if (testAim)
            isAiming = true;

        if (bowScript.arrowCount < 1)
            isAiming = false;

        characterAim(isAiming);    

        if (isAiming)
        {
            Aim();
            bowScript.EquipBow();
            characterPullString(Input.GetButton("Fire1"));
            if (Input.GetButtonUp("Fire1"))
            {
                chartacterFire();
                if (hitDetected)
                {
                    bowScript.Fire(hit.point);
                }
                else
                {
                    bowScript.Fire(ray.GetPoint(500f));
                }
            }
            
        }
        else
        {
            bowScript.UnEquipBow();
            bowScript.RemoveCorssHair();
            bowScript.DisableArrow();
            bowScript.ReleaseString();
        }
    } // get button to anim

    void RotateToCamView()
    {
        Vector3 camCenterPos = camCenter.position;
        Vector3 lookPoint = camCenterPos + (camCenter.forward * lookDIstance);
        Vector3 direction = lookPoint - transform.position;

        Quaternion lookRotation = Quaternion.LookRotation(direction);
        lookRotation.x = 0;
        lookRotation.z = 0;

        Quaternion finalRotation = Quaternion.Lerp(transform.rotation, lookRotation, lookSpeed * Time.deltaTime);
        transform.rotation = finalRotation;
    } // rotate character belong to camera

    void Aim()
    {
        Vector3 camPos = mainCam.position; 
        Vector3 dir = mainCam.forward;

        ray = new Ray(camPos, dir);
        if(Physics.Raycast(ray, out hit, 2000f, aimLayers))
        {
            hitDetected = true;
            Debug.DrawLine(ray.origin, hit.point, Color.green);
            bowScript.ShowCrosshair(hit.point);
        }
        else
        {
            hitDetected = false;
        }

    } // Show cross hair if isAiming


    void ApplyGravity()
    {
        moveVector = Vector3.zero;
        if(!characterController.isGrounded) {
            moveVector += Physics.gravity;
        }
        characterController.Move(moveVector * Time.deltaTime);
    }

    void RotateCharacterSpine()
    {
        spine.LookAt(ray.GetPoint(50));
        spine.Rotate(spineOffSet);
    } // rotate spine of character when aiming

    public void PullString()
    {
        bowScript.PullString();
    }

    public void EnableArrow()
    {
        bowScript.PickArrow();
    } // can see arrow

    public void DisableArrow()
    {
        bowScript.DisableArrow();
    } // can not see arrow

    public void ReleaseString()
    {
        bowScript.ReleaseString();
    }

    public void PlayFootSound()
    {
        playerFootAudio.PlayOneShot(footClip);
    } // Play sound when move

    void AnimateCharacter(float forward, float strafe)
    {
        anim.SetFloat("forward", forward);
        anim.SetFloat("strafe", strafe);
    }

    void SprintCharacter(bool isSprinting)
    {
        anim.SetBool("sprint", isSprinting);
    }
    public void characterAim(bool aiming)
    {
        anim.SetBool("aim", aiming);
    }

    public void characterPullString(bool pull)
    {
        anim.SetBool("pullString", pull);
    }

    public void chartacterFire()
    {
        anim.SetTrigger("fire");
    }

    public void chartacterFireSword1()
    {
        anim.SetTrigger("attack1");
    }

}
