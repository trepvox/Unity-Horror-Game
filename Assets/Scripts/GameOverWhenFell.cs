using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class GameOverWhenFell : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		// if character position is less or = to -5 on the Y axis, they must've fallen in a hole and trigger a game over screen.
		if (transform.position.y <= -5) {

			SceneManager.LoadScene("GameOver");

			// Have to find and stop the whisper sound due to it being set to not be destroyed as talked about in the lecture.
			var whisperSource = GameObject.Find("WhisperSource");
			GameObject.Destroy(whisperSource);
		}
	}
}
