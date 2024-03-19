using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    private InputManager inputManager;
    public int attackDame = 10;

    private void Start()
    {
        inputManager = GameObject.Find("Paladin").GetComponent<InputManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<HealthPlayer>().takeDamage(attackDame);
            inputManager.takedamage();
        }
    }
    public void enemyStartDealDamage()
    {
        gameObject.GetComponent<BoxCollider>().enabled = true;
    }

    public void enemyEndDealDamage()
    {
        gameObject.GetComponent<BoxCollider>().enabled = false;
    }
}
