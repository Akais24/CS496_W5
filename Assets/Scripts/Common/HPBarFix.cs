using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPBarFix : MonoBehaviour {

    private Camera my_camera;

    void Awake()
    {
        my_camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(transform.position + my_camera.transform.rotation * Vector3.back,
            my_camera.transform.rotation * Vector3.down);
    }
}
