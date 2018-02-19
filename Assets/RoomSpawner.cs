using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSpawner : MonoBehaviour {
	
	public List <GameObject> roomsStraight = new List <GameObject>();
	public List <GameObject> roomsUp = new List <GameObject>();
	public List <GameObject> roomsDown = new List <GameObject>();
	public List <GameObject> specialUpRooms = new List <GameObject>();
	public List <GameObject> specialDownRooms = new List <GameObject>();

	public int roomUpPerc = 30;
	public int roomDownPerc = 70;
	public int roomAmount = 5;


	private int spawnPosX;
	private int spawnPosY;
	private GameObject nextRoom;
	//public int roomSize = 8;
	// Use this for initialization
	void Start () {
		//First Room
		
		nextRoom = PickRandomRoom();

		for (int i = 0; i < roomAmount; i++)
		{
			
			Instantiate (nextRoom, (Vector2)transform.position + new Vector2(spawnPosX, spawnPosY), Quaternion.identity);

			Room nextRoomProps = nextRoom.GetComponent<Room>();

			nextRoom = PickRandomRoom();
			
			switch (nextRoomProps.roomVariant)
			{
				case 0:
					spawnPosY += nextRoomProps.roomSizeY;
					nextRoom = specialUpRooms[Random.Range(0, specialUpRooms.Count)] as GameObject;
				break;

				case 1:
					spawnPosX += nextRoomProps.roomSizeX;
				break;

				case 2:
					spawnPosY -= nextRoomProps.roomSizeY;
					nextRoom = specialDownRooms[Random.Range(0, specialDownRooms.Count)] as GameObject;
				break;
			}
		}
	}
	
	private GameObject PickRandomRoom()
	{
		int randNum = Random.Range(0,100);

		//Check which room to spawn
		if (randNum <= roomUpPerc)
		{
			return roomsUp[Random.Range(0, roomsUp.Count)];
		}
		else if (randNum > roomUpPerc && randNum > roomDownPerc)
		{
			return roomsStraight[Random.Range(0, roomsStraight.Count)];
		}
		else
		{
			return roomsDown[Random.Range(0, roomsDown.Count)];
		}
	}
}
