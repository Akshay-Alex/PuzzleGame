using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManager;
    private void Start()
    {
        GridManager.gridManager.GenerateGrid();
        LevelManager.levelManager.ReadAndLoadLevel();
    }
}
