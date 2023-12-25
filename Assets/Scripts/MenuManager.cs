using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public static MenuManager menuManager;
    public GameObject _levelMenu;
    public GameObject _mainCanvas;
    public GameObject _ingameMenu;
    public GameObject _playgameMenu;
    private void Start()
    {
        menuManager = this;
    }
    public void ToggleLevelMenu(bool enabled)
    {
        _levelMenu.SetActive(enabled);
    }
    public void ToggleInGameMenu(bool enabled)
    {
        _ingameMenu.SetActive(enabled);
    }
    public void TogglePlayGameMenu(bool enabled)
    {
        _playgameMenu.SetActive(enabled);
    }
    public void ResetLevel()
    {
        SfxManager.sfxManager.PlayClickAudio();
        GameLogicManager.gameLogicManager.ResetBoard();
        LevelManager.levelManager.ResetLevel();
        LevelManager.levelManager.ToggleLevelCompleteUI(false);
        GameLogicManager.gameLogicManager._currentLevelComplete = false;
    }
    public void GoBackToLevelsMenu()
    {
        SfxManager.sfxManager.PlayClickAudio();
        ResetLevel();
        LevelManager.levelManager.ToggleLevelCompleteUI(false);
        ToggleInGameMenu(false);
        ToggleLevelMenu(true);
    }
    public void CloseGame()
    {
        Application.Quit();
    }
    public void StartGame()
    {
        SfxManager.sfxManager.PlayClickAudio();
        TogglePlayGameMenu(false);
        ToggleLevelMenu(true);
        GridManager.gridManager.GenerateGrid();
        LevelButtonGenerator.levelButtonGenerator.GenerateLevelButtons();
    }

}
