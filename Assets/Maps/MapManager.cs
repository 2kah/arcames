using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class MapManager : MonoBehaviour
{
    public Transform WallPiece, GroundPiece;
 
    private Util util;
    private Ruleset rules;
    private Map MapObject;
    
    // Use this for initialization
    void Start ()
    {
        util = new Util();
        rules = GameObject.Find("Ruleset").GetComponent<Ruleset>();
        MapObject = rules.MapObject;
        CreateMap();
    }
    
    public void CreateMap()
    {
        GroundPiece.localScale = new Vector3(0.1f * MapObject.Width, 1, 0.1f * MapObject.Height);
        Instantiate(GroundPiece, Vector3.zero, Quaternion.identity);
        
        //work from top left, positive Y is downwards
        Vector2 topLeft = new Vector2(-((float)MapObject.Width-1)/2, -((float)MapObject.Height-1)/2);
        for(int x = 0; x < MapObject.Width; x++)
        {
            for(int y = 0; y < MapObject.Height; y++)
            {
                if(MapObject.MapData[x,y])
                {
                    Vector3 wallPos = new Vector3(topLeft.x + x, 0.5f, topLeft.y + y);
                    Instantiate(WallPiece, wallPos, Quaternion.identity);
                }
            }
        }
    }
    
    // Update is called once per frame
    void Update ()
    {
    
    }
}

