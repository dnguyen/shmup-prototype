using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameOver : MonoBehaviour {

    public Text text;

	// Use this for initialization
	void Start () {
        text.text = "Levels completed: " + PlayerPrefs.GetInt("completedLevels");
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.Return)) {
            Application.LoadLevel("Menu");
        }
	}
}
