using UnityEngine;
using System.Collections.Generic;
using AssemblyCSharp;
using System;

public class MapManager : MonoBehaviour
{
    public Transform WallPiece, GroundPiece;
 
    private Util util;
    private Ruleset rules;
    private List<GameObject> mapPieces;
    
    // Use this for initialization
    void Start ()
    {
        mapPieces = new List<GameObject>();
        util = new Util();
        rules = GameObject.Find("Ruleset").GetComponent<Ruleset>();
        CreateMap(rules.MapWidth, rules.MapHeight, rules.MapData);
    }
    
    public void ChangeMap(int width, int height, bool[] mapData)
    {
        if(mapPieces.Count > 0)
        {
            foreach(var piece in mapPieces)
            {
                Destroy(piece);
            }
            mapPieces.Clear();
        }
        CreateMap(width, height, mapData);
    }
    
    private void CreateMap(int width, int height, bool[] mapData)
    {
        Map MapObject = new Map();
        GroundPiece.localScale = new Vector3(0.1f * width, 1, 0.1f * height);
        mapPieces.Add(((Transform)Instantiate(GroundPiece, Vector3.zero, Quaternion.identity)).gameObject);
        
        //work from top left, positive Y is upwards
        Vector2 bottomLeft = new Vector2(-((float)width-1)/2, -((float)height-1)/2);
        for(int i = 0; i < mapData.Length; i++)
        {
            if(rules.MapData[i])
            {
                Vector2 index2D = MapObject.To2DIndex(i, width);
                Vector3 wallPos = new Vector3(bottomLeft.x + index2D.x, 0.5f, bottomLeft.y + index2D.y);
                mapPieces.Add(((Transform)Instantiate(WallPiece, wallPos, Quaternion.identity)).gameObject);
            }
        }
        
        //create lights
        float lightHeight = (float) Math.Max(width, height) / 3;
        CreateLight(bottomLeft.x, lightHeight, bottomLeft.y);
        CreateLight(bottomLeft.x + width, lightHeight, bottomLeft.y + height);
    }
    
    private void CreateLight(float x, float y, float z)
    {
        GameObject lightGameObject = new GameObject();
        lightGameObject.AddComponent<Light>();
        lightGameObject.light.color = Color.white;
        lightGameObject.transform.position = new Vector3(x, y, z);
        lightGameObject.light.range = y * 4;
        lightGameObject.light.intensity = 4;
    }
    
    // Update is called once per frame
    void Update ()
    {
    
    }
}

