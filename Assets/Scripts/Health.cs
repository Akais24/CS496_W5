using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Health : NetworkBehaviour {

    public bool isPlayer = false;

    public Image image;
    public float scale;
    public float currentHealth;
    public float startingHealth = 100;
    public bool isDead = false;
    private GameObject player;
    PlayerMovement playerMovement;
    Actions actions;
    

    //AudioSource playerAudio;

    // Use this for initialization
    void Start () {
        // playerMovement = GetComponent<PlayerMovement>();
        //playerAudio = GetComponent<AudioSource>();
        currentHealth = startingHealth;
        actions = GetComponent<Actions>();
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void TakeDamage(int champion, int skill)
    {
        //일단
        //from samuzai
        if(champion == 0)
        {
            switch (skill)
            {
                //평타
                case 0:
                    {
                        currentHealth -= 4;
                        actions.Damage();
                    }
                    break;

                //Q
                case 1:
                    {
                        currentHealth -= 2;
                        player = GameObject.FindGameObjectWithTag("Player");
                        this.transform.position = Vector3.MoveTowards(this.transform.position, player.transform.position, 40 * Time.deltaTime);
                        actions.Jump();
                    }
                    break;
                //R
                case 4:
                    {
                        currentHealth -= 2;
                        actions.Damage();
                    }
                    break;

            }
        }

        //from Scifi
        if(champion == 1)
        {
            switch (skill)
            { //평타
                case 0:
                    {
                        currentHealth -= 2;

                    }
                    break;

                //Q
                case 1:
                    {
                        currentHealth -= 8;

                    }
                    break;
                case 4:
                    {
                        currentHealth -= 1;
                    }
                    break;

            }
        }


        image.fillAmount = currentHealth / startingHealth;
        if (currentHealth <= 0 && !isDead)
        {
            Death();

        }
    }

    void Death()
    {
        isDead = true;
        actions.Death();
        //playerMovement.enabled = false;
    }

    public void Healthier()
    {
        currentHealth += 5;
        image.fillAmount = currentHealth / startingHealth;

    }


}
