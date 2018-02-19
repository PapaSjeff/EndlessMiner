using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour {

	public enum RoomType {Straight, Up, Down};
	public RoomType roomType = new RoomType();
	public int roomVariant; //0 is up, 1 is straight, 2 is down

	public int roomSizeX;
	public int roomSizeY;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
