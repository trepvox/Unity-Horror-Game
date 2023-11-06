using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelDisplay : MonoBehaviour {

	public Text levelText;

	public static int currentLevel = 1;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		// padding to the right since we have this live on the left side of screen.
		levelText.text = "Level " + currentLevel.ToString();
	}
}
