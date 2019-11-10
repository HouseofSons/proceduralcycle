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
	private static GameObject currentSection;
	private static GameObject gameCamera;
	private static GameObject gameManager;

	//State triggers
	private static bool moveToSpawnState;
	private static bool aimArrowState;
	private static bool changeLevel;
	private static bool enableEnemyMovement;
	private static bool moveCamera;
	private static bool grapplingHookReady;
	private static bool grappleShot;
	private static bool grappleShotComplete;
	private static bool grappleNotCaught;
	private static bool grappleNotCaughtComplete;
	private static bool grappleCaught;
	private static bool grappleCaughtComplete;
	private static bool enterDoor;
	
	//used to attach visual arrow over player when aiming
	private static GameObject aimArrow;
	//Grappling Hooks are awesome!
	private static GameObject grapplingHook;

	void Start () {

		gameManager = this.gameObject;

		stageNumber = 0;
		isPaused = false;

		moveToSpawnState = false;
		aimArrowState = false;
		changeLevel = false;
		enableEnemyMovement = false;
		moveCamera = false;
		grapplingHookReady = false;
		grappleShot = false;
		grappleShotComplete = false;
		grappleNotCaughtComplete = false;
		grappleNotCaught = false;
		grappleCaught = false;
		grappleCaughtComplete = false;
		enterDoor = false;

		speed = speedInput;

		InitializePlayerInLevel(playerName,levelName,spawnName);
		InitializeCameraInLevel();

		aimArrow = currentPlayer.transform.Find ("AimArrow").gameObject;
		aimArrow.gameObject.GetComponent<SpriteRenderer> ().enabled = false;

		grapplingHook = currentPlayer.transform.Find ("GrapplingHook").gameObject;
		grapplingHook.gameObject.GetComponent<MeshRenderer> ().enabled = false;
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
		if (stageNumber == 2 && !aimArrowState) {
			//Player has been released from original level spawn
			enableEnemyMovement = true;
			grapplingHookReady = true;
			stageNumber = 3;
		}
		if (stageNumber == 3 && grappleNotCaught) {
			//Player shot Grappling Hook and missed and is reeling in
			grappleNotCaught = false;
			stageNumber = 4;
		}
		if (stageNumber == 4 && grappleNotCaughtComplete) {
			//Grappling Hook has been reeled in
			grappleNotCaughtComplete = false;
			grapplingHookReady = true;
			stageNumber = 3;
		}
		if (stageNumber == 3 && grappleCaught) {	
			//Player shot Grappling Hook and it caught
			grappleCaught = false;
			StopCoroutine(Player.PlayerFollowPathCoRoutine);
			stageNumber = 5;
		}
		if (stageNumber == 5 && grappleCaughtComplete) {
			//Player stuck at Grapple Position
			grappleCaughtComplete = false;
			grapplingHookReady = false;
			aimArrowState = true;
			stageNumber = 6;
		}
		if (stageNumber == 6 && !aimArrowState) {
			//Player released from Grapple position
			grapplingHookReady = true;
			stageNumber = 3;
		}
		if (stageNumber == 3 && enterDoor) {
			//player switch level
			enterDoor = false;
			changeLevel = true;
			moveToSpawnState = true;
			moveCamera = true;
			enableEnemyMovement = false;
			grapplingHookReady = false;
			stageNumber = 1;
		}
	
		//		moveToSpawnState
		//		aimArrowState
		//		changeLevel
		//		enableEnemyMovement
		//		moveCamera
		//		grapplingHookReady
		//		grappleShot
		//		grappleShotComplete
		//		grappleNotCaught
		//		grappleNotCaughtComplete
		//		grappleCaught
		//		grappleCaughtComplete
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
	
	public static bool ChangeLevel {
		get {return changeLevel;}
		set {changeLevel = value;}
	}
	
	public static bool EnableEnemyMovement {
		get {return enableEnemyMovement;}
		set {enableEnemyMovement = value;}
	}
	
	public static bool MoveCamera {
		get {return moveCamera;}
		set {moveCamera = value;}
	}
	
	public static bool GrapplingHookReady {
		get {return grapplingHookReady;}
		set {grapplingHookReady = value;}
	}
	
	public static bool GrappleShot {
		get {return grappleShot;}
		set {grappleShot = value;}
	}
	
	public static bool GrappleShotComplete {
		get {return grappleShotComplete;}
		set {grappleShotComplete = value;}
	}
	
	public static bool GrappleNotCaught {
		get {return grappleNotCaught;}
		set {grappleNotCaught = value;}
	}
	
	public static bool GrappleNotCaughtComplete {
		get {return grappleNotCaughtComplete;}
		set {grappleNotCaughtComplete = value;}
	}
	
	public static bool GrappleCaught {
		get {return grappleCaught;}
		set {grappleCaught = value;}
	}

	public static bool GrappleCaughtComplete {
		get {return grappleCaughtComplete;}
		set {grappleCaughtComplete = value;}
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
	
	public static GameObject CurrentSection {
		get{return currentSection;}
		set{currentSection = value;}
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
	
	public static GameObject GrapplingHook() {
		return grapplingHook;
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
				Section.InitializeSectionsOfCurrentLevel();;
			}
		}
	}

	private void InitializeCameraInLevel() {

		if (currentLevel != null) {
			gameCamera = Instantiate (Resources.Load ("Camera")) as GameObject;
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
	//Determines players Y position
	public static float YPositionPlayer(Vector3 pos) {
		
		float yPos = pos.y;
		RaycastHit hit;
		if (Physics.Raycast (new Vector3(pos.x,pos.y+1.0f,pos.z), Vector3.down, out hit, 1000.0f, 1 << LayerMask.NameToLayer ("Floor"))) {
			yPos = hit.point.y+1.0f;
			if (!hit.transform.gameObject.name.StartsWith("Ramp")) {
				currentSection = hit.transform.parent.gameObject;//used to determine which floor player is on
			} else {
				Section.LoadSections(hit.transform.gameObject);//used to identify all sections touched by ramp
			}
		}
		return yPos;
	}
	//Shows Game Over Canvas
	public static void GameOver() {
		gameManager.GetComponent<InGameOptions> ().ShowGameOverMenu ();
	}
}








