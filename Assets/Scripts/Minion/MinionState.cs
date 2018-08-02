using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class MinionState : NetworkBehaviour
{
    // Movement + Attack

    Transform enemy_nexus;
    Transform target_destination;
    HealthSystem healthsystem;
    UnityEngine.AI.NavMeshAgent nav;

    public int side;
    public bool isAttack;

    public float timeBetweenAttacks = 0.5f;
    public int attackDamage = 10;

    bool InRange;
    float timer;

    public int collisioncounter;

    float gatherTime = 0.0f;
    Vector3 gatherDest;
    float gatherSpeed = 10.0f;

    void Awake()
    {
        isAttack = false;
        healthsystem = GetComponent<HealthSystem>();
        nav = GetComponent<UnityEngine.AI.NavMeshAgent>();

        timer = 0f;
        collisioncounter = 0;
    }

    public void setSide(int givenside)
    {
        this.side = givenside;
        enemy_nexus = GameObject.FindGameObjectWithTag("Nexus" + (Mathf.Abs(side - 2) + 1).ToString()).transform;
        nav = GetComponent<UnityEngine.AI.NavMeshAgent>();
        nav.enabled = true;
        target_destination = enemy_nexus;
    }


    void Update()
    {
        timer += Time.deltaTime;
        if(gatherTime > 0)
        {
            gatherTime -= Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, gatherDest, gatherSpeed * Time.deltaTime);
            return;
        }
        if (collisioncounter <= 0)
        {
            isAttack = false;
            target_destination = enemy_nexus;
        }

        if (nav == null) return;
        if (healthsystem.currentHealth > 0 && enemy_nexus != null)
        {
            nav.enabled = true;
            nav.SetDestination(target_destination.position);
        }
        else
        {
            nav.enabled = false;
        }
    }

    public void Gather(Vector3 dest)
    {
        gatherDest = dest;
        gatherTime = 0.5f;
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject target = other.gameObject;
        if (target.tag == "Minion")
        {
            if (target.GetComponent<MinionState>().side != side) collisioncounter += 1;
        }
        else if (target.tag == "Champion")
        {
            if (target.GetComponent<ScifiController>() != null)
            {
                if (target.GetComponent<ScifiController>().side != side) collisioncounter += 1;
            }
            if (target.GetComponent<SamuzaiController>() != null)
            {
                if (target.GetComponent<SamuzaiController>().side != side) collisioncounter += 1;
            }
        }
    }


    void OnTriggerStay(Collider other)
    {
        GameObject target = other.gameObject;
        if (target.tag == "Minion")
        {
            if (target.GetComponent<MinionState>().side != side)
            {
                isAttack = true;

                if (timer >= timeBetweenAttacks)
                {
                    timer = 0f;
                    target_destination = target.transform;
                    if (target.GetComponent<HealthSystem>() != null) target.GetComponent<HealthSystem>().TakeDamage(attackDamage);
                    else if (target.GetComponent<ChampHealthSystem>() != null) target.GetComponent<ChampHealthSystem>().TakeDamage(attackDamage);

                    //Debug.Log("ATTACK");
                }
            }
        }
        else if (target.tag == "Champion")
        {
            if (target.GetComponent<ScifiController>() != null)
            {
                if (target.GetComponent<ScifiController>().side != side)
                {
                    isAttack = true;

                    if (timer >= timeBetweenAttacks)
                    {
                        timer = 0f;
                        target_destination = target.transform;
                        if (target.GetComponent<ChampHealthSystem>() != null) target.GetComponent<ChampHealthSystem>().TakeDamage(attackDamage);

                        //Debug.Log("ATTACK");
                    }
                }
            }else if (target.GetComponent<SamuzaiController>() != null)
            {
                if (target.GetComponent<SamuzaiController>().side != side)
                {
                    isAttack = true;

                    if (timer >= timeBetweenAttacks)
                    {
                        timer = 0f;
                        target_destination = target.transform;
                        if (target.GetComponent<ChampHealthSystem>() != null) target.GetComponent<ChampHealthSystem>().TakeDamage(attackDamage);

                        //Debug.Log("ATTACK");
                    }
                }
            }
        }

    }


    void OnTriggerExit(Collider other)
    {
        GameObject target = other.gameObject;
        if (target.tag == "Minion")
        {
            if (target.GetComponent<MinionState>().side != side)
            {
                collisioncounter -= 1;
                if (collisioncounter == 0) isAttack = false;
            }
        }
        else if (target.tag == "Champion")
        {
            if (target.GetComponent<ScifiController>() != null)
            {
                if (target.GetComponent<ScifiController>().side != side) collisioncounter -= 1;
            }
            if (target.GetComponent<SamuzaiController>() != null)
            {
                if (target.GetComponent<SamuzaiController>().side != side) collisioncounter -= 1;
            }
        }
    }
}
