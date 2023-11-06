using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneOnInput : MonoBehaviour {

	// tells what scene to load and not auto start on a certain one since we need to loop with the game over screen
	public string whatSceneToLoad;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetAxis("Submit") == 1) {
			//Reset the current level
			//LevelDisplay.currentLevel = 1;
			//SceneManager.LoadScene("Play");
			SceneManager.LoadScene(whatSceneToLoad);
		}
	}
}
