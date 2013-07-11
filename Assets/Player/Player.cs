using UnityEngine;
using System.Collections;
using AssemblyCSharp;


public class Player : CollisionEntity {
	
	public float speed;
	
	private Vector2 movementDir = new Vector2(0f,0f);
    private CollisionEffect hitRed, hitGreen, hitBlue;
    private Ruleset rules;
    private ScoreKeeper scoreKeeper;
    private Util util;
	
	// Use this for initialization
	void Start () {
        util = new Util();
        scoreKeeper = GameObject.Find("ScoreKeeper").GetComponent<ScoreKeeper>();
        rules = GameObject.Find("Ruleset").GetComponent<Ruleset>();
	}
    
    void Awake()
    {
        rules = GameObject.Find("Ruleset").GetComponent<Ruleset>();
        hitRed = rules.PlayerRed;
        hitGreen = rules.PlayerGreen;
        hitBlue = rules.PlayerBlue;
    }
	
	// Update is called once per frame
	void Update () {
        //TODO: use input axis
		if(Input.anyKeyDown)
		{
			if(Input.GetKeyDown(KeyCode.W))
				movementDir = util.DirectionToVector(Direction.Up);
			else if(Input.GetKeyDown(KeyCode.S))
				movementDir = util.DirectionToVector(Direction.Down);
			else if(Input.GetKeyDown(KeyCode.D))
				movementDir = util.DirectionToVector(Direction.Right);
			else if(Input.GetKeyDown(KeyCode.A))
				movementDir = util.DirectionToVector(Direction.Left);
		}
		else if(KeyUp())
		{
			if(Input.GetKey(KeyCode.W))
				movementDir = util.DirectionToVector(Direction.Up);
			else if(Input.GetKey(KeyCode.S))
				movementDir = util.DirectionToVector(Direction.Down);
			else if(Input.GetKey(KeyCode.D))
				movementDir = util.DirectionToVector(Direction.Right);
			else if(Input.GetKey(KeyCode.A))
				movementDir = util.DirectionToVector(Direction.Left);
			else
				movementDir = util.DirectionToVector(Direction.None);
		}
		
		Vector2 newPos = new Vector2();
		newPos = (movementDir * speed * Time.deltaTime);
		Vector3 newPosition = new Vector3(newPos.x, 0f, newPos.y);
		var controller = GetComponent<CharacterController>();
		controller.Move(newPosition);
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
        transform.position = util.EmptyPosition();
    }
	
	private bool KeyUp()
	{
		if(Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.A))
			return true;
		return false;
	}
}