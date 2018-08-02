using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class HealthSystem : NetworkBehaviour
{
    public const int startingHealth = 100;
    [SyncVar(hook = "OnChangeHealth")] public int currentHealth = startingHealth;
    public float sinkSpeed = 3f;

    Animator anim;
    CapsuleCollider capsuleCollider;

    Outline outline;

    public bool isPlayer = false;
    bool isChanged = false;
    bool isDead;
    bool isSinking;
   
    [Header("Unity Stuff")]
    public Image healthBar;

    void Awake()
    {
        anim = GetComponent<Animator>();
        capsuleCollider = GetComponent<CapsuleCollider>();
    }

    void Update ()
    {
        if (!isChanged)
        {
            if (isLocalPlayer)
            {
                healthBar.GetComponent<Image>().color = Color.green;
                isChanged = true;
            }
            else if(gameObject.tag == "Minion")
            {
                int side = GetComponent<MinionState>().side;
                GameObject playerobject = ClientScene.localPlayers[0].gameObject;
                if (playerobject.GetComponent<PlayerController>() != null)
                {
                    //int playerSide = ClientScene.localPlayers[0].gameObject.GetComponent<PlayerController2>().side;
                    int playerSide = 1;
                    if (side == playerSide)
                    {
                        healthBar.GetComponent<Image>().color = Color.blue;
                    }
                    isChanged = true;
                }
            }
            else
            {
                isChanged = true;
            }
        }
        //if (isPlayer)
        //{
        //    healthBar.GetComponent<Image>().color = Color.green;
        //}
        //else if(!isChanged)
        //{
        //    int side = GetComponent<MinionState>().side;
        //    GameObject playerobject = ClientScene.localPlayers[0].gameObject;
        //    if (playerobject.GetComponent<PlayerController>() != null) {
        //        int playerSide = ClientScene.localPlayers[0].gameObject.GetComponent<PlayerController>().side;
        //        if (side == playerSide)
        //        {
        //            healthBar.GetComponent<Image>().color = Color.blue;
        //        }
        //        isChanged = true;
        //    }
        //}

        if (isSinking)
        {
            transform.Translate (-Vector3.up * sinkSpeed * Time.deltaTime);
        }
    }


    public void TakeDamage (int amount)
    {
        if (!isServer)
        {
            return;
        }

        if (isDead) return;

        currentHealth -= amount;
        healthBar.fillAmount = (float) currentHealth / startingHealth;

        if(currentHealth <= 0)
        {
            Death ();
        }
    }

    void OnChangeHealth(int health)
    {
        currentHealth = health;
        healthBar.fillAmount = (float) health / startingHealth;
    }


    void Death ()
    {
        isDead = true;
        capsuleCollider.isTrigger = true;
        anim.SetTrigger ("Dead");
        Rpcdeathsignal();
    }

    [ClientRpc]
    void Rpcdeathsignal()
    {
        isDead = true;
        capsuleCollider.isTrigger = true;
        anim.SetTrigger("Dead");
    }


    public void StartSinking ()
    {
        GetComponent <UnityEngine.AI.NavMeshAgent> ().enabled = false;
        GetComponent <Rigidbody> ().isKinematic = true;
        isSinking = true;
        Destroy (gameObject, 2f);
    }
}
