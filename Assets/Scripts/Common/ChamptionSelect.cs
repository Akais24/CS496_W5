using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChamptionSelect : MonoBehaviour {
    
    //public GameObject championPrefab;
    public int index = 0;
    private GameObject canvas;
    
	void Start () {
        Button btn = GetComponent<Button>();
        btn.onClick.AddListener(process);
    }

    void process()
    {
        GameObject respawn = GameObject.FindGameObjectWithTag("RespawnPoint");
        if (respawn != null)
        {
            canvas = GameObject.FindGameObjectWithTag("SelectionCanvas");
            respawn.GetComponent<MakeSelection>().select(index);
            Destroy(canvas);
        }
            
    }
	
 //   [Command]
	//public void CmdSummon()
 //   {
 //       Debug.Log("CLICK ON BUTTON");
 //       canvas = GameObject.FindGameObjectWithTag("SelectionCanvas");
 //       GameObject respawn = GameObject.FindGameObjectWithTag("RespawnPoint");
 //       if(respawn != null)
 //       {
 //           Transform spawnPoint = respawn.transform;
 //           NetworkIdentity identity = respawn.GetComponent<NetworkIdentity>();

 //           GameObject champ = (GameObject)Instantiate(championPrefab, spawnPoint.position, spawnPoint.rotation);
 //           champ.GetComponent<HealthSystem>().isPlayer = true;

 //           int side = 2;
 //           Transform point1 = GameObject.FindGameObjectWithTag("RespawnPoint1").transform;
 //           if (spawnPoint.position == point1.position) side = 1;
 //           champ.GetComponent<PlayerController>().side = side;

 //           NetworkServer.ReplacePlayerForConnection(identity.connectionToClient, champ, identity.playerControllerId);

 //           //canvas.SetActive(false);
 //           NetworkServer.Destroy(canvas);
 //       }
        
 //   }
}
