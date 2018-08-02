using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class ChampHealthSystem : NetworkBehaviour
{
    public bool isCham1;
    
    //bool isChanged = false;
    public bool isDead = false;

    public Image healthBar;
    public float currentHealth;
    public float startingHealth = 500;
    Animation anim;

    private GameObject player;
    PlayerMovement playerMovement;
    Actions actions;

    private Vector3 dest;
    public float MoveTime = 0.0f;
    public float moveDistance;

    void Start()
    {
        currentHealth = startingHealth;
        if (isCham1)
            anim = GetComponent<Animation>();
        else
            actions = GetComponent<Actions>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isLocalPlayer)
        {
            healthBar.GetComponent<Image>().color = Color.green;
            //isChanged = true;
        }

        if (MoveTime > 0 && transform.position != dest)
        {
            MoveTime -= Time.deltaTime;
            MoveTo(dest);
        }
    }

    public void TakeDamage(int amount)
    {
        if (!isServer)
        {
            return;
        }

        if (isDead) return;

        currentHealth -= amount;
        healthBar.fillAmount = (float)currentHealth / startingHealth;
        CmdTakeDamage(amount);

        if (currentHealth <= 0)
        {
            Death();
        }
    }

    [Command]
    void CmdTakeDamage(int amount)
    {
        if (isDead) return;

        currentHealth -= amount;
        healthBar.fillAmount = (float)currentHealth / startingHealth;

        if (currentHealth <= 0)
        {
            Death();
        }
    }

    public void MoveTo(Vector3 destination)
    {
        dest = destination;
        //int total_length = 100;
        float costTime = 10.0f;
        float speed = 5f;

        // moveDistance = total_length * Time.deltaTime;
        MoveTime = costTime;

        this.transform.position = Vector3.MoveTowards(this.transform.position, destination, speed * Time.deltaTime);

    }

    public void Jump()
    {
        if (isCham1)
            anim.Play("Jump");
        else
            actions.Jump();
    }

    public void Damage()
    {
        if (!isCham1)
            actions.Damage();
    }


    void Death()
    {
        Debug.Log("DEATH SIGNAL");
        isDead = true;
        if (isCham1)
        {
            anim.Play("SamuzaiDeath");
            GetComponent<SamuzaiController>().enabled = false;
        }
        else
        {
            actions.Death();
            GetComponent<ScifiController>().enabled = false;
        }
           
    }

    public void Healthier()
    {
        currentHealth += 5;
        healthBar.fillAmount = currentHealth / startingHealth;
        CmdHealthier();

    }

    [Command]
    void CmdHealthier()
    {
        currentHealth += 5;
        healthBar.fillAmount = currentHealth / startingHealth;

    }

    void OnChangeHealth(int health)
    {
        currentHealth = health;
        healthBar.fillAmount = (float)health / startingHealth;
    }


}
