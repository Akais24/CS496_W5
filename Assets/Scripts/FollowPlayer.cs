using UnityEngine;

public class FollowPlayer : MonoBehaviour {

    public Transform player;
    public Vector3 offset;
	
	// Update is called once per frame
	void Update () {

        //it attaches exactly where the player at, (inside of it)
        transform.position = player.position + offset;

        Debug.Log(player.position);
		
	}
}
