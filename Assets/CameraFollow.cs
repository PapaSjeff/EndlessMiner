using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

	public GameObject target;
	public float lookForward = 2f;
	public float lookUp = 1f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		transform.position = new Vector3(target.transform.position.x + lookForward, target.transform.position.y + lookUp, -10);
	}
}
