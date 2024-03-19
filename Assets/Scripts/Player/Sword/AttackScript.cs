using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackScript : MonoBehaviour
{
    public int attackDame = 20;

    public int damageDragon = 10;
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Enemy")
        {
            other.GetComponent<HealthEnemy>().takeDamage(attackDame);
        }

        if(other.tag == "Dragon")
        {
            other.GetComponent<Dragon>().TakeDamage(damageDragon);
        }
    }

    public void startDealDamage()
    {
        gameObject.GetComponent<BoxCollider>().enabled = true;
    }

    public void endDealDamage()
    {
        gameObject.GetComponent<BoxCollider>().enabled = false;
    }
}
