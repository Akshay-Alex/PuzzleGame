using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LevelButtonGenerator : MonoBehaviour
{
    public static LevelButtonGenerator levelButtonGenerator;
    public Transform _buttonsParentObject;
    public GameObject _levelButtonPrefab;
    int _levelCount;
   public void GenerateLevelButtons()
    {
        var info = new DirectoryInfo(LevelManager.levelManager._levelDataPath);
        var allLevelFiles = info.GetFiles("*.txt");
        foreach(FileInfo file in allLevelFiles)
        {
            var button = GameObject.Instantiate(_levelButtonPrefab, _buttonsParentObject);
            var buttonData = button.GetComponent<LevelButtonData>();
            buttonData._text.text = _levelCount.ToString();
            buttonData._levelDataFileName = file.Name;
            button.gameObject.name = file.Name;
            _levelCount++;
        }
    }
    private void Start()
    {
        _levelCount = 1;
        levelButtonGenerator = this;
    }
}
