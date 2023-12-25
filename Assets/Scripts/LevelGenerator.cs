using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;

public class LevelGenerator : MonoBehaviour
{
    Level level;
    public string _levelDataPath;
    // Start is called before the first frame update
    void CreateLevel()
    {
        level = new Level();
        level._levelName = "Level 6";
        level._startPoints.Add((new Vector2(0, 0)).ToString(), "#" + ColorUtility.ToHtmlStringRGBA(Color.red));
        level._startPoints.Add((new Vector2(1, 1)).ToString(), "#" + ColorUtility.ToHtmlStringRGBA(Color.red));
        level._startPoints.Add((new Vector2(1, 0)).ToString(), "#" + ColorUtility.ToHtmlStringRGBA(Color.green));
        level._startPoints.Add((new Vector2(4, 1)).ToString(), "#" + ColorUtility.ToHtmlStringRGBA(Color.green));
        level._startPoints.Add((new Vector2(2, 2)).ToString(), "#" + ColorUtility.ToHtmlStringRGBA(Color.blue));
        level._startPoints.Add((new Vector2(4, 3)).ToString(), "#" + ColorUtility.ToHtmlStringRGBA(Color.blue));
        level._startPoints.Add((new Vector2(0, 2)).ToString(), "#" + ColorUtility.ToHtmlStringRGBA(Color.yellow));
        level._startPoints.Add((new Vector2(0, 4)).ToString(), "#" + ColorUtility.ToHtmlStringRGBA(Color.yellow));
        level._startPoints.Add((new Vector2(1, 3)).ToString(), "#" + ColorUtility.ToHtmlStringRGBA(Color.gray));
        level._startPoints.Add((new Vector2(4, 4)).ToString(), "#" + ColorUtility.ToHtmlStringRGBA(Color.gray));
    }
    void SaveLevelAsJson(Level level)
    {
        string json = JsonConvert.SerializeObject(level);
        //Debug.Log("data path " + Application.dataPath + "/Resources/LevelData/");
        File.WriteAllText(_levelDataPath + level._levelName + ".txt", json);
    }
    private void Start()
    {
        _levelDataPath = Application.dataPath + "/Resources/LevelData/";
        CreateLevel();
        SaveLevelAsJson(level);
    }
}
