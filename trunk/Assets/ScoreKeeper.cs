using UnityEngine;
using System.Collections;

//TODO: this object should be a singleton
public class ScoreKeeper : MonoBehaviour {
 
    public static float Score;
    
    private int scoreLimit;
    private Ruleset rules;
    private bool gameOver = false, won = false;
    
    void Awake()
    {
        rules = GameObject.Find("Ruleset").GetComponent<Ruleset>();
        Score = 0;
        gameOver = false;
        won = false;
        scoreLimit = rules.ScoreLimit;
    }
    
	// Use this for initialization
	void Start () {
	    rules = GameObject.Find("Ruleset").GetComponent<Ruleset>();
	}
	
	// Update is called once per frame
	void Update () {
        CheckWinCondition();
        if(gameOver)
        {
            Time.timeScale = 0;
            if(Input.GetKeyDown(KeyCode.Space))
            {
                Time.timeScale = 1;
                gameOver = false;
                won = false;
                Application.LoadLevel(0);
            }
        } 
	}
    
    void OnGUI()
    {
        //TODO: better looking gui
        GUI.Label(new Rect(0, 0, 100, 25), "Score: " + Score);
        if(gameOver)
        {
            string message = "";
            if(won)
                message = "You win!\nPress space to play again";
            else
                message = "You lose!\nPress space to play again";
            GUI.Label(new Rect(50, 50, 100, 60), message);
        }
    }
    
    private void CheckWinCondition()
    {
        if(Score >= scoreLimit)
        {
            gameOver = true;
            won = true;
        }
    }
    
    public void GameLost()
    {
        gameOver = true;
        won = false;
    }
}
