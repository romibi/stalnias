using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public Transform target;
    Camera cam;

    Vector3 camOffset = new Vector3(0,0,-10);

	// Use this for initialization
	void Start () {
        
        cam = GetComponent<Camera>();
		
	}
	
	// Update is called once per frame
	void Update () {
        cam.orthographicSize = (Screen.height / 100f) / 3f;
        if(target) {
            transform.position = Vector3.Lerp(transform.position, target.position+camOffset, 0.1f);
        }
	}
}
