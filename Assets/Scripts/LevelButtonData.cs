using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelButtonData : MonoBehaviour
{
    //public Level _level;
    public string _levelDataFileName;
    public TextMeshProUGUI _text;

    public void LoadThisLevel()
    {
        //SfxManager.sfxManager.PlayClickAudio();
        Debug.Log(_levelDataFileName);
        LevelManager.levelManager.ReadAndLoadLevelFromResources(_levelDataFileName);
    }
}
