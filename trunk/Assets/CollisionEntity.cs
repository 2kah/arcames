using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using AssemblyCSharp;

public class CollisionEntity : MonoBehaviour
{
	public EntityType entityType;
    
    public float speed;
    public Direction moving;
    
    public List<CollisionEntity> pushList;

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
}

