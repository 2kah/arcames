using UnityEngine;
using System.Collections;
using AssemblyCSharp;


public class Player : CollisionEntity {
	
	public float speed;
	
	private Vector2 movementDir = new Vector2(0f,0f);
	private Vector2[] movements = new Vector2[5];
	
	// Use this for initialization
	void Start () {
		movements[(int)Direction.None] = new Vector2(0,0);
		movements[(int)Direction.Up] = new Vector2(0,1);
		movements[(int)Direction.Right] = new Vector2(1,0);
		movements[(int)Direction.Down] = new Vector2(0,-1);
		movements[(int)Direction.Left] = new Vector2(-1,0);
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.anyKeyDown)
		{
			if(Input.GetKeyDown(KeyCode.W))
				movementDir = movements[(int)Direction.Up];
			else if(Input.GetKeyDown(KeyCode.S))
				movementDir = movements[(int)Direction.Down];
			else if(Input.GetKeyDown(KeyCode.D))
				movementDir = movements[(int)Direction.Right];
			else if(Input.GetKeyDown(KeyCode.A))
				movementDir = movements[(int)Direction.Left];
		}
		else if(KeyUp())
		{
			if(Input.GetKey(KeyCode.W))
				movementDir = movements[(int)Direction.Up];
			else if(Input.GetKey(KeyCode.S))
				movementDir = movements[(int)Direction.Down];
			else if(Input.GetKey(KeyCode.D))
				movementDir = movements[(int)Direction.Right];
			else if(Input.GetKey(KeyCode.A))
				movementDir = movements[(int)Direction.Left];
			else
				movementDir = movements[(int)Direction.None];
		}
		
		Vector2 newPos = new Vector2();
		newPos = (movementDir * speed * Time.deltaTime);
		Vector3 newPosition = new Vector3(newPos.x, 0f, newPos.y);
		var controller = GetComponent<CharacterController>();
		controller.Move(newPosition);
		//transform.Translate(newPosition);
	}
	
	private bool KeyUp()
	{
		if(Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.A))
			return true;
		return false;
	}
}
