using cakeslice;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Mouse_Interaction : MonoBehaviour
{

    Outline outline;
    // HealthSystem health;

    public int damage;

    // Use this for initialization
    void Start()
    {
        outline = GetComponent<Outline>();
        // health = GetComponentInParent<HealthSystem>();
        outline.color = 1;
    }

    void OnMouseOver()
    {
        int side = GetComponentInParent<MinionState>().side;
        GameObject localplayer = ClientScene.localPlayers[0].gameObject;
        int playerSide = 0;
        if (localplayer.GetComponent<ScifiController>() != null)
        {
            playerSide = localplayer.GetComponent<ScifiController>().side;
            if (side != playerSide)
            {
                outline.color = 0;
                localplayer.GetComponent<ScifiController>().mouseTarget = gameObject;
            }
        }

        if (localplayer.GetComponent<SamuzaiController>() != null)
        {
            playerSide = localplayer.GetComponent<SamuzaiController>().side;
            if (side != playerSide)
            {
                outline.color = 0;
                localplayer.GetComponent<SamuzaiController>().mouseTarget = gameObject;
            }
        }

    }
    void OnMouseExit()
    {
        outline.color = 1;

        int side = GetComponentInParent<MinionState>().side;
        GameObject localplayer = ClientScene.localPlayers[0].gameObject;
        int playerSide = 0;
        //if (localplayer.GetComponent<ScifiController>() != null) playerSide = localplayer.GetComponent<ScifiController>().side;
        //if (localplayer.GetComponent<SamuzaiController>() != null) playerSide = localplayer.GetComponent<SamuzaiController>().side;
        //if (side != playerSide) localplayer.GetComponent<ScifiController>().mouseTarget = null;

        if (localplayer.GetComponent<ScifiController>() != null)
        {
            playerSide = localplayer.GetComponent<ScifiController>().side;
            if (side != playerSide)
            {
                localplayer.GetComponent<ScifiController>().mouseTarget = null;
            }
        }

        if (localplayer.GetComponent<SamuzaiController>() != null)
        {
            playerSide = localplayer.GetComponent<SamuzaiController>().side;
            if (side != playerSide)
            {
                localplayer.GetComponent<SamuzaiController>().mouseTarget = null;
            }
        }

    }

    //void OnMouseDown()
    //{
    //    health.TakeDamage(damage);
    //}

}
