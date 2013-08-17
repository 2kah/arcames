using UnityEngine;
using System.Collections;
using AssemblyCSharp;


public class Player : CollisionEntity {
	
    private CollisionEffect hitRed, hitGreen, hitBlue;
    private Ruleset rules;
    private ScoreKeeper scoreKeeper;
    private Util util;
    private CharacterController controller;
	
	// Use this for initialization
	void Start () {
        util = new Util();
        scoreKeeper = GameObject.Find("ScoreKeeper").GetComponent<ScoreKeeper>();
        rules = GameObject.Find("Ruleset").GetComponent<Ruleset>();
        controller = GetComponent<CharacterController>();
	}
    
    void Awake()
    {
        rules = GameObject.Find("Ruleset").GetComponent<Ruleset>();
        hitRed = rules.PlayerRed;
        hitGreen = rules.PlayerGreen;
        hitBlue = rules.PlayerBlue;
        speed = rules.PlayerSpeed;
    }
	
	// Update is called once per frame
	void Update () {
        //TODO: use input axis
		if(Input.anyKeyDown)
		{
			if(Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
				moving = Direction.Up;
			else if(Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
				moving = Direction.Down;
			else if(Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
				moving = Direction.Right;
			else if(Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
				moving = Direction.Left;
		}
		else if(KeyUp())
		{
			if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
				moving = Direction.Up;
			else if(Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
				moving = Direction.Down;
			else if(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
				moving = Direction.Right;
			else if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
				moving = Direction.Left;
			else
				moving = Direction.None;
		}
		
        Vector2 moveDirection = util.DirectionToVector(moving);
        moveDirection.Normalize();
        Vector3 newPos = new Vector3(moveDirection.x, 0, moveDirection.y) * speed * Time.deltaTime;
		controller.Move(newPos);
		//transform.Translate(newPosition);
	}
    
    void OnTriggerEnter(Collider other)
    {
        var otherObject = other.gameObject.GetComponent<CollisionEntity>();
        if(otherObject == null)
            return;
        EntityType otherType = otherObject.entityType;
        CollisionEffect effect = CollisionEffect.None;
        switch(otherType)
        {
        case EntityType.Red:
            effect = hitRed;
            break;
        case EntityType.Green:
            effect = hitGreen;
            break;
        case EntityType.Blue:
            effect = hitBlue;
            break;
        }
        ResolveCollision(effect);
    }
    
    private void ResolveCollision(CollisionEffect effect)
    {
        switch(effect)
        {
        case CollisionEffect.None:
            return;
        case CollisionEffect.Teleport:
            Teleport ();
            return;
        case CollisionEffect.Death:
            scoreKeeper.GameLost();
            return;
        //TODO: rest of the effects
        }
    }
        
    private void Teleport()
    {
        transform.position = util.EmptyPosition(rules.MapWidth, rules.MapHeight);
    }
	
	private bool KeyUp()
	{
		if(Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.DownArrow) || Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow))
			return true;
		return false;
	}
}
