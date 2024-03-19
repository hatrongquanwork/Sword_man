    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class HealthPlayer : MonoBehaviour
{
    [Header("Stat Settings")]
    public float health;
    private float maxHealth = 100f;

    public float hunger;
    private float maxHunger = 100f;

    public float thirst;
    private float maxThirst = 100f;

    [Header("Stats Deleption")]
    public float hungerDeleption = 0.5f;
    public float thirstDelepltion = 0.75f;

    [Header("Stats Damage")]
    public float hungerDamage = 1.5f;
    public float thirstDamage = 2.25f;

    [Header("UI")]
    public StatsBar healthBar;
    public StatsBar hungerBar;
    public StatsBar thirstBar;
    private Animator animator;
    private Collider objectCollider;
    public GameObject logLoseObject;
    public AudioClip soundDeath;
    public AudioClip biHit;

    public AudioClip backgroundClip;
    private AudioSource audioSource;

    private void Start()
    {
        health = maxHealth;
        hunger = maxHunger;
        thirst = maxThirst;
        animator = GetComponent<Animator>();

        audioSource = GetComponent<AudioSource>(); 

        audioSource.clip = backgroundClip; 
        audioSource.loop = true; 
        audioSource.Play();
    }

    private void Update()
    {
        UpdateStats();
        UpdateUI();
    }

    public void UpdateUI()
    {
        healthBar.numberText.text = health.ToString("f0");
        healthBar.bar.fillAmount = health / 100;

        hungerBar.numberText.text = hunger.ToString("f0");
        hungerBar.bar.fillAmount = hunger / 100;

        thirstBar.numberText.text = thirst.ToString("f0");
        thirstBar.bar.fillAmount = thirst / 100;
    }

    public void UpdateStats()
    {
        if (health <= 0)
        {
            GetComponent<Collider>().enabled = false;
            audioSource.Stop();
            StartCoroutine(DieCoroutine());
        }
        if (health >= maxHealth)
            health = maxHealth;

        if (hunger <= 0)
            hunger = 0;
        if (hunger >= maxHunger)
            hunger = maxHunger;

        if (thirst <= 0)
            thirst = 0;
        if (thirst >= maxThirst)
            thirst = maxThirst;

        // Damage
        if (hunger <= 0)
            health -= hungerDamage * Time.deltaTime;

        if (thirst <= 0)
            health -= thirstDamage * Time.deltaTime;

        // Deleption
        if (hunger > 0)
            hunger -= hungerDeleption * Time.deltaTime;

        if (thirst > 0)
            thirst -= thirstDelepltion * Time.deltaTime;

    } // Update stats

    public void takeDamage(int damageAmount)
    {
        health -= damageAmount;
        AudioSource.PlayClipAtPoint(biHit, transform.position);

        if (health <= 0)
        {
            animator.SetTrigger("die");
            objectCollider = GetComponent<Collider>();
            objectCollider.enabled = false;
            AudioSource.PlayClipAtPoint(soundDeath, transform.position);
            StartCoroutine(DieCoroutine());
        
        }
    } // minus health when be attack

    IEnumerator DieCoroutine()
    {
        MiniMapSettings miniMapSettings = FindObjectOfType<MiniMapSettings>();
        MiniMapCameraFollow miniMapCameraFollow = FindObjectOfType<MiniMapCameraFollow>();

        if (miniMapSettings != null)
        {
            miniMapSettings.gameObject.SetActive(false);
        }

        if (miniMapCameraFollow != null)
        {
            miniMapCameraFollow.gameObject.SetActive(false);
        }

        
        // yield return new WaitForSeconds(2f);
        

        logLoseObject.SetActive(true);
        yield return new WaitForSeconds(9f);
        Destroy(this.gameObject);
        

        SceneManager.LoadScene(1);
    }



    // void Die()
    // {
    //     MiniMapSettings miniMapSettings = FindObjectOfType<MiniMapSettings>();
    //     MiniMapCameraFollow miniMapCameraFollow = FindObjectOfType<MiniMapCameraFollow>();

    //     if (miniMapSettings != null)
    //     {
    //         miniMapSettings.gameObject.SetActive(false);
    //     }

    //     if (miniMapCameraFollow != null)
    //     {
    //         miniMapCameraFollow.gameObject.SetActive(false);
    //     }

    //     AudioSource.PlayClipAtPoint(soundDeath, transform.position);
    //     Destroy(this.gameObject);

    //     StartCoroutine(ShowLogLoseCoroutine());
        

    // }
    // IEnumerator DieCoroutine()
    // {
    //     yield return new WaitForSeconds(2.5f); // chờ 2 giây
    //     Die();
    // }

    // IEnumerator ShowLogLoseCoroutine()
    // {
    //     logLoseObject.SetActive(true);
    //     yield return new WaitForSeconds(9f);

    //     SceneManager.LoadScene(1);
        
    // }
}