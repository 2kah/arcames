using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class MapManager : MonoBehaviour
{
    public Transform WallPiece, GroundPiece;
 
    private Util util;
    private Ruleset rules;
    
    // Use this for initialization
    void Start ()
    {
        util = new Util();
        rules = GameObject.Find("Ruleset").GetComponent<Ruleset>();
        CreateMap();
    }
    
    public void CreateMap()
    {
        Map MapObject = new Map();
        GroundPiece.localScale = new Vector3(0.1f * rules.MapWidth, 1, 0.1f * rules.MapHeight);
        Instantiate(GroundPiece, Vector3.zero, Quaternion.identity);
        
        //TODO: make positive Y upwards
        //work from top left, positive Y is downwards
        Vector2 topLeft = new Vector2(-((float)rules.MapWidth-1)/2, -((float)rules.MapHeight-1)/2);
        for(int i = 0; i < rules.MapData.Length; i++)
        {
            if(rules.MapData[i])
            {
                Vector2 index2D = MapObject.To2DIndex(i, rules.MapWidth);
                Vector3 wallPos = new Vector3(topLeft.x + index2D.x, 0.5f, topLeft.y + index2D.y);
                Instantiate(WallPiece, wallPos, Quaternion.identity);
            }
        }
        
//        for(int x = 0; x < MapObject.Width; x++)
//        {
//            for(int y = 0; y < MapObject.Height; y++)
//            {
//                //todo: handle that this is now 1D
//                if(MapObject.MapData[x,y])
//                {
//                    Vector3 wallPos = new Vector3(topLeft.x + x, 0.5f, topLeft.y + y);
//                    Instantiate(WallPiece, wallPos, Quaternion.identity);
//                }
//            }
//        }
    }
    
    // Update is called once per frame
    void Update ()
    {
    
    }
}

