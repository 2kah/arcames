using UnityEngine;
using System.Collections;
using AssemblyCSharp;
using System;
using System.IO;
using System.Linq;

//http://wiki.unity3d.com/index.php?title=PauseMenu
public class PauseMenu : MonoBehaviour
{
 
    public GUISkin Skin;
    
    private float startTime = 0.1f;
    
    private float savedTimeScale;
    private Ruleset ruleset;
    private MapManager mapManager;
    private Util util;
    private Rules[] inbuiltRules;
    private Map[] inbuiltMaps;
    private Vector2 scrollPosition;
    private string importXml = "";
    private Rules editRules;
    private string scoreLimit, numRed, numGreen, numBlue, scorePlayerRed, scorePlayerGreen, scorePlayerBlue, scoreRedRed, scoreRedGreen, scoreRedBlue, scoreGreenGreen, scoreGreenBlue, scoreBlueBlue;
    private string playerSpeed, redSpeed, greenSpeed, blueSpeed;
    private string[] movementTypes, entityTypes, collisionEffects, mapNames;
    private int chosenMap = 0;
    
    public GUIStyle[] style;
    //public GUIStyle style[(int)EntityType.Red], style[(int)EntityType.Green], style[(int)EntityType.Blue];
    
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
        mapManager = GameObject.Find("MapManager").GetComponent<MapManager>();
        Time.timeScale = 1;
        PauseGame();
        inbuiltRules = new Rules[util.InbuiltRules.Count];
        for(int i = 0; i < inbuiltRules.Length; i++)
        {
            Type rulesType = util.InbuiltRules[i];
            inbuiltRules[i] = (Rules)Activator.CreateInstance(rulesType);
        }
        inbuiltMaps = new Map[util.InbuiltMaps.Count];
        mapNames = new string[inbuiltMaps.Length];
        for(int i = 0; i < inbuiltMaps.Length; i++)
        {
            Type mapType = util.InbuiltMaps[i];
            mapNames[i] = mapType.Name;
            inbuiltMaps[i] = (Map)Activator.CreateInstance(mapType);
        }
        movementTypes = Enum.GetNames(typeof(MovementType));
        entityTypes = Enum.GetNames(typeof(EntityType));
        collisionEffects = Enum.GetNames(typeof(CollisionEffect));
        scrollPosition = Vector2.zero;
        
        //TODO: make this part of map load code
        //move camera up to match size of map
        var cameraPos = transform.position;
        cameraPos.y = (float) Math.Max(ruleset.MapWidth / 1.6, ruleset.MapHeight);
        transform.position = cameraPos;
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
        numRed = editRules.NumEntities[(int)EntityType.Red].ToString();
        numGreen = editRules.NumEntities[(int)EntityType.Green].ToString();
        numBlue = editRules.NumEntities[(int)EntityType.Blue].ToString();
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
        chosenMap = CurrentMap(editRules);
    }
    
    private int CurrentMap(Rules currentRules)
    {
        for(int i = 0; i < inbuiltMaps.Length; i++)
        {
            if(inbuiltMaps[i].MapData.SequenceEqual(currentRules.MapData))
                return i;
        }
        return 0;
    }
    
    private bool MapEquals(bool[] map1, bool[] map2)
    {
        if(map1.Length != map2.Length)
            return false;
        for(int i = 0; i < map1.Length; i++)
        {
            if(map1[i] != map2[i])
                return false;
        }
        return true;
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
        
        BeginEditControls("Map", null);
        var lastChosen = chosenMap;
        chosenMap = GUILayout.SelectionGrid(chosenMap,mapNames,2,"toggle");
        if(chosenMap != lastChosen)
        {
            //map selection has changed
            editRules.MapWidth = inbuiltMaps[chosenMap].Width;
            editRules.MapHeight = inbuiltMaps[chosenMap].Height;
            editRules.MapData = inbuiltMaps[chosenMap].MapData;
        }
        GUILayout.EndHorizontal();
        
        BeginEditControls("Player Speed",null);
        playerSpeed = GUILayout.TextField(playerSpeed);
        editRules.PlayerSpeed = StringToFloat(playerSpeed);
        GUILayout.EndHorizontal();
        
        BeginEditControls("Score Limit",null);
        scoreLimit = GUILayout.TextField(scoreLimit);
        editRules.ScoreLimit = StringToInt(scoreLimit);
        GUILayout.EndHorizontal();
        
        BeginEditControls("Number of Reds", style[(int)EntityType.Red]);
        numRed = GUILayout.TextField(numRed);
        editRules.NumEntities[(int)EntityType.Red] = StringToInt(numRed);
        GUILayout.EndHorizontal();
        
        BeginEditControls("Number of Greens", style[(int)EntityType.Green]);
        numGreen = GUILayout.TextField(numGreen);
        editRules.NumEntities[(int)EntityType.Green] = StringToInt(numGreen);
        GUILayout.EndHorizontal();
        
        BeginEditControls("Number of Blues", style[(int)EntityType.Blue]);
        numBlue = GUILayout.TextField(numBlue);
        editRules.NumEntities[(int)EntityType.Blue] = StringToInt(numBlue);
        GUILayout.EndHorizontal();
        
        if(editRules.NumEntities[(int)EntityType.Red] > 0)
        {
            BeginEditControls("Red Movement", style[(int)EntityType.Red]);
            editRules.RedMovement = (MovementType) GUILayout.SelectionGrid((int)editRules.RedMovement,movementTypes,3,"toggle");
            GUILayout.EndHorizontal();
            
            if(editRules.RedMovement == MovementType.Chase || editRules.RedMovement == MovementType.Flee)
            {
                BeginEditControls("Red Target", style[(int)EntityType.Red]);
                editRules.RedTarget = (EntityType) GUILayout.SelectionGrid((int)editRules.RedTarget,entityTypes,4,"toggle");
                GUILayout.EndHorizontal();
            }
            
            if(editRules.RedMovement != MovementType.Still)
            {
                BeginEditControls("Red Speed", style[(int)EntityType.Red]);
                redSpeed = GUILayout.TextField(redSpeed);
                editRules.RedSpeed = StringToFloat(redSpeed);
                GUILayout.EndHorizontal();
            }
        }
        
        if(editRules.NumEntities[(int)EntityType.Green] > 0)
        {
            BeginEditControls("Green Movement", style[(int)EntityType.Green]);
            editRules.GreenMovement = (MovementType) GUILayout.SelectionGrid((int)editRules.GreenMovement,movementTypes,3,"toggle");
            GUILayout.EndHorizontal();
            
            if(editRules.GreenMovement == MovementType.Chase || editRules.GreenMovement == MovementType.Flee)
            {
                BeginEditControls("Green Target", style[(int)EntityType.Green]);
                editRules.GreenTarget = (EntityType) GUILayout.SelectionGrid((int)editRules.GreenTarget,entityTypes,4,"toggle");
                GUILayout.EndHorizontal();
            }
            
            if(editRules.GreenMovement != MovementType.Still)
            {
                BeginEditControls("Green Speed", style[(int)EntityType.Green]);
                greenSpeed = GUILayout.TextField(greenSpeed);
                editRules.GreenSpeed = StringToFloat(greenSpeed);
                GUILayout.EndHorizontal();
            }
        }
        
        if(editRules.NumEntities[(int)EntityType.Blue] > 0)
        {
            BeginEditControls("Blue Movement", style[(int)EntityType.Blue]);
            editRules.BlueMovement = (MovementType) GUILayout.SelectionGrid((int)editRules.BlueMovement,movementTypes,3,"toggle");
            GUILayout.EndHorizontal();
            
            if(editRules.BlueMovement == MovementType.Chase || editRules.BlueMovement == MovementType.Flee)
            {
                BeginEditControls("Blue Target", style[(int)EntityType.Blue]);
                editRules.BlueTarget = (EntityType) GUILayout.SelectionGrid((int)editRules.BlueTarget,entityTypes,4,"toggle");
                GUILayout.EndHorizontal();
            }
            
            if(editRules.BlueMovement != MovementType.Still)
            {
                BeginEditControls("Blue Speed", style[(int)EntityType.Blue]);
                blueSpeed = GUILayout.TextField(blueSpeed);
                editRules.BlueSpeed = StringToFloat(blueSpeed);
                GUILayout.EndHorizontal();
            }
        }
        
        string[] entityNames = Enum.GetNames(typeof(EntityType));
        for(int i = 0; i < entityNames.Length - 1; i++)
        {
            if(editRules.NumEntities[i] > 0)
            {
                for(int j = 0; j < 3; j++)
                {
                    int first = j == 0 ? 3 : i;
                    int second = j == 1 ? 3 : i;
                    BeginEditControls(entityNames[first] + " When Hit " + entityNames[second], style[i]);
                    int collisionIndex = util.CollisionEffectsIndex((EntityType)first, (EntityType)second);
                    editRules.CollisionEffects[collisionIndex] = (CollisionEffect) GUILayout.SelectionGrid((int)editRules.CollisionEffects[collisionIndex],collisionEffects,3,"toggle");
                    GUILayout.EndHorizontal();
                }
            }
        }
        
//        if(editRules.NumRed > 0)
//        {
//            BeginEditControls("Player When Hit Red", null);
//            int playerRedIndex = util.CollisionEffectsIndex(EntityType.Player, EntityType.Red);
//            editRules.CollisionEffects[playerRedIndex] = (CollisionEffect) GUILayout.SelectionGrid((int)editRules.CollisionEffects[playerRedIndex],collisionEffects,3,"toggle");
//            GUILayout.EndHorizontal();
//            
//            BeginEditControls("Red When Hit Player", style[(int)EntityType.Red]);
//            editRules.RedPlayer = (CollisionEffect) GUILayout.SelectionGrid((int)editRules.RedPlayer,collisionEffects,3,"toggle");
//            GUILayout.EndHorizontal();
//            
//            BeginEditControls("Red When Hit Red", style[(int)EntityType.Red]);
//            editRules.RedRed = (CollisionEffect) GUILayout.SelectionGrid((int)editRules.RedRed,collisionEffects,3,"toggle");
//            GUILayout.EndHorizontal();
//        }
//        
//        if(editRules.NumGreen > 0)
//        {
//            BeginEditControls("Player When Hit Green",null);
//            editRules.PlayerGreen = (CollisionEffect) GUILayout.SelectionGrid((int)editRules.PlayerGreen,collisionEffects,3,"toggle");
//            GUILayout.EndHorizontal();
//            
//            BeginEditControls("Green When Hit Player", style[(int)EntityType.Green]);
//            editRules.GreenPlayer = (CollisionEffect) GUILayout.SelectionGrid((int)editRules.GreenPlayer,collisionEffects,3,"toggle");
//            GUILayout.EndHorizontal();
//            
//            BeginEditControls("Green When Hit Green", style[(int)EntityType.Green]);
//            editRules.GreenGreen = (CollisionEffect) GUILayout.SelectionGrid((int)editRules.GreenGreen,collisionEffects,3,"toggle");
//            GUILayout.EndHorizontal();
//        }
//        
//        if(editRules.NumBlue > 0)
//        {
//            BeginEditControls("Player When Hit Blue",null);
//            editRules.PlayerBlue = (CollisionEffect) GUILayout.SelectionGrid((int)editRules.PlayerBlue,collisionEffects,3,"toggle");
//            GUILayout.EndHorizontal();
//            
//            BeginEditControls("Blue When Hit Player", style[(int)EntityType.Blue]);
//            editRules.BluePlayer = (CollisionEffect) GUILayout.SelectionGrid((int)editRules.BluePlayer,collisionEffects,3,"toggle");
//            GUILayout.EndHorizontal();
//            
//            BeginEditControls("Blue When Hit Blue", style[(int)EntityType.Blue]);
//            editRules.BlueBlue = (CollisionEffect) GUILayout.SelectionGrid((int)editRules.BlueBlue,collisionEffects,3,"toggle");
//            GUILayout.EndHorizontal();
//        }
        
        if(editRules.NumEntities[(int)EntityType.Red] > 0 && editRules.NumEntities[(int)EntityType.Green] > 0)
        {
            BeginEditControls("Red When Hit Green", style[(int)EntityType.Red]);
            int collisionIndex = util.CollisionEffectsIndex(EntityType.Red, EntityType.Green);
            editRules.CollisionEffects[collisionIndex] = (CollisionEffect) GUILayout.SelectionGrid((int)editRules.CollisionEffects[collisionIndex],collisionEffects,3,"toggle");
            GUILayout.EndHorizontal();
            
            BeginEditControls("Green When Hit Red", style[(int)EntityType.Green]);
            collisionIndex = util.CollisionEffectsIndex(EntityType.Green, EntityType.Red);
            editRules.CollisionEffects[collisionIndex] = (CollisionEffect) GUILayout.SelectionGrid((int)editRules.CollisionEffects[collisionIndex],collisionEffects,3,"toggle");
            GUILayout.EndHorizontal();
        }
        
        if(editRules.NumEntities[(int)EntityType.Red] > 0 && editRules.NumEntities[(int)EntityType.Blue] > 0)
        {
            BeginEditControls("Red When Hit Blue", style[(int)EntityType.Red]);
            int collisionIndex = util.CollisionEffectsIndex(EntityType.Red, EntityType.Blue);
            editRules.CollisionEffects[collisionIndex] = (CollisionEffect) GUILayout.SelectionGrid((int)editRules.CollisionEffects[collisionIndex],collisionEffects,3,"toggle");
            GUILayout.EndHorizontal();
            
            BeginEditControls("Blue When Hit Red", style[(int)EntityType.Blue]);
            collisionIndex = util.CollisionEffectsIndex(EntityType.Blue, EntityType.Red);
            editRules.CollisionEffects[collisionIndex] = (CollisionEffect) GUILayout.SelectionGrid((int)editRules.CollisionEffects[collisionIndex],collisionEffects,3,"toggle");
            GUILayout.EndHorizontal();
        }
        
        if(editRules.NumEntities[(int)EntityType.Green] > 0 && editRules.NumEntities[(int)EntityType.Blue] > 0)
        {
            BeginEditControls("Green When Hit Blue", style[(int)EntityType.Green]);
            int collisionIndex = util.CollisionEffectsIndex(EntityType.Green, EntityType.Blue);
            editRules.CollisionEffects[collisionIndex] = (CollisionEffect) GUILayout.SelectionGrid((int)editRules.CollisionEffects[collisionIndex],collisionEffects,3,"toggle");
            GUILayout.EndHorizontal();
            
            BeginEditControls("Blue When Hit Green", style[(int)EntityType.Blue]);
            collisionIndex = util.CollisionEffectsIndex(EntityType.Blue, EntityType.Green);
            editRules.CollisionEffects[collisionIndex] = (CollisionEffect) GUILayout.SelectionGrid((int)editRules.CollisionEffects[collisionIndex],collisionEffects,3,"toggle");
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