using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : MonoBehaviour
{
    [Header("Arrow Setting")]
    public float arrowCount;
    public Rigidbody arrowPrefab;
    private Rigidbody currentArrow;
    public float arrowForce = 3f;

    public Transform arrowPos;
    public Transform arrowEquipParent;

    public Transform EquipPos;
    public Transform UnEquipPos;
    public Transform UnEquipParent;
    public Transform EquipParent;

    public Transform BowString;
    public Transform stringInitialPos;
    public Transform stringHandPullPos;
    public Transform stringInititalParent;

    public Bow bowscript;

    [Header("Crosshair Setting")]
    public GameObject crossHairPrefab;
    private GameObject currentCrossHair;


    [Header("Audio Source")]
    AudioSource bowAudio;
    public AudioClip pullStringAudio;
    public AudioClip releaseStringAudio;
    public AudioClip pickArrow;

    void Start()
    {
        bowAudio = GetComponent<AudioSource>();
    }

    //******************************************Using Animator******************************************************//
    public void PickArrow()
    {
        bowAudio.PlayOneShot(pickArrow);
        arrowPos.gameObject.SetActive(true);

    }

    public void DisableArrow()
    {
        arrowPos.gameObject.SetActive(false);
    }

    public void PullString()
    {
        bowAudio.PlayOneShot(pullStringAudio);
        BowString.transform.position = stringHandPullPos.position;
        BowString.parent = stringHandPullPos;
    }

    public void ReleaseString()
    {
        BowString.transform.position = stringInitialPos.position;
        BowString.transform.parent = stringInititalParent;
    }

    public void EquipBow()
    {
        this.transform.position = EquipPos.position;
        this.transform.rotation = EquipPos.rotation;
        this.transform.parent = EquipParent;
    }

    public void UnEquipBow()
    {
        this.transform.position = UnEquipPos.position;
        this.transform.rotation = UnEquipPos.rotation;
        this.transform.parent = UnEquipParent;
    }

    //******************************************Using Input System******************************************************//

    //****************************************** Function ******************************************************//

    public void ShowCrosshair(Vector3 crossHairPos) 
    {
        if (!currentCrossHair)
            currentCrossHair = Instantiate(crossHairPrefab) as GameObject;

        currentCrossHair.transform.position = crossHairPos;
        currentCrossHair.transform.LookAt(Camera.main.transform);
    } // Show cross hair

    public void RemoveCorssHair()
    {
        if (currentCrossHair)
            Destroy(currentCrossHair);
    } // Remove cross hair

    public void Fire(Vector3 hitPoint)
    {
        if (arrowCount < 1)
            return;
        arrowCount -= 1;

        bowAudio.PlayOneShot(releaseStringAudio);
        Vector3 dir = hitPoint - arrowPos.position;
        currentArrow = Instantiate(arrowPrefab, arrowPos.position, arrowPos.rotation) as Rigidbody;
        currentArrow.AddForce(dir * arrowForce, ForceMode.VelocityChange);

    }
    //****************************************** End Function ******************************************************//
}
