using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
    NORMAL,
    ATTACK
}

public class PlayerController : MonoBehaviour
{
    [Header("Input fields")]
    private InputManager inputManager;
    private CharacterController characterController;
    private Transform camCenter;

    [Header("Player Settings")]
    PlayerState playerState;
    private Vector3 moveVector;
    private float lookDIstance = 5f;
    private float lookSpeed = 5f;

    [Header("Sound Settings")]
    public AudioSource playerFootAudio;
    public AudioClip footClip;
    public AudioClip rutKiem;
    public AudioClip thuKiem;
    public AudioClip hitChuotTrai;
    public AudioClip hitChuotPhai;

    [Header("Equipment Settings")]
    public GameObject swordHolderHand;
    public GameObject sword;
    public GameObject swordHolderBody;
    GameObject currentSwordInHand;
    GameObject currentSwordInBody;

    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;
    private bool isJumping = false;


    void Start()
    {
        inputManager = GetComponent<InputManager>();
        characterController = GetComponent<CharacterController>();

        camCenter = Camera.main.transform.parent;

        playerState = PlayerState.NORMAL;

        currentSwordInBody = Instantiate(sword, swordHolderBody.transform);
        characterController.enabled = true;

    }
    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isJumping)
        {
            moveVector.y = jumpSpeed;
            isJumping = true;
        }
        else if (characterController.isGrounded)
        {
            isJumping = false;
        }
    }

    void Update()
    {
        if (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0)
            RotateToCamView();

        applyGravity();

        if (playerState == PlayerState.NORMAL)
        {
            Normal();
        }

        if (playerState == PlayerState.ATTACK)
        {
            Attack();
        }
    }

    void Normal()
    {
        inputManager.moveCharacter(Input.GetAxis("Vertical"), Input.GetAxis("Horizontal"));
        inputManager.SprintCharacter(Input.GetButton("Fire3"));

        inputManager.drawweapon(false);

        if (Input.GetKeyDown(KeyCode.Q))
        {
            AudioSource.PlayClipAtPoint(rutKiem, transform.position);
            playerState = PlayerState.ATTACK;
        }
        Jump();
    }

    void Attack()
    {
        inputManager.drawweapon(true);
        inputManager.moveCharacter(Input.GetAxis("Vertical"), Input.GetAxis("Horizontal"));

        inputManager.drawsword();

        if (Input.GetMouseButtonDown(0))
        {
            inputManager.combo1();
            AudioSource.PlayClipAtPoint(hitChuotTrai, transform.position);
        }

        if (Input.GetMouseButtonDown(1))
        {
            inputManager.combo2();
            AudioSource.PlayClipAtPoint(hitChuotPhai, transform.position);
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            AudioSource.PlayClipAtPoint(thuKiem, transform.position);
            playerState = PlayerState.NORMAL;
        }
        Jump();
    }

    void applyGravity()
    {
        if (!characterController.isGrounded)
        {
            moveVector.y -= gravity * Time.deltaTime;
        }
        characterController.Move(moveVector * Time.deltaTime);
    }

    // void applyGravity()
    // {
    //     moveVector = Vector3.zero;
    //     if (!characterController.isGrounded)
    //     {
    //         moveVector += Physics.gravity;
    //     }
    //     characterController.Move(moveVector * Time.deltaTime);
    // } // APply gravity

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

    public void PlayFootSound()
    {
        playerFootAudio.PlayOneShot(footClip);
    } // Play sound when move

    public void drawSword()
    {
        currentSwordInHand = Instantiate(sword, swordHolderHand.transform);
        Destroy(currentSwordInBody);
    } //Enable weapon

    public void sheathSword()
    {
        currentSwordInBody = Instantiate(sword, swordHolderBody.transform);
        Destroy(currentSwordInHand);
    } //Disable weapon

    public void startDealDamage()
    {
        GetComponentInChildren<AttackScript>().startDealDamage();
    } // open box attack
    public void endDealDamage()
    {
        GetComponentInChildren<AttackScript>().endDealDamage();
    } // close box attack
}