using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class EntityManager : MonoBehaviour {
	
	public Transform redPrefab, greenPrefab, bluePrefab;
	
    private Util util;
    private Ruleset rules;
	
	// Use this for initialization
	void Start () {
        rules = GameObject.Find("Ruleset").GetComponent<Ruleset>();
        util = new Util();
	}
    
    void Awake()
    {
        rules = GameObject.Find("Ruleset").GetComponent<Ruleset>();
        util = new Util();
        for(int i = 0; i < rules.NumRed; i++)
        {
            Instantiate(redPrefab, util.EmptySpawnPosition(), Quaternion.identity);
        }
        for(int i = 0; i < rules.NumGreen; i++)
        {
            Instantiate(greenPrefab, util.EmptySpawnPosition(), Quaternion.identity);
        }
        for(int i = 0; i < rules.NumBlue; i++)
        {
            Instantiate(bluePrefab, util.EmptySpawnPosition(), Quaternion.identity);
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
