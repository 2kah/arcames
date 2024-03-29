using UnityEngine;
using System.Collections;
using AssemblyCSharp;
using System;

public class EntityBehaviour : CollisionEntity {
	
    private MovementType movementType;
    private CollisionEffect[] collisionEffects;
    private CollisionEffect hitPlayer, hitRed, hitGreen, hitBlue;
    private int scorePlayer, scoreRed, scoreGreen, scoreBlue;
    private EntityType target;
    private GameObject player;
    private bool[] blocked;
    private Util util;
    private Ruleset rules;
    private float randomLongMaxTime = 3f;
    private float randomLongMinTime = 0.5f;
    private float randomLongEndTime = 0f;
	
	// Use this for initialization
	void Start () {
        rules = GameObject.Find("Ruleset").GetComponent<Ruleset>();
		player = GameObject.Find("Player");
		blocked = new bool[5];
        util = new Util();
        InvokeRepeating("UpdateMovementDirection", 0, 0.5f);
	}
    
    void Awake()
    {
        util = new Util();
        rules = GameObject.Find("Ruleset").GetComponent<Ruleset>();
        GetRules ();
    }
    
    private void GetRules()
    {
        collisionEffects = new CollisionEffect[Enum.GetNames(typeof(CollisionEffect)).Length];
        //TODO: better way of doing this - just use this.entitytype
        if(entityType == EntityType.Red)
        {
            speed = rules.RedSpeed;
            movementType = rules.RedMovement;
            target = rules.RedTarget;
            scorePlayer = rules.ScorePlayerRed;
            scoreRed = rules.ScoreRedRed;
            scoreGreen = rules.ScoreRedGreen;
            scoreBlue = rules.ScoreRedBlue;
        }
        else if(entityType == EntityType.Green)
        {
            speed = rules.GreenSpeed;
            movementType = rules.GreenMovement;
            target = rules.GreenTarget;
            scorePlayer = rules.ScorePlayerGreen;
            scoreRed = rules.ScoreRedGreen;
            scoreGreen = rules.ScoreGreenGreen;
            scoreBlue = rules.ScoreGreenBlue;
        }
        else if(entityType == EntityType.Blue)
        {
            speed = rules.BlueSpeed;
            movementType = rules.BlueMovement;
            target = rules.BlueTarget;
            scorePlayer = rules.ScorePlayerBlue;
            scoreRed = rules.ScoreRedBlue;
            scoreGreen = rules.ScoreGreenBlue;
            scoreBlue = rules.ScoreBlueBlue;
        }
        hitPlayer = rules.CollisionEffects[util.CollisionEffectsIndex(this.entityType, EntityType.Player)];
        hitRed = rules.CollisionEffects[util.CollisionEffectsIndex(this.entityType, EntityType.Red)];
        hitGreen = rules.CollisionEffects[util.CollisionEffectsIndex(this.entityType, EntityType.Green)];
        hitBlue = rules.CollisionEffects[util.CollisionEffectsIndex(this.entityType, EntityType.Blue)];
        this.currentMoveSpeed = this.speed;
    }
	
	// Update is called once per frame
	void Update () {
        if(this.pushList.Count > 0)
            UpdatePushDirection();
        
        //TODO: work out why util is sometimes null here
        Vector2 moveDirection = util.DirectionToVector(moving);
        moveDirection.Normalize();
        Vector3 newPos = rigidbody.position + (new Vector3(moveDirection.x, 0, moveDirection.y) * this.currentMoveSpeed * Time.deltaTime);
        rigidbody.MovePosition(newPos);
	}
    
    private void UpdatePushDirection()
    {
        //work out if the other entity is pushed by us
        CollisionEffect otherEffect = rules.CollisionEffects[util.CollisionEffectsIndex(this.pushList[0].entityType, this.entityType)];
        if(this.pushList.Count == 1)
        {
            CollisionEntity pusher = this.pushList[0];
            this.moving = pusher.moving;
            this.currentMoveSpeed = pusher.currentMoveSpeed;
        }
    }
    
    private void UpdateMovementDirection()
    {
        //if being pushed then override all other movement
        if(this.pushList.Count > 0)
            return;
        DetectBlockages();
        moving = GetMoveDirection();
    }
    
    private Direction GetMoveDirection()
    {
        switch(movementType)
        {
        case MovementType.Still:
            return Direction.None;
        case MovementType.RandomShort:
            return RandomDirection();
        case MovementType.RandomLong:
            if(Time.timeSinceLevelLoad >= randomLongEndTime)
            {
                randomLongEndTime = Time.timeSinceLevelLoad + UnityEngine.Random.Range(randomLongMinTime, randomLongMaxTime);
                return RandomDirection();
            }
            else
                return moving;
        case MovementType.Clockwise:
            return CircularMove(true);
        case MovementType.AntiClockwise:
            return CircularMove(false);
        case MovementType.Flee:
            return TargetMove(true);
        case MovementType.Chase:
            return TargetMove(false);
        default:
            return Direction.None;
        }
    }
    
    private Direction RandomDirection()
    {
        return (Direction) UnityEngine.Random.Range(1,5);
    }
    
    private Direction CircularMove(bool clockwise)
    {
        Direction desiredMove = moving;
        //for each of the 4 directions starting with the current move direction
        for(int i = 0; i < 4; i++)
        {
            //check if we can move in the given direction
            if(!blocked[(int)desiredMove])
                return desiredMove;
            else
                desiredMove = clockwise ? util.RightTurn(desiredMove) : util.LeftTurn(desiredMove);
        }
        return Direction.None;
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
        if(nearest == null)
            nearest = gameObject;
        
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
            ScoreKeeper.Score += scorePlayer;
            break;
        case EntityType.Red:
            effect = hitRed;
            ScoreKeeper.Score += ((float)scoreRed / 4);
            break;
        case EntityType.Green:
            effect = hitGreen;
            ScoreKeeper.Score += ((float)scoreGreen / 4);
            break;
        case EntityType.Blue:
            effect = hitBlue;
            ScoreKeeper.Score += ((float)scoreBlue / 4);
            break;
        }
        ResolveCollision(effect, otherObject);
	}
    
    private void ResolveCollision(CollisionEffect effect, CollisionEntity other)
    {
        switch(effect)
        {
        case CollisionEffect.None:
            return;
        case CollisionEffect.Teleport:
            Teleport ();
            return;
        case CollisionEffect.Death:
            Destroy(gameObject);
            return;
        case CollisionEffect.Push:
            Push(other);
            return;
        }
    }
    
    private void Push(CollisionEntity other)
    {
        other.pushList.Add(this);
    }
	
	private void Teleport()
	{
        transform.position = util.EmptyPosition(rules.MapWidth, rules.MapHeight);
        UpdateMovementDirection();
	}
}
