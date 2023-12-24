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

}
