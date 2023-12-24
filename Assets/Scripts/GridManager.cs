using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private int _width, _height;

    [SerializeField] private Tile _tilePrefab;

    private Transform _cam;

    private Dictionary<Vector2, Tile> _tiles;
    public static GridManager gridManager;

    void Start()
    {
        gridManager = this;
        FindCamera();
        //GenerateGrid();
        //InitializeLevel();
    }
    void FindCamera()
    {
        _cam = Camera.main.transform;
    }
    public void GenerateGrid()
    {
        
        _tiles = new Dictionary<Vector2, Tile>();
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                var spawnedTile = Instantiate(_tilePrefab, new Vector3(x, y), Quaternion.identity,this.transform);
                spawnedTile.name = $"Tile {x} {y}";

                var isOffset = (x % 2 == 0 && y % 2 != 0) || (x % 2 != 0 && y % 2 == 0);
                spawnedTile.Init(isOffset);


                _tiles[new Vector2(x, y)] = spawnedTile;
            }
        }
        //transform.position = new Vector3(-((float)_width / 2 - 0.5f), -((float)_height / 2 - 0.5f), -1);
        //_cam.transform.position = new Vector3(((float)_width / 2 - 0.5f), ((float)_height / 2 - 0.5f), 10);
        SetGridAsCenter();

    }
    public void ResetTiles()
    {
        foreach(KeyValuePair<Vector2,Tile> Tile in _tiles)
        {
            var tile = Tile.Value;
            tile._isStartingTile = false;
            tile._isLineDrawnThroughTile = false;
            tile.ToggleCircle(false);
        }
    }
    void SetGridAsCenter()
    {
        _cam.transform.position = new Vector3(((float)_width / 2 - 0.5f), ((float)_height / 2 - 0.5f), -10);
        MenuManager.menuManager._mainCanvas.transform.position = new Vector3(_cam.transform.position.x, _cam.transform.position.y, MenuManager.menuManager._mainCanvas.transform.position.z);
    }
    public Tile GetTileAtPosition(Vector2 pos)
    {
        if (_tiles.TryGetValue(pos, out var tile)) return tile;
        return null;
    }

    
}