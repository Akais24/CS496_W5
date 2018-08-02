using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class FixCamera : NetworkBehaviour {

    private Camera m_Camera;
    Vector3 offset;

    // Use this for initialization
    void Start () {
        if (!isLocalPlayer) return;

        m_Camera = Camera.main;
        offset = m_Camera.transform.position;
    }
	
	// Update is called once per frame
	void Update () {
        if (!isLocalPlayer) return;

        m_Camera.transform.position = transform.position + offset;
    }
}
