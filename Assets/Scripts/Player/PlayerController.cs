using System.Collections; 
using System.Collections.Generic; 
using UnityEngine; 
 
public class PlayerController : MonoBehaviour { 
	public Camera playerCamera; 
	[SerializeField] private AudioClip jumpSound; 
	[SerializeField] private AudioSource audioSource; 
	private CharacterController characterController; 
	private ArrayList scoreBoard;
	private Vector3 rotationX, rotationY, verticalMovement, horizontalMovement; 
	private Vector3 characterVelocity; 
	private float characterSpeed;
	private const float GRAVITY = 12f; 
	private const float JUMP_AMOUNT = 4f; 
	private const float SENSITIVITY = 1.2f; 
	private const int RUN_SPEED = 3;
	private int isSprinting; 
	private bool atCart;
	private Character player;
	private PlayerGUI gui;
	private GameController gameController;
	Dictionary<int, Teams> userTeamDict;
	Texture2D pixel;
	Color pixelColor;
	void Start() { 
		characterController = GetComponent<CharacterController>(); 
		Screen.lockCursor = true; 
		Cursor.visible = false;
		characterSpeed = gameObject.GetComponent<Character>().getSpeed();
		player = gameObject.GetComponent<Character> ();
		gui = gameObject.GetComponentInChildren<PlayerGUI> ();
		gameController = GameObject.FindWithTag("Control").GetComponent<GameController>();
		pixelColor = Color.black;
		pixelColor.a = 0.5f;
		pixel = new Texture2D (1, 1);
		pixel.SetPixel (0, 0, pixelColor);
		pixel.Apply ();
	}
 
	void Update() { 
		rotationY = new Vector3(0f, Input.GetAxisRaw("Mouse X"), 0f) * SENSITIVITY; 
		rotationX = new Vector3(Input.GetAxisRaw("Mouse Y"), 0f, 0f) * -SENSITIVITY; 
		verticalMovement = Input.GetAxisRaw("Vertical") * transform.forward; 
		horizontalMovement = Input.GetAxisRaw("Horizontal") * transform.right; 
		if (Input.GetKey(KeyCode.LeftShift)) { 
			isSprinting = 1; 
		}	else { isSprinting = 0; } 
 
		playerCamera.transform.Rotate(rotationX); 
		gameObject.transform.Rotate(rotationY); 
		characterVelocity.x = (verticalMovement.x + horizontalMovement.x) * (characterSpeed + isSprinting * RUN_SPEED); 
		characterVelocity.z = (verticalMovement.z + horizontalMovement.z) * (characterSpeed + isSprinting * RUN_SPEED); 
 
 
			 
		if (characterController.isGrounded) { 
			characterVelocity.y = 0; 
		} 
 
		if (Input.GetKeyDown(KeyCode.Space) && characterController.isGrounded) { 
				characterVelocity.y = JUMP_AMOUNT; 
				audioSource.PlayOneShot(jumpSound); 
		} 
		else { 
			characterVelocity.y -= GRAVITY * Time.deltaTime; 
		} 
 
		characterController.Move(characterVelocity * Time.deltaTime); 

	}
	private void OnGUI() {
        if (Input.GetKey (KeyCode.Tab)) {
            scoreBoard = gameController.getScores ();
            userTeamDict = gameController.getUserTeamDict();
            float screenWidth = Screen.width;
            float screenHeight = Screen.height;
            string playerCell;
            int teamCount = 0;
			GUI.DrawTexture (new Rect (screenWidth/8 - screenWidth/100, screenHeight/4, 3*screenWidth/4 + screenWidth/100, screenHeight/2), pixel);
			foreach (List<KeyValuePair<int, Teams.Stats>> teamScores in scoreBoard) {
                int playerCount = 1;
                GUI.Label(new Rect ((screenWidth/8) + (3*screenWidth/16 * teamCount), screenHeight/4, 3*screenWidth/16, screenHeight/12), "Team " + (teamCount + 1));
                foreach (KeyValuePair<int, Teams.Stats> playerScores in teamScores) {
					Player player = userTeamDict [playerScores.Key].findPlayerByUserId (playerScores.Key);
					playerCell = "[" + Global.CHARACTER_NAMES[player.getClassType()].ToUpper() + "] " + player.getUsername () + ": "
						+ playerScores.Value.kills + ", " + playerScores.Value.assists + ", " + playerScores.Value.deaths + ", " + playerScores.Value.goldStolen;
                    GUI.Label(new Rect ((screenWidth/8) + (3*screenWidth/16 * teamCount), (screenHeight/4) + (screenHeight/12 * playerCount), 3*screenWidth/16, screenHeight/12), playerCell);
                    playerCount++;
                }
              teamCount++;
          }
      }
  }
	public void setSpeed (float speed) {
		characterSpeed = speed;
	}
} 
