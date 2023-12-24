using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;

public class LevelManager : MonoBehaviour
{
    public Level level1;
    public Level _currentLevel;
    public static LevelManager levelManager;
    public string _levelDataPath;
    public int _numberOfColorsInCurrentLevel,_numberOfColorsCompleted;
    private void Start()
    {
        levelManager = this;
        _levelDataPath = Application.dataPath + "/Resources/LevelData/";
        //CreateOneLevel();
        //SaveLevelAsJson(level1);
    }
    public void CheckIfAllLinesConnected()
    {
        if (_numberOfColorsCompleted >= _numberOfColorsInCurrentLevel)
        {
            FinishLevel();
        }
    }
    public void ColorCompleted()
    {
        _numberOfColorsCompleted++;
    }
    public void FinishLevel()
    {
        Debug.Log("level finished");
    }
    void CreateOneLevel()
    {
        level1 = new Level();
        level1._levelName = "Level 1";
        level1._startPoints.Add((new Vector2(3, 3)).ToString(), "#" + ColorUtility.ToHtmlStringRGBA (Color.red));
        level1._startPoints.Add((new Vector2(1, 3)).ToString(), "#" + ColorUtility.ToHtmlStringRGBA(Color.red));
        level1._startPoints.Add((new Vector2(0, 0)).ToString(), "#" + ColorUtility.ToHtmlStringRGBA(Color.green));
        level1._startPoints.Add((new Vector2(4, 1)).ToString(), "#" + ColorUtility.ToHtmlStringRGBA(Color.green));
    }
    void LoadLevel(string json)
    {
        var level = JsonConvert.DeserializeObject<Level>(json);
        _currentLevel = level;
        foreach(KeyValuePair<string,string> point in level._startPoints)
        {
            var Tile = GridManager.gridManager.GetTileAtPosition((StringToVector2(point.Key)));
            Color color;
            ColorUtility.TryParseHtmlString(point.Value, out color);
            Tile.SetAsStartTile(color);
            Tile._isStartingTile = true;
            
        }
        _numberOfColorsInCurrentLevel = level._startPoints.Count/2;
        _numberOfColorsCompleted = 0;
    }
    public void SaveLevelAsJson(Level level)
    {
        string json = JsonConvert.SerializeObject(level);
        //Debug.Log("data path " + Application.dataPath + "/Resources/LevelData/");
        File.WriteAllText(_levelDataPath + level._levelName + ".txt", json);
    }
    public void ReadAndLoadLevel()
    {
        string json = File.ReadAllText(_levelDataPath + "Level 1" + ".txt");
        LoadLevel(json);
    }
    public Vector2 StringToVector2(string rString)
    {
        float floatx, floaty;
        rString = rString.Replace('(', ' ');
        rString = rString.Replace(')', ' ');
        Debug.Log("Vector 2 value after removing ( " + rString);
        string[] temp = rString.Substring(0, rString.Length - 1).Split(',');
        floatx = System.Convert.ToSingle(temp[0]);
        floaty = System.Convert.ToSingle(temp[1]);
        Vector2 rValue = new Vector2(floatx, floaty);
        return rValue;
    }
}
