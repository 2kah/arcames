using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class EntityBehaviour : CollisionEntity {
	
	public float speed;
    
    private MovementType movementType;
    private CollisionEffect hitPlayer, hitRed, hitGreen, hitBlue;
    private int scorePlayer, scoreRed, scoreGreen, scoreBlue;
    private EntityType target;
    private GameObject player;
    private bool[] blocked;
    private Direction moving;
    private Util util;
    private Ruleset rules;
	
	// Use this for initialization
	void Start () {
        rules = GameObject.Find("Ruleset").GetComponent<Ruleset>();
		player = GameObject.Find("Player");
		blocked = new bool[5];
        util = new Util();
        GetRules();
        InvokeRepeating("UpdateMovementDirection", 0, 0.5f);
	}
    
    private void GetRules()
    {
        //TODO: better way of doing this
        if(entityType == EntityType.Red)
        {
            movementType = rules.RedMovement;
            hitPlayer = rules.RedPlayer;
            hitRed = rules.RedRed;
            hitGreen = rules.RedGreen;
            hitBlue = rules.RedBlue;
            target = rules.RedTarget;
            scorePlayer = rules.ScorePlayerRed;
            scoreRed = rules.ScoreRedRed;
            scoreGreen = rules.ScoreRedGreen;
            scoreBlue = rules.ScoreRedBlue;
        }
        else if(entityType == EntityType.Green)
        {
            movementType = rules.GreenMovement;
            hitPlayer = rules.GreenPlayer;
            hitRed = rules.GreenRed;
            hitGreen = rules.GreenGreen;
            hitBlue = rules.GreenBlue;
            target = rules.GreenTarget;
            scorePlayer = rules.ScorePlayerGreen;
            scoreRed = rules.ScoreRedGreen;
            scoreGreen = rules.ScoreGreenGreen;
            scoreBlue = rules.ScoreGreenBlue;
        }
        else if(entityType == EntityType.Blue)
        {
            movementType = rules.BlueMovement;
            hitPlayer = rules.BluePlayer;
            hitRed = rules.BlueRed;
            hitGreen = rules.BlueGreen;
            hitBlue = rules.BlueBlue;
            target = rules.BlueTarget;
            scorePlayer = rules.ScorePlayerBlue;
            scoreRed = rules.ScoreRedBlue;
            scoreGreen = rules.ScoreGreenBlue;
            scoreBlue = rules.ScoreBlueBlue;
        }
    }
	
	// Update is called once per frame
	void Update () {
        Vector2 moveDirection = util.DirectionToVector(moving);
        moveDirection.Normalize();
        Vector3 newPos = rigidbody.position + (new Vector3(moveDirection.x, 0, moveDirection.y) * speed * Time.deltaTime);
        rigidbody.MovePosition(newPos);
	}
    
    private void UpdateMovementDirection()
    {
        DetectBlockages();
        moving = GetMoveDirection();
    }
    
    private Direction GetMoveDirection()
    {
        switch(movementType)
        {
        case MovementType.Flee:
            return TargetMove(true);
        case MovementType.Chase:
            return TargetMove(false);
        default:
            return Direction.None;
        }
    }
    
    private Direction TargetMove(bool away)
    {
        Vector2 TargetPos = GetTargetPosition();
        Vector2 currentPos = new Vector2(transform.position.x, transform.position.z);
        Vector2 desiredVector;
        if(away)
            desiredVector = currentPos - TargetPos;
        else
            desiredVector = TargetPos - currentPos;
        Direction desiredDirection;
        //work out which direction we'd like to move in
        if(Mathf.Abs(desiredVector.x) >= Mathf.Abs(desiredVector.y))
        {
            //moving horizontally
            desiredDirection = desiredVector.x > 0 ? Direction.Right : Direction.Left;
        }
        else
        {
            //moving vertically
            desiredDirection = desiredVector.y > 0 ? Direction.Up : Direction.Down;
        }
        //work out if we can actually move in this direction
        if(!blocked[(int)desiredDirection])
            return desiredDirection;
        //we can't move the way we want to, so try perpendicular
        Direction secondChoice;
        Axis axis = util.GetAxis(desiredDirection);
        if(axis == Axis.Horizontal)
        {
            //trying to move horizontally, so try vertical
            secondChoice = desiredVector.y > 0 ? Direction.Up : Direction.Down;
        }
        else
        {
            //trying to move vertically, so try horizontal
            secondChoice = desiredVector.x > 0 ? Direction.Right : Direction.Left;
        }
        //work out if we can actually move in this direction
        if(!blocked[(int)secondChoice])
            return secondChoice;
        //we are stuck in a corner
        Direction thirdChoice = util.Opposite(secondChoice);
        return thirdChoice;
    }
    
    private Vector2 GetTargetPosition()
    {
        switch(target)
        {
        case EntityType.Player:
            return new Vector2(player.transform.position.x, player.transform.position.z);
        case EntityType.Red:
            return ClosestOfType("Red");
        case EntityType.Green:
            return ClosestOfType("Green");
        case EntityType.Blue:
            return ClosestOfType("Blue");
        default:
            return transform.position;
        }
    }
    
    private Vector2 ClosestOfType(string colour)
    {
        var taggedObjects = GameObject.FindGameObjectsWithTag(colour);
        GameObject nearest = null;
        var nearestDistance = Mathf.Infinity;
        
        foreach(GameObject obj in taggedObjects)
        {
            var objPos = obj.transform.position;
            var distSquared = (objPos - transform.position).sqrMagnitude;
            if(distSquared < nearestDistance)
            {
                nearest = obj;
                nearestDistance = distSquared;
            }
        }
        return new Vector2(nearest.transform.position.x, nearest.transform.position.z);
    }
    
    private void DetectBlockages()
    {
        //cast a ray right
        blocked[(int)Direction.Right] = CastRay(new Vector3(1,0.5f,0));
        //left
        blocked[(int)Direction.Left] = CastRay(new Vector3(-1,0.5f,0));
        //up
        blocked[(int)Direction.Up] = CastRay(new Vector3(0,0.5f,1));
        //down
        blocked[(int)Direction.Down] = CastRay(new Vector3(0,0.5f,-1));
    }
    
    private bool CastRay(Vector3 direction)
    {
        return Physics.Raycast(transform.position, direction, 0.55f, 1 << LayerMask.NameToLayer("Walls"));
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
        case EntityType.Player:
            effect = hitPlayer;
            //TODO: use score effects
            ScoreKeeper.Score += scorePlayer;
            break;
        case EntityType.Red:
            effect = hitRed;
            ScoreKeeper.Score += (scoreRed / 2);
            break;
        case EntityType.Green:
            effect = hitGreen;
            ScoreKeeper.Score += (scoreGreen / 2);
            break;
        case EntityType.Blue:
            effect = hitBlue;
            ScoreKeeper.Score += (scoreBlue / 2);
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
        //TODO: rest of the effects
        }
    }
	
	private void Teleport()
	{
        transform.position = util.EmptyPosition();
        UpdateMovementDirection();
	}
}
