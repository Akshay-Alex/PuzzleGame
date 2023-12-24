using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManager;
    private void Start()
    {
        //LevelButtonGenerator.levelButtonGenerator.GenerateLevelButtons();
        //LevelManager.levelManager.ReadAndLoadLevel();
    }
    public void StartGame()
    {
        MenuManager.menuManager.TogglePlayGameMenu(false);
        GridManager.gridManager.GenerateGrid();
        LevelButtonGenerator.levelButtonGenerator.GenerateLevelButtons();
    }
}
