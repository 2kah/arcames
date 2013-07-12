using UnityEngine;
using System.Collections;
using AssemblyCSharp;
using System;
using System.IO;

//http://wiki.unity3d.com/index.php?title=PauseMenu
public class PauseMenu : MonoBehaviour
{
 
    public GUISkin skin;
    
    private float startTime = 0.1f;
 
    public Material mat;
    
    private float savedTimeScale;
    private Ruleset ruleset;
    private Util util;
    private Rules[] inbuiltRules;
    private Vector2 scrollPosition;
    
    public GameObject start;
 
    public string url = "http://2kah.co.uk";
 
    public Color statColor = Color.yellow;
 
    public enum Page {
        None,Main,Choose,Export,Import
    }
 
    private Page currentPage;
    
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
        if (skin != null) {
            GUI.skin = skin;
        }
        if (IsGamePaused()) {
            GUI.color = statColor;
            switch (currentPage) {
            case Page.Main: MainPauseMenu(); break;
			case Page.Choose: ShowChoose(); break;
            case Page.Export: ShowExport(); break;
            case Page.Import: ShowImport(); break;
            }
        }
    }
	
	void ShowChoose()
	{
		BeginPage(200,200);
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
                currentPage = Page.Main;
            }
        }
        
		EndPage();
	}
    
    void ShowExport()
    {
        BeginPage(200,200);
        string xml = util.ExportRuleset();
        scrollPosition = GUILayout.BeginScrollView(scrollPosition);
        GUILayout.TextArea(xml);
        GUILayout.EndScrollView();
        TextEditor te = new TextEditor();
        te.content = new GUIContent(xml);
        if(GUILayout.Button("Copy all"))
        {
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
        BeginPage(200,200);
//        filename = GUILayout.TextField(filename);
//        if(GUILayout.Button("Load"))
//        {
//            util.LoadRuleset(filename);
//            Application.LoadLevel(0);
//            currentPage = Page.Main;
//        }
        EndPage();
    }
 
    void ShowBackButton() {
        if (GUI.Button(new Rect(20, Screen.height - 50, 50, 20),"Back")) {
            currentPage = Page.Main;
        }
    }
 
    void BeginPage(int width, int height) {
        GUILayout.BeginArea( new Rect((Screen.width - width) / 2, (Screen.height - height) / 2, width, height));
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
        BeginPage(200,200);
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
        var textStyle = new GUIStyle();
        textStyle.normal.textColor = Color.black;
        textStyle.wordWrap = true;
        GUILayout.Label(ruleset.Description, textStyle);
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
 
 void UnPauseGame() {
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