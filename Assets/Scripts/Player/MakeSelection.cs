using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MakeSelection : NetworkBehaviour {

    public GameObject canvasPrefab;
    public GameObject[] playerprefabs;
    
    private GameObject canvas;

	void Start () {
        //Cmdpreprocess();
        canvas = (GameObject)Instantiate(canvasPrefab);
    }

    public void select(int index)
    {
        //if (isLocalPlayer)
        //{
        //    playerPrefab = prefab;
        //    Destroy(canvas);
        //    CmdSummon();
        //}
        if (isLocalPlayer && canvas != null) Cmdsummon(index);

    }

    private void Update()
    {
        if (!isLocalPlayer && canvas != null)
        {
            Destroy(canvas);
        }
    }

    //[Command]
    //public void Cmdpreprocess()
    //{
    //    canvas = (GameObject)Instantiate(canvasPrefab);
    //}

    [Command]
    public void Cmdsummon(int index)
    {
        Debug.Log("CmdSummon");
        //NetworkServer.Destroy(canvas);
        Transform spawnPoint = transform;

        GameObject targetPrefab = playerprefabs[index];
        GameObject champ = (GameObject)Instantiate(targetPrefab, spawnPoint.position, spawnPoint.rotation);
        if(champ.GetComponent<HealthSystem>() != null) champ.GetComponent<HealthSystem>().isPlayer = true;
        if(champ.GetComponent<Health>() != null) champ.GetComponent<Health>().isPlayer = true;

        int side = 2;
        Transform point1 = GameObject.FindGameObjectWithTag("RespawnPoint1").transform;
        if (spawnPoint.position == point1.position) side = 1;

        Debug.Log("SPAWN : " + spawnPoint.position.ToString());
        Debug.Log("RESPAWN : " + point1.position.ToString());
        Debug.Log("SIDE : " + side.ToString());
        
        if (champ.GetComponent<ScifiController>() != null) champ.GetComponent<ScifiController>().side = side;
        if (champ.GetComponent<SamuzaiController>() != null) champ.GetComponent<SamuzaiController>().side = side;

        NetworkServer.ReplacePlayerForConnection(connectionToClient, champ, playerControllerId);
        NetworkServer.Destroy(gameObject);
    }
}
