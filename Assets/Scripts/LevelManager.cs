using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using TMPro;

public class LevelManager : MonoBehaviour
{
    public string _currentLevel;
    public static LevelManager levelManager;
    public string _levelDataPath;
    public int _numberOfColorsInCurrentLevel,_numberOfColorsCompleted;
    public GameObject _levelCompleteUI;
    public TextMeshProUGUI _boardFillPercentageText;
    private void Start()
    {
        levelManager = this;
        //Debug.Log("streaming assets path " + Application.streamingAssetsPath);
        _levelDataPath = Application.streamingAssetsPath + "/LevelData/";
        //CreateOneLevel();
        //SaveLevelAsJson(level1);
    }
    public void ToggleLevelCompleteUI(bool enabled)
    {
        _levelCompleteUI.SetActive(enabled);
    }
    public void CheckIfAllLinesConnected()
    {
        if (_numberOfColorsCompleted >= _numberOfColorsInCurrentLevel && GridManager.gridManager.CalculateBoardFillPercentage() >= 100)
        {
            GameLogicManager.gameLogicManager.FinishLevel();
        }
    }
    public void ColorCompleted()
    {
        _numberOfColorsCompleted++;
    }
    public void FinishLevel()
    {
        ToggleLevelCompleteUI(true);
        //GameLogicManager.gameLogicManager.ResetBoard();
        //MenuManager.menuManager.ToggleLevelMenu(true);
        //MenuManager.menuManager.ToggleInGameMenu(false);
        //Debug.Log("level finished");
    }
    void LoadLevel(string json)
    {
        GameLogicManager.gameLogicManager.ResetBoard();
        var level = JsonConvert.DeserializeObject<Level>(json);
        _currentLevel = json;
        foreach(KeyValuePair<string,string> point in level._startPoints)
        {
            var Tile = GridManager.gridManager.GetTileAtPosition((StringToVector2(point.Key)));
            Color color;
            ColorUtility.TryParseHtmlString(point.Value, out color);
            Tile.SetAsStartTile(color);
            Tile._isPermanentStartTile = true;
            //Tile._isStartingTile = true;
            
        }
        _numberOfColorsInCurrentLevel = level._startPoints.Count/2;
        _numberOfColorsCompleted = 0;
        GameLogicManager.gameLogicManager._currentLevelComplete = false;
    }
    public void ResetLevel()
    {
        LoadLevel(_currentLevel);
    }
    public void ReadAndLoadLevel(string _filename)
    {
        string json = File.ReadAllText(_levelDataPath + _filename);
        LoadLevel(json);
        MenuManager.menuManager.ToggleLevelMenu(false);
        MenuManager.menuManager.ToggleInGameMenu(true);
    }
    public void ReadAndLoadLevelFromResources(string _filename)
    {
        string json = Resources.Load("LevelData/" + _filename).ToString();
        //string json = File.ReadAllText(_levelDataPath + _filename);
        LoadLevel(json);
        MenuManager.menuManager.ToggleLevelMenu(false);
        MenuManager.menuManager.ToggleInGameMenu(true);
    }
    public Vector2 StringToVector2(string rString)
    {
        float floatx, floaty;
        rString = rString.Replace('(', ' ');
        rString = rString.Replace(')', ' ');
        //Debug.Log("Vector 2 value after removing ( " + rString);
        string[] temp = rString.Substring(0, rString.Length - 1).Split(',');
        floatx = System.Convert.ToSingle(temp[0]);
        floaty = System.Convert.ToSingle(temp[1]);
        Vector2 rValue = new Vector2(floatx, floaty);
        return rValue;
    }
}
