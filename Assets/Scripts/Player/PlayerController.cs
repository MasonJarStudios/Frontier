using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
	public Camera playerCamera;
	[SerializeField] AudioClip jumpSound;
	[SerializeField] AudioSource audioSource;
	CharacterController characterController;
	Vector3 rotationX, rotationY, verticalMovement, horizontalMovement;
	Vector3 characterVelocity;
	const float GRAVITY = 12f;
	const float CHARACTER_SPEED = 5f;
	const float JUMP_AMOUNT = 4f;
	const float SENSITIVITY = 1.2f;
	int isSprinting;
	void Start () {
		characterController = GetComponent<CharacterController> ();
		Screen.lockCursor = true;
		Cursor.visible = false;
	}

	void Update () {
		rotationY = new Vector3 (0f, Input.GetAxisRaw ("Mouse X"), 0f) * SENSITIVITY;
		rotationX = new Vector3 (Input.GetAxisRaw ("Mouse Y"), 0f, 0f) * -SENSITIVITY;
		verticalMovement = Input.GetAxisRaw ("Vertical") * transform.forward;
		horizontalMovement = Input.GetAxisRaw ("Horizontal") * transform.right;
		if (Input.GetKey(KeyCode.LeftShift)) {
			isSprinting = 1;
		} else { isSprinting = 0; }

		playerCamera.transform.Rotate (rotationX);
		gameObject.transform.Rotate (rotationY);

		characterVelocity.x = (verticalMovement.x + horizontalMovement.x) * CHARACTER_SPEED + isSprinting*3;
		characterVelocity.z = (verticalMovement.z + horizontalMovement.z) * CHARACTER_SPEED + isSprinting*3;


			
		if (characterController.isGrounded) {
			characterVelocity.y = 0;
		}

		if (Input.GetKeyDown (KeyCode.Space) && characterController.isGrounded) { 
				characterVelocity.y = JUMP_AMOUNT;
				audioSource.PlayOneShot(jumpSound);
			}
		else {
			characterVelocity.y -= GRAVITY * Time.deltaTime;
		}

		characterController.Move (characterVelocity * Time.deltaTime); 
	}
	void FixedUpdate () {

	}
}
