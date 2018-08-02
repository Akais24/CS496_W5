using UnityEngine;
using UnityEngine.Networking;

public class MinionSpawnManager : NetworkBehaviour
{
    public GameObject minion;
    public float spawnTime = 0.2f;
    public int side;

    public float spawninterval = 3f;
    public int onetime = 5;
    public Transform[] spawnPoints;

    private int count;
    private int index;

    public override void OnStartServer()
    {
        count = onetime;
        index = 0;
        InvokeRepeating("SpawnMinion", 0.1f, spawnTime);
    }


    void SpawnMinion()
    {
        GameObject newMinion = Instantiate(minion, spawnPoints[index].position, spawnPoints[index].rotation);
        newMinion.GetComponent<MinionState>().setSide(side);
        NetworkServer.Spawn(newMinion);
        
        if (++index == spawnPoints.Length) {
            index = 0;
        }

        if (--count == 0)
        {
            CancelInvoke("SpawnMinion");
            count = onetime;
            InvokeRepeating("SpawnMinion", spawninterval, spawnTime);
        }
    }
}
