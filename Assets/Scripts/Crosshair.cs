using UnityEngine;
using System.Collections;

public class Crosshair : MonoBehaviour {
    public Texture2D crosshair;
    // Use this for initialization
    void Start() {

    }

    void OnGUI() {
        GUI.DrawTexture(new Rect(Input.mousePosition.x - crosshair.width / 2, Screen.height - Input.mousePosition.y - crosshair.height / 2, crosshair.width, crosshair.height), crosshair);
    }
    // Update is called once per frame
    void Update() {
        //this.transform.position = Camera.main.ScreenToViewportPoint(Input.mousePosition);
    }
}
