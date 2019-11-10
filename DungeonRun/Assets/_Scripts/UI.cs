﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI : MonoBehaviour {

	private static GameObject experienceText;
	private static GameObject levelText;
	private static GameObject energyText;
	private static GameObject grapplingText;

	// Use this for initialization
	void Start () {
		energyText = GameObject.Find ("EnergyValue");
		grapplingText = GameObject.Find ("GrapplingValue");
		experienceText = GameObject.Find ("ExperienceValue");
		levelText = GameObject.Find ("LevelValue");
	}
	
	public static void InitializeUI() {
		energyText.GetComponent<Text> ().text = Player.Energy.ToString();
		grapplingText.GetComponent<Text> ().text = Player.GrappleCharges.ToString() + "/" + Player.MaxGrappleCharges.ToString();
		experienceText.GetComponent<Text> ().text = Player.TotalExperiencePoints.ToString();
		levelText.GetComponent<Text> ().text = Player.Level.ToString();
	}

	public static void UpdateEnergyText(int energy) {
		energyText.GetComponent<Text> ().text = energy.ToString();
	}
	
	public static void UpdateGrappleText(int grapCharges, int maxGrapCharges) {
		grapplingText.GetComponent<Text> ().text = grapCharges.ToString() + "/" + maxGrapCharges.ToString();
	}
	
	public static void UpdateExperienceText(int experience) {
		experienceText.GetComponent<Text> ().text = experience.ToString();
	}
	
	public static void UpdateLevelText(int level) {
		levelText.GetComponent<Text> ().text = level.ToString();
	}
}