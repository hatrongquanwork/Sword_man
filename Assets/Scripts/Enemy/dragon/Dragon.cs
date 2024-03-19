using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dragon : MonoBehaviour
{

    public int HP = 100;
    public Animator animator;
    public Slider healthBar;
    public GameObject NPC;
    private Collider objectCollider;
    public AudioClip soundDeath;

    
    void Update() 
    {
        healthBar.value = HP;    
    }


    // public void Scream()
    // {
    //     FindObjectOfType<AudioManager>().Play("DragonScream");
    // }
    // public void Attack()
    // {
    //     FindObjectOfType<AudioManager>().Play("DragonAttack");
    // }

    public void TakeDamage(int damageDragon)
    {
        HP -= damageDragon;
        if (HP <=0)
        {
            // AudioManager.instance.Play("DragonDeath");
            animator.SetTrigger("die");
            AudioSource.PlayClipAtPoint(soundDeath, transform.position);

            objectCollider = GetComponent<Collider>();
            objectCollider.enabled = false;

            NPC.SetActive(true);
        }
        else
        {
            // AudioManager.instance.Play("DragonDamage");
            animator.SetTrigger("damage");
        }
    }

    

}
