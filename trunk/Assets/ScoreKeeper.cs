using UnityEngine;
using System.Collections;

//TODO: this object should be a singleton
public class ScoreKeeper : MonoBehaviour {
 
    public static float Score;
    public static bool GameOver = false, Won = false;
    
    private int scoreLimit;
    private Ruleset rules;
    private PauseMenu pauseMenu;
    
    void Awake()
    {
        rules = GameObject.Find("Ruleset").GetComponent<Ruleset>();
        pauseMenu = GameObject.Find("Main Camera").GetComponent<PauseMenu>();
        Score = 0;
        GameOver = false;
        Won = false;
        scoreLimit = rules.ScoreLimit;
    }
    
	// Use this for initialization
	void Start () {
	    rules = GameObject.Find("Ruleset").GetComponent<Ruleset>();
	}
	
	// Update is called once per frame
	void Update () {
        if(Time.timeScale > 0)
        {
            CheckWinCondition();
            if(GameOver)
            {
    //            if(Input.GetKeyDown(KeyCode.Space))
    //            {
    //                Time.timeScale = 1;
    //                GameOver = false;
    //                Won = false;
    //            }
                pauseMenu.PauseGame();
            }
        }
	}
    
    void OnGUI()
    {
        //TODO: better looking gui
        GUI.Label(new Rect(0, 0, 100, 25), "Score: " + Score);
    }
    
    private void CheckWinCondition()
    {
        if(Score >= scoreLimit)
        {
            GameOver = true;
            Won = true;
        }
    }
    
    public void GameLost()
    {
        GameOver = true;
        Won = false;
    }
}
