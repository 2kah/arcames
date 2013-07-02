using UnityEngine;
using System.Collections;

public class ScoreKeeper : MonoBehaviour {
 
    public static int Score;
    
    private int scoreLimit;
    private Ruleset rules;
    private bool gameOver = false;
    
    void Awake()
    {
        Score = 0;
    }
    
	// Use this for initialization
	void Start () {
	    rules = GameObject.Find("Ruleset").GetComponent<Ruleset>();
        scoreLimit = rules.ScoreLimit;
	}
	
	// Update is called once per frame
	void Update () {
	    if(Score >= scoreLimit)
        {
            gameOver = true;
            Time.timeScale = 0;
            if(Input.GetKeyDown(KeyCode.Space))
            {
                Time.timeScale = 1;
                gameOver = false;
                Application.LoadLevel(0);
            }
        }
            
	}
    
    void OnGUI()
    {
        //TODO: display score better
        GUI.Label(new Rect(0, 0, 100, 25), "Score: " + Score);
        if(gameOver)
            GUI.Label(new Rect(50, 50, 100, 50), "You win!\nPress space to play again");
    }
}
