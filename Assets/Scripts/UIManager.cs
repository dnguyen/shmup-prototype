using UnityEngine;
using System.Collections;

public class UIManager : MonoBehaviour {
    public void Start() {
        Screen.showCursor = true;
    }
    public void StartGame() {
        Application.LoadLevel("Main");
    }
}
