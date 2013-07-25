using UnityEngine;
using System.Collections;
using AssemblyCSharp;
using System;
using System.IO;

//http://wiki.unity3d.com/index.php?title=PauseMenu
public class PauseMenu : MonoBehaviour
{
 
    public GUISkin Skin;
    
    private float startTime = 0.1f;
    
    private float savedTimeScale;
    private Ruleset ruleset;
    private Util util;
    private Rules[] inbuiltRules;
    private Vector2 scrollPosition;
    private string importXml = "";
    private Rules editRules;
    private string scoreLimit, numRed, numGreen, numBlue, scorePlayerRed, scorePlayerGreen, scorePlayerBlue, scoreRedRed, scoreRedGreen, scoreRedBlue, scoreGreenGreen, scoreGreenBlue, scoreBlueBlue;
    private string playerSpeed, redSpeed, greenSpeed, blueSpeed;
    private string[] movementTypes, entityTypes, collisionEffects;
    
    public GUIStyle redBackground, greenBackground, blueBackground;
    
    public GameObject start;
 
    public enum Page {
        None,Main,Choose,Edit,Export,Import
    }
 
    private Page currentPage;
    
//    void Awake()
//    {
//        DontDestroyOnLoad(this);
//    }
    
    void Start() {
        util = new Util();
        ruleset = GameObject.Find("Ruleset").GetComponent<Ruleset>();
        Time.timeScale = 1;
        PauseGame();
        inbuiltRules = new Rules[util.InbuiltRules.Count];
        for(int i = 0; i < inbuiltRules.Length; i++)
        {
            Type rulesType = util.InbuiltRules[i];
            inbuiltRules[i] = (Rules)Activator.CreateInstance(rulesType);
        }
        movementTypes = Enum.GetNames(typeof(MovementType));
        entityTypes = Enum.GetNames(typeof(EntityType));
        collisionEffects = Enum.GetNames(typeof(CollisionEffect));
        scrollPosition = Vector2.zero;
    }
 
    static bool IsDashboard() {
        return Application.platform == RuntimePlatform.OSXDashboardPlayer;
    }
 
    static bool IsBrowser() {
        return (Application.platform == RuntimePlatform.WindowsWebPlayer ||
            Application.platform == RuntimePlatform.OSXWebPlayer);
    }
 
    void LateUpdate () {
 
        if (Input.GetKeyDown("escape")) 
        {
            switch (currentPage) 
            {
            case Page.None: 
                PauseGame();
                break;
 
            case Page.Main: 
                if (!IsBeginning()) 
                    UnPauseGame();
                break;
 
            default: 
                currentPage = Page.Main;
                break;
            }
        }
    }

    void OnGUI () {
        GUI.skin = Skin;
        if (IsGamePaused()) {
            GUI.Box(new Rect(0,0,Screen.width,Screen.height),"");
            switch (currentPage) {
            case Page.Main: MainPauseMenu(); break;
			case Page.Choose: ShowChoose(); break;
            case Page.Edit: ShowEdit(); break;
            case Page.Export: ShowExport(); break;
            case Page.Import: ShowImport(); break;
            }
        }
    }
	
	void ShowChoose()
	{
		BeginPage();
//		DirectoryInfo di = new DirectoryInfo(System.Environment.CurrentDirectory);
//        FileInfo[] files = di.GetFiles("*.xml");
//        
//        foreach(FileInfo file in files)
//        {
//            if(GUILayout.Button(file.Name))
//            {
//                util.LoadRuleset(file.Name);
//                Application.LoadLevel(0);
//                currentPage = Page.Main;
//            }
//        }
        
        foreach(Rules rules in inbuiltRules)
        {
            if(GUILayout.Button(rules.Name))
            {
                util.CopyFromRules(rules);
                Application.LoadLevel(0);
            }
        }
        
		EndPage();
	}
    
    void SetupEdit()
    {
        editRules = util.CopyToRules();
        playerSpeed = editRules.PlayerSpeed.ToString();
        scoreLimit = editRules.ScoreLimit.ToString();
        numRed = editRules.NumRed.ToString();
        numGreen = editRules.NumGreen.ToString();
        numBlue = editRules.NumBlue.ToString();
        redSpeed = editRules.RedSpeed.ToString();
        greenSpeed = editRules.GreenSpeed.ToString();
        blueSpeed = editRules.BlueSpeed.ToString();
        scorePlayerRed = editRules.ScorePlayerRed.ToString();
        scorePlayerGreen = editRules.ScorePlayerGreen.ToString();
        scorePlayerBlue = editRules.ScorePlayerBlue.ToString();
        scoreRedRed = editRules.ScoreRedRed.ToString();
        scoreRedGreen = editRules.ScoreRedGreen.ToString();
        scoreRedBlue = editRules.ScoreRedBlue.ToString();
        scoreGreenGreen = editRules.ScoreGreenGreen.ToString();
        scoreGreenBlue = editRules.ScoreGreenBlue.ToString();
        scoreBlueBlue = editRules.ScoreBlueBlue.ToString();
    }
    
    void ShowEdit()
    {
        BeginPage();
        scrollPosition = GUILayout.BeginScrollView(scrollPosition);
        
        BeginEditControls("Name",null);
        editRules.Name = GUILayout.TextField(editRules.Name);
        GUILayout.EndHorizontal();
        
        BeginEditControls("Description",null);
        editRules.Description = GUILayout.TextArea(editRules.Description);
        GUILayout.EndHorizontal();
        
        BeginEditControls("Player Speed",null);
        playerSpeed = GUILayout.TextField(playerSpeed);
        editRules.PlayerSpeed = StringToFloat(playerSpeed);
        GUILayout.EndHorizontal();
        
        BeginEditControls("Score Limit",null);
        scoreLimit = GUILayout.TextField(scoreLimit);
        editRules.ScoreLimit = StringToInt(scoreLimit);
        GUILayout.EndHorizontal();
        
        BeginEditControls("Number of Reds", redBackground);
        numRed = GUILayout.TextField(numRed);
        editRules.NumRed = StringToInt(numRed);
        GUILayout.EndHorizontal();
        
        BeginEditControls("Number of Greens", greenBackground);
        numGreen = GUILayout.TextField(numGreen);
        editRules.NumGreen = StringToInt(numGreen);
        GUILayout.EndHorizontal();
        
        BeginEditControls("Number of Blues", blueBackground);
        numBlue = GUILayout.TextField(numBlue);
        editRules.NumBlue = StringToInt(numBlue);
        GUILayout.EndHorizontal();
        
        if(editRules.NumRed > 0)
        {
            BeginEditControls("Red Movement", redBackground);
            editRules.RedMovement = (MovementType) GUILayout.SelectionGrid((int)editRules.RedMovement,movementTypes,3,"toggle");
            GUILayout.EndHorizontal();
            
            if(editRules.RedMovement == MovementType.Chase || editRules.RedMovement == MovementType.Flee)
            {
                BeginEditControls("Red Target", redBackground);
                editRules.RedTarget = (EntityType) GUILayout.SelectionGrid((int)editRules.RedTarget,entityTypes,4,"toggle");
                GUILayout.EndHorizontal();
            }
            
            BeginEditControls("Red Speed", redBackground);
            redSpeed = GUILayout.TextField(redSpeed);
            editRules.RedSpeed = StringToFloat(redSpeed);
            GUILayout.EndHorizontal();
        }
        
        if(editRules.NumGreen > 0)
        {
            BeginEditControls("Green Movement", greenBackground);
            editRules.GreenMovement = (MovementType) GUILayout.SelectionGrid((int)editRules.GreenMovement,movementTypes,3,"toggle");
            GUILayout.EndHorizontal();
            
            if(editRules.GreenMovement == MovementType.Chase || editRules.GreenMovement == MovementType.Flee)
            {
                BeginEditControls("Green Target", greenBackground);
                editRules.GreenTarget = (EntityType) GUILayout.SelectionGrid((int)editRules.GreenTarget,entityTypes,4,"toggle");
                GUILayout.EndHorizontal();
            }
            
            BeginEditControls("Green Speed", greenBackground);
            greenSpeed = GUILayout.TextField(greenSpeed);
            editRules.GreenSpeed = StringToFloat(greenSpeed);
            GUILayout.EndHorizontal();
        }
        
        if(editRules.NumBlue > 0)
        {
            BeginEditControls("Blue Movement", blueBackground);
            editRules.BlueMovement = (MovementType) GUILayout.SelectionGrid((int)editRules.BlueMovement,movementTypes,3,"toggle");
            GUILayout.EndHorizontal();
            
            if(editRules.BlueMovement == MovementType.Chase || editRules.BlueMovement == MovementType.Flee)
            {
                BeginEditControls("Blue Target", blueBackground);
                editRules.BlueTarget = (EntityType) GUILayout.SelectionGrid((int)editRules.BlueTarget,entityTypes,4,"toggle");
                GUILayout.EndHorizontal();
            }
            
            BeginEditControls("Blue Speed", blueBackground);
            blueSpeed = GUILayout.TextField(blueSpeed);
            editRules.BlueSpeed = StringToFloat(blueSpeed);
            GUILayout.EndHorizontal();
        }
        
        if(editRules.NumRed > 0)
        {
            BeginEditControls("Player When Hit Red", null);
            editRules.PlayerRed = (CollisionEffect) GUILayout.SelectionGrid((int)editRules.PlayerRed,collisionEffects,3,"toggle");
            GUILayout.EndHorizontal();
            
            BeginEditControls("Red When Hit Player", redBackground);
            editRules.RedPlayer = (CollisionEffect) GUILayout.SelectionGrid((int)editRules.RedPlayer,collisionEffects,3,"toggle");
            GUILayout.EndHorizontal();
            
            BeginEditControls("Red When Hit Red", redBackground);
            editRules.RedRed = (CollisionEffect) GUILayout.SelectionGrid((int)editRules.RedRed,collisionEffects,3,"toggle");
            GUILayout.EndHorizontal();
        }
        
        if(editRules.NumGreen > 0)
        {
            BeginEditControls("Player When Hit Green",null);
            editRules.PlayerGreen = (CollisionEffect) GUILayout.SelectionGrid((int)editRules.PlayerGreen,collisionEffects,3,"toggle");
            GUILayout.EndHorizontal();
            
            BeginEditControls("Green When Hit Player", greenBackground);
            editRules.GreenPlayer = (CollisionEffect) GUILayout.SelectionGrid((int)editRules.GreenPlayer,collisionEffects,3,"toggle");
            GUILayout.EndHorizontal();
            
            BeginEditControls("Green When Hit Green", greenBackground);
            editRules.GreenGreen = (CollisionEffect) GUILayout.SelectionGrid((int)editRules.GreenGreen,collisionEffects,3,"toggle");
            GUILayout.EndHorizontal();
        }
        
        if(editRules.NumBlue > 0)
        {
            BeginEditControls("Player When Hit Blue",null);
            editRules.PlayerBlue = (CollisionEffect) GUILayout.SelectionGrid((int)editRules.PlayerBlue,collisionEffects,3,"toggle");
            GUILayout.EndHorizontal();
            
            BeginEditControls("Blue When Hit Player", blueBackground);
            editRules.BluePlayer = (CollisionEffect) GUILayout.SelectionGrid((int)editRules.BluePlayer,collisionEffects,3,"toggle");
            GUILayout.EndHorizontal();
            
            BeginEditControls("Blue When Hit Blue", blueBackground);
            editRules.BlueBlue = (CollisionEffect) GUILayout.SelectionGrid((int)editRules.BlueBlue,collisionEffects,3,"toggle");
            GUILayout.EndHorizontal();
        }
        
        if(editRules.NumRed > 0 && editRules.NumGreen > 0)
        {
            BeginEditControls("Red When Hit Green", redBackground);
            editRules.RedGreen = (CollisionEffect) GUILayout.SelectionGrid((int)editRules.RedGreen,collisionEffects,3,"toggle");
            GUILayout.EndHorizontal();
            
            BeginEditControls("Green When Hit Red", greenBackground);
            editRules.GreenRed = (CollisionEffect) GUILayout.SelectionGrid((int)editRules.GreenRed,collisionEffects,3,"toggle");
            GUILayout.EndHorizontal();
        }
        
        if(editRules.NumRed > 0 && editRules.NumBlue > 0)
        {
            BeginEditControls("Red When Hit Blue", redBackground);
            editRules.RedBlue = (CollisionEffect) GUILayout.SelectionGrid((int)editRules.RedBlue,collisionEffects,3,"toggle");
            GUILayout.EndHorizontal();
            
            BeginEditControls("Blue When Hit Red", blueBackground);
            editRules.BlueRed = (CollisionEffect) GUILayout.SelectionGrid((int)editRules.BlueRed,collisionEffects,3,"toggle");
            GUILayout.EndHorizontal();
        }
        
        if(editRules.NumGreen > 0 && editRules.NumBlue > 0)
        {
            BeginEditControls("Green When Hit Blue", greenBackground);
            editRules.GreenBlue = (CollisionEffect) GUILayout.SelectionGrid((int)editRules.GreenBlue,collisionEffects,3,"toggle");
            GUILayout.EndHorizontal();
            
            BeginEditControls("Blue When Hit Green", blueBackground);
            editRules.BlueGreen = (CollisionEffect) GUILayout.SelectionGrid((int)editRules.BlueGreen,collisionEffects,3,"toggle");
            GUILayout.EndHorizontal();
        }
        
        BeginEditControls("Collision Scores",null);
        //GUILayout.BeginHorizontal();
        GUILayout.BeginVertical();
        SizedLabel("");
        SizedLabel("Player");
        SizedLabel("Red");
        SizedLabel("Green");
        SizedLabel("Blue");
        GUILayout.EndVertical();
        GUILayout.BeginVertical();
        GUILayout.Label("Red", GUILayout.MinWidth(40));
        scorePlayerRed = GUILayout.TextField(scorePlayerRed);
        editRules.ScorePlayerRed = StringToInt(scorePlayerRed);
        scoreRedRed = GUILayout.TextField(scoreRedRed);
        editRules.ScoreRedRed = StringToInt(scoreRedRed);
        GUILayout.EndVertical();
        GUILayout.BeginVertical();
        GUILayout.Label("Green", GUILayout.MinWidth(40));
        scorePlayerGreen = GUILayout.TextField(scorePlayerGreen);
        editRules.ScorePlayerGreen = StringToInt(scorePlayerGreen);
        scoreRedGreen = GUILayout.TextField(scoreRedGreen);
        editRules.ScoreRedGreen = StringToInt(scoreRedGreen);
        scoreGreenGreen = GUILayout.TextField(scoreGreenGreen);
        editRules.ScoreGreenGreen = StringToInt(scoreGreenGreen);
        GUILayout.EndVertical();
        GUILayout.BeginVertical();
        GUILayout.Label("Blue", GUILayout.MinWidth(40));
        scorePlayerBlue = GUILayout.TextField(scorePlayerBlue);
        editRules.ScorePlayerBlue = StringToInt(scorePlayerBlue);
        scoreRedBlue = GUILayout.TextField(scoreRedBlue);
        editRules.ScoreRedBlue = StringToInt(scoreRedBlue);
        scoreGreenBlue = GUILayout.TextField(scoreGreenBlue);
        editRules.ScoreGreenBlue = StringToInt(scoreGreenBlue);
        scoreBlueBlue = GUILayout.TextField(scoreBlueBlue);
        editRules.ScoreBlueBlue = StringToInt(scoreBlueBlue);
        GUILayout.EndVertical();
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        
        
        GUILayout.EndScrollView();
        if(GUILayout.Button("Save Changes"))
        {
            util.CopyFromRules(editRules);
            Application.LoadLevel(0);
        }
        EndPage();
    }
    
    private void SizedLabel(string text)
    {
        //create a label with minimum width of the width of the content
        GUIContent str = new GUIContent(text);
        GUILayout.Label(str, GUILayout.MinWidth(Skin.GetStyle("label").CalcSize(str).x));
    }
    
    private void BeginEditControls(string name, GUIStyle style)
    {
        if(style == null)
            GUILayout.BeginHorizontal();
        else
            GUILayout.BeginHorizontal(style);
        SizedLabel(name);
        GUILayout.FlexibleSpace();
    }
    
    private int StringToInt(string str)
    {
        return Convert.ToInt32(str == "" ? "0" : str);
    }
    
    private float StringToFloat(string str)
    {
        return float.Parse(str == "" ? "0" : str);
    }
    
    void ShowExport()
    {
        BeginPage();
        string xml = util.ExportRuleset();
        scrollPosition = GUILayout.BeginScrollView(scrollPosition);
        GUILayout.TextArea(xml);
        GUILayout.EndScrollView();
        
        if(GUILayout.Button("Copy all"))
        {
            TextEditor te = new TextEditor();
            te.content = new GUIContent(xml);
            te.SelectAll();
            te.Copy();
        }
//        filename = GUILayout.TextField(filename);
//        if(GUILayout.Button("Save"))
//        {
//            if(!filename.EndsWith(".xml"))
//                filename = filename.Insert(filename.Length, ".xml");
//            util.SaveRuleset(filename);
//            currentPage = Page.Main;
//        }
        EndPage();
    }
    
    void ShowImport()
    {
        BeginPage();
        scrollPosition = GUILayout.BeginScrollView(scrollPosition);
        importXml = GUILayout.TextArea(importXml);
        GUILayout.EndScrollView();
        
        if(GUILayout.Button("Import"))
        {
            util.ImportRuleset(importXml);
            importXml = "";
            Application.LoadLevel(0);
        }
//        filename = GUILayout.TextField(filename);
//        if(GUILayout.Button("Load"))
//        {
//            util.LoadRuleset(filename);
//            Application.LoadLevel(0);
//        }
        EndPage();
    }
 
    void ShowBackButton() {
        if (GUI.Button(new Rect(20, Screen.height - 50, 50, 20),"Back")) {
            currentPage = Page.Main;
        }
    }
 
    void BeginPage()
    {
        GUILayout.BeginArea(new Rect(Screen.width / 4, Screen.height / 4, Screen.width / 2, Screen.height / 2));
        //GUILayout.BeginArea( new Rect((Screen.width - width) / 2, (Screen.height - height) / 2, width, height));
    }
 
    void EndPage() {
        GUILayout.EndArea();
        if (currentPage != Page.Main) {
            ShowBackButton();
        }
    }
 
    bool IsBeginning() {
        return (Time.timeSinceLevelLoad < startTime);
    }
 
 
    void MainPauseMenu() {
        BeginPage();
        if(ScoreKeeper.GameOver)
        {
            string message = "";
            if(ScoreKeeper.Won)
                message = "You win!";
            else
                message = "You lose!";
            GUILayout.Label(message);
        }
        GUILayout.Label(ruleset.Name);
//        var textStyle = new GUIStyle();
//        textStyle.normal.textColor = Color.black;
//        textStyle.wordWrap = true;
        GUILayout.Label(ruleset.Description/*, textStyle*/);
        if(!ScoreKeeper.GameOver)
        {
            if (GUILayout.Button (IsBeginning() ? "Play" : "Continue"))
                UnPauseGame();
        }
        if (!IsBeginning())
        {
            if(GUILayout.Button ("Restart"))
            {
                Application.LoadLevel(0);
            }
        }
		if (GUILayout.Button ("Choose Ruleset"))
			currentPage = Page.Choose;
        if (GUILayout.Button ("Edit Ruleset"))
        {
            SetupEdit();
            currentPage = Page.Edit;
        }
        if (GUILayout.Button ("Export Ruleset"))
            currentPage = Page.Export;
        if (GUILayout.Button ("Import Ruleset"))
            currentPage = Page.Import;
        EndPage();
    }
 
    public void PauseGame()
    {
        savedTimeScale = Time.timeScale;
        Time.timeScale = 0;
        AudioListener.pause = true;
        currentPage = Page.Main;
    }
 
    void UnPauseGame()
    {
        Time.timeScale = savedTimeScale;
        AudioListener.pause = false;
        
        currentPage = Page.None;
        
        if (IsBeginning() && start != null) {
            start.SetActive(true);
        }
    }
 
 bool IsGamePaused() {
     return (Time.timeScale == 0);
 }
 
 void OnApplicationPause(bool pause) {
     if (IsGamePaused()) {
         AudioListener.pause = true;
     }
 }
}