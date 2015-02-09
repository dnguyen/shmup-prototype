using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	private float SPEED = 5f;
	private float BULLET_SPEED = 500.0f;
	private float FIRE_DELAY = 0.15f;
	private float PLAYER_ROTATION_SPEED = 5;
	private float lastShot = 0.0f;

	public GameObject firePosition;
	public GameObject bullet;

	// Use this for initialization
	void Start () {
	
	}
	// Update is called once per frame
	void Update () {
		Vector2 mousePosV2 = Camera.main.ScreenToViewportPoint (Input.mousePosition);
		Vector3 objPos = Camera.main.WorldToViewportPoint (transform.position);
		Vector2 relobjpos = new Vector2 (objPos.x - 0.5f, objPos.y - 0.5f);
		Vector2 relmousepos = new Vector2 (mousePosV2.x - 0.5f, mousePosV2.y - 0.5f) - relobjpos;
	
		float angle = Vector2.Angle (Vector2.up, relmousepos);
		if (relmousepos.x > 0)
			angle = 360 - angle;
		//Debug.Log ("lastShot=" + lastShot);

		Quaternion quat = Quaternion.identity;
		quat.eulerAngles = new Vector3 (0, 0, angle);
		//transform.rotation = quat;

		Quaternion rotDir = Quaternion.AngleAxis (transform.rotation.eulerAngles.z, Vector3.right);
		Vector3 ldir = rotDir * Vector3.forward;

		if (Input.GetKey (KeyCode.A) && Input.GetKey (KeyCode.W)) {
			Debug.Log("Go top left");
			rotDir = Quaternion.AngleAxis (45, Vector3.right);
			ldir = rotDir * Vector3.forward;
			transform.position = new Vector3(
				transform.position.x + ldir.normalized.y * SPEED * Time.deltaTime,
				transform.position.y + ldir.normalized.z * SPEED * Time.deltaTime
				);
		} else if (Input.GetKey (KeyCode.UpArrow) || Input.GetKey (KeyCode.W)) {
//			transform.position = new Vector3(
//				transform.position.x + relmousepos.normalized.x * SPEED * Time.deltaTime, 
//				transform.position.y + relmousepos.normalized.y * SPEED * Time.deltaTime,
//				0);
			transform.position = new Vector3(
				transform.position.x + ldir.normalized.y * SPEED * Time.deltaTime,
				transform.position.y + ldir.normalized.z * SPEED * Time.deltaTime
				);
		} else if (Input.GetKey (KeyCode.S)) {
			Vector3 modifiedDir = ldir * -1;
			transform.position = new Vector3(
				transform.position.x + modifiedDir.normalized.y * SPEED * Time.deltaTime,
				transform.position.y + modifiedDir.normalized.z * SPEED * Time.deltaTime
				);
		} else if (Input.GetKey (KeyCode.A)) {

			transform.position = new Vector3(
				transform.position.x - SPEED * Time.deltaTime, 
				transform.position.y,
				0);
		} else if (Input.GetKey (KeyCode.D)) {
			
			transform.position = new Vector3(
				transform.position.x + SPEED * Time.deltaTime, 
				transform.position.y,
				0);
		}

		if ((Input.GetMouseButton(0) || Input.GetKey (KeyCode.Space)) && lastShot >= FIRE_DELAY) {

			// We want bullets to come out of the gun barrel, not the player sprite.
			// Calculate velocity direction vector between mouse position and firePosition.
//			Vector3 gunpos = Camera.main.WorldToViewportPoint(firePosition.transform.position);
//			Vector2 directionalVector = new Vector2(mousePosV2.x - 0.5f, mousePosV2.y - 0.5f) - new Vector2(gunpos.x - 0.5f, gunpos.y - 0.5f);
//			//float fireAngle = Vector2.Angle (Vector2.up, directionalVector);
//			float fireAngle = Vector2.Angle (Vector2.up, new Vector2(ldir.y, ldir.z));
//			if (directionalVector.x > 0)
//				fireAngle = 360 - fireAngle;
//
//			Debug.Log("firePosition=" + firePosition.transform.position);
//			Debug.Log ("playerPosition=" + transform.position);
//			//float randAccuracy = Random.Range (-1.0f, 1.0f);
//			Quaternion bulletRotation = Quaternion.identity;
//			bulletRotation.eulerAngles = new Vector3 (0, 0, fireAngle);


			GameObject clone = Instantiate(bullet, firePosition.transform.position, Quaternion.identity) as GameObject;
			clone.transform.rotation = transform.rotation;
			clone.rigidbody2D.velocity = new Vector2(
				ldir.normalized.y * BULLET_SPEED * Time.deltaTime, 
				ldir.normalized.z * BULLET_SPEED * Time.deltaTime);

			lastShot = 0;

		}

		if (Input.GetKey (KeyCode.LeftArrow)) {
			Debug.Log (transform.rotation.eulerAngles.magnitude);
			Quaternion newRotation = Quaternion.identity;

			if (transform.rotation.eulerAngles.z >= 360) {
				newRotation.eulerAngles = new Vector3(0, 0, 0);
			} else {
				newRotation.eulerAngles = new Vector3(0, 0, transform.rotation.eulerAngles.z + PLAYER_ROTATION_SPEED);
			}
			transform.rotation = newRotation;

		} else if (Input.GetKey (KeyCode.RightArrow)) {
			Quaternion newRotation = Quaternion.identity;
			
			if (transform.rotation.eulerAngles.z <= 0) {
				newRotation.eulerAngles = new Vector3(0, 0, 360);
			} else {
				newRotation.eulerAngles = new Vector3(0, 0, transform.rotation.eulerAngles.z - PLAYER_ROTATION_SPEED);
			}
			transform.rotation = newRotation;
		}



		lastShot += Time.deltaTime;


	}
}
