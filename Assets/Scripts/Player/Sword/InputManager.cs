using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        
    }

    public void moveCharacter(float vertical, float horizontal)
    {
        anim.SetFloat("vertical", vertical);
        anim.SetFloat("horizontal", horizontal);
    }

    public void SprintCharacter(bool isSprinting)
    {
        anim.SetBool("sprint", isSprinting);
    }

    public void drawweapon(bool drawweapon)
    {
        anim.SetBool("drawweapon", drawweapon);
    }

    public void drawsword()
    {
        anim.SetTrigger("drawsword");
    }

    public void combo1()
    {
        anim.SetTrigger("combo1");
    }

    public void combo2()
    {
        anim.SetTrigger("combo2");
    }

    public void takedamage()
    {
        anim.SetTrigger("takedamage");
    }
}
