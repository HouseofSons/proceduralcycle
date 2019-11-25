﻿using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	private static int stageNumber;
	private static bool isPaused;

	//used for development purposes
	public string playerName;
	public string levelName;
	public string spawnName;

	//used to determine speed of player
	public float speedInput;
	private static float speed;

	//used to establish current player level and spawn
	private static GameObject currentPlayer;
	private static GameObject currentLevel;
	private static GameObject currentSpawn;
	private static GameObject gameCamera;
	private static GameObject gameManager;

	//State triggers
	private static bool moveToSpawnState;
	private static bool aimArrowState;
    private static bool playerMovingState;
    private static bool changeLevel;
	private static bool enableEnemyMovement;
    private static bool moveCamera;
    private static bool enterDoor;
	
	//used to attach visual arrow over player when aiming
	private static GameObject aimArrow;

	void Start () {

		gameManager = this.gameObject;
        gameCamera = GameObject.Find("MainCamera");

		stageNumber = 0;
		isPaused = false;

		moveToSpawnState = false;
		aimArrowState = false;
        playerMovingState = false;
        changeLevel = false;
		enableEnemyMovement = false;
        moveCamera = false;
		enterDoor = false;

		speed = speedInput;

		InitializePlayerInLevel(playerName,levelName,spawnName);
		InitializeCameraInLevel();

		aimArrow = currentPlayer.transform.Find ("AimArrow").gameObject;
		aimArrow.gameObject.GetComponent<SpriteRenderer> ().enabled = false;
	}

	void Update() {

		if (!isPaused) {
			currentPlayer.transform.LookAt(MouseLocation ());
		}

		if (stageNumber == 0) {
			//Game Started and Camera is in position
			moveToSpawnState = true;
			stageNumber = 1;
		}
		if (stageNumber == 1 && !moveToSpawnState) {
			//Player is in position
			aimArrowState = true;
			stageNumber = 2;
		}
		if (stageNumber == 2 && playerMovingState) {
			//Player has been released from original level spawn
			enableEnemyMovement = true;
			stageNumber = 3;
		}
		if (stageNumber == 3 && enterDoor) {
			//player switch level
			enterDoor = false;
			changeLevel = true;
			moveToSpawnState = true;
            aimArrowState = false;
            playerMovingState = false;
            enableEnemyMovement = false;
			stageNumber = 1;
		}
	
		//		moveToSpawnState
		//		aimArrowState
        //      playerMovingState
		//		changeLevel
		//		enableEnemyMovement
		//		moveCamera
		//		enterDoor
	}
	
	public static int StageNumber {
		get {return stageNumber;}
	}

	public static bool IsPaused {
		get {return isPaused;}
		set {isPaused = value;}
	}

	public static bool MoveToSpawnState {
		get {return moveToSpawnState;}
		set {moveToSpawnState = value;}
	}
	
	public static bool AimArrowState {
		get {return aimArrowState;}
		set {aimArrowState = value;}
    }

    public static bool PlayerMovingState
    {
        get { return playerMovingState; }
        set { playerMovingState = value; }
    }

    public static bool ChangeLevel {
		get {return changeLevel;}
		set {changeLevel = value;}
	}
	
	public static bool EnableEnemyMovement {
		get {return enableEnemyMovement;}
		set {enableEnemyMovement = value;}
    }

    public static bool MoveCamera
    {
        get { return moveCamera; }
        set { moveCamera = value; }
    }

    public static bool EnterDoor {
		get {return enterDoor;}
		set {enterDoor = value;}
	}
	
	public static float Speed {
		get {return speed;}
		set {speed = value;}
	}

	public static GameObject GetCurrentLevel() {
		return currentLevel;
	}
	
	public static GameObject GetCurrentSpawn() {
		return currentSpawn;
	}
	
	public static GameObject GetCurrentPlayer() {
		return currentPlayer;
	}

	public static GameObject GetCamera() {
		return gameCamera;
	}

	public static GameObject AimArrow() {
		return aimArrow;
	}

	private static void InitializePlayerInLevel(string player,string level, string spawn) {

		currentLevel = Instantiate (Resources.Load (level)) as GameObject;

		if (currentLevel != null) {
			currentSpawn = currentLevel.transform.Find (spawn).gameObject;

			if (currentSpawn != null) {
				if (currentPlayer == null) { //for level switching
					currentPlayer = Instantiate(Resources.Load(player)) as GameObject;
				}
				if (currentPlayer != null) {
					currentPlayer.GetComponent<Player>().SpawnPoint = currentSpawn.transform.position;
				}
			}
		}
	}

	private void InitializeCameraInLevel() {

		if (currentLevel != null) {
			gameCamera.gameObject.transform.position = currentLevel.transform.Find ("CameraPosition").gameObject.transform.position;
		}
	}

	public static void DoorHit(string doorName) {
		string levelName = "Level" + doorName.Substring (4, 3);
		string spawnName = "Spawn" + doorName.Substring (8, 2);
		InitializePlayerInLevel(currentPlayer.name,levelName,spawnName);
		Player.LevelStatsReset (1000);
		enterDoor = true;
	}

	public static Vector3 MouseLocation () {

		Plane plane=new Plane(Vector3.up, new Vector3(0,currentPlayer.transform.position.y,0));
		Ray ray=gameCamera.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
		float distance;
		if (plane.Raycast (ray, out distance)) {
			return ray.GetPoint (distance);
		} else {
			return currentPlayer.transform.position;
		}
	}
	//Shows Game Over Canvas
	public static void GameOver() {
		gameManager.GetComponent<InGameOptions> ().ShowGameOverMenu ();
	}
}









