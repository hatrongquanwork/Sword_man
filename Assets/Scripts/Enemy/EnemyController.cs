using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyState
{
    PATROL,
    CHASE,
    ATTACK
}

public class EnemyController : MonoBehaviour
{
    EnemyAnim enemy_Anim;
    NavMeshAgent navAgent;
    EnemyState enemy_State;

    [Header("Enemy Settings")]
    public float walk_Speed = 0.5f;
    public float run_Speed = 4f;
    public float chase_Distance = 7f;
    float current_Chase_Distance;
    public float attack_Distance = 1.8f;
    public float chase_After_Attack_Distance = 2f;

    public float patrol_Radius_Min = 20f, patrol_Radius_Max = 60f;
    public float patrol_For_This_Time = 15f;
    float patrol_Timer;

    public float wait_Before_Attack = 2f;
    float attack_Timer;

    private Transform target;
    public AudioClip enemyDetectSound;
    // public Dialogue dialogue;


    void Awake()
    {
        // dialogue = dialogue.GetComponent<Dialogue>();
        enemy_Anim = GetComponent<EnemyAnim>();
        navAgent = GetComponent<NavMeshAgent>();

        target = GameObject.FindWithTag("Player").transform;
    }

    void Start()
    {
        enemy_State = EnemyState.PATROL;
        //patrol_Timer = patrol_For_This_Time;

        // when the enemy first gets to the player attack right away
        attack_Timer = wait_Before_Attack;

        // memorize the value of chase distance
        current_Chase_Distance = chase_Distance;
    }

    // Update is called once per frame
    void Update()
    {
        // if (dialogue.hasOpened && dialogue.hasCompleted)
        // {
        //     if (enemy_State == EnemyState.PATROL)
        //     {
        //         enemy_State = EnemyState.PATROL;
        //     }
        // }
        // else
        // {
        //     if (enemy_State == EnemyState.CHASE)
        //     {
        //         enemy_State = EnemyState.PATROL;
        //     }
        // }

        if (enemy_State == EnemyState.PATROL)
        {
            Patrol();
        }
        if (enemy_State == EnemyState.CHASE)
        {
            Chase();
        }
        if (enemy_State == EnemyState.ATTACK)
        {
            Attack();
        }

    }


    void Patrol()
    {
        navAgent.isStopped = false;

        navAgent.speed = walk_Speed;

        patrol_Timer += Time.deltaTime;
        if(patrol_Timer > patrol_For_This_Time)
        {
            SetNewRandomDestination();
            patrol_Timer = 0f;
        }
        
        if(navAgent.velocity.sqrMagnitude > 0)
        {
            enemy_Anim.Walk(true);
        }
        else{
            enemy_Anim.Walk(false);
        }

        if( target!= null && Vector3.Distance(transform.position, target.position) <= chase_Distance)
        {
            enemy_Anim.Walk(false);
            enemy_State = EnemyState.CHASE;
        }
    }

    void Chase()
    {
        if(target != null)
        {
            navAgent.isStopped = false;
            navAgent.speed = run_Speed;

            navAgent.SetDestination(target.position);
            if (navAgent.velocity.sqrMagnitude > 0)
            {
                enemy_Anim.Run(true);
            }
            else
            {
                enemy_Anim.Run(false);
            }

            if (Vector3.Distance(transform.position, target.position) <= attack_Distance)
            {
                enemy_Anim.Run(false);
                enemy_Anim.Walk(false);
                enemy_State = EnemyState.ATTACK;

                // reset the chase distance
                if(chase_Distance != current_Chase_Distance)
                {
                    chase_Distance = current_Chase_Distance;
                } 
            } 
            else if(Vector3.Distance(transform.position, target.position) > chase_Distance)
            {
                enemy_Anim.Run(false);
                enemy_State = EnemyState.PATROL;

                // reset patrol Timer
                patrol_Timer = patrol_For_This_Time;

                if (chase_Distance != current_Chase_Distance)
                {
                    chase_Distance = current_Chase_Distance;
                }
            }
        }
    }

    void Attack()
    {
        navAgent.velocity = Vector3.zero;
        navAgent.isStopped = true;

        attack_Timer += Time.deltaTime;
        if(attack_Timer > wait_Before_Attack)
        {
            enemy_Anim.Attack();

            attack_Timer = 0f;
        }

        if(target!= null && Vector3.Distance(transform.position, target.position) > attack_Distance + chase_After_Attack_Distance)
        {
            enemy_State = EnemyState.CHASE;
        }
    }

    void SetNewRandomDestination()
    {
        float rand_Radius = Random.Range(patrol_Radius_Min, patrol_Radius_Max);

        Vector3 randDir = Random.insideUnitSphere * rand_Radius;
        randDir += transform.position;

        NavMeshHit navHit;
        NavMesh.SamplePosition(randDir, out navHit, rand_Radius, -1);
        navAgent.SetDestination(navHit.position);
    }

    public void enemyStartDealDamage()
    {
        GetComponentInChildren<EnemyAttack>().enemyStartDealDamage();
    } // open box attack

    public void enemyEndDealDamage()
    {
        GetComponentInChildren<EnemyAttack>().enemyEndDealDamage();
    } // close box attack

void OnTriggerEnter(Collider other)
{
    if (other.CompareTag("Player"))
    {
        // Play the sound effect
        AudioSource.PlayClipAtPoint(enemyDetectSound, transform.position);

        // Wait for 5 seconds before attacking
        StartCoroutine(WaitBeforeAttack());
    }
}

IEnumerator WaitBeforeAttack()
{
    yield return new WaitForSeconds(5f);

    // Change enemy state to CHASE and start attacking
    enemy_State = EnemyState.CHASE;
}


}
