using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorLine : MonoBehaviour
{
    public LineRenderer _lineRenderer;
    public List<Vector3> _points;
    public List<Tile> _tilesInSameLine;
    public int _nextPosition;
    public Tile _currentSelectedTile,_currentStartTile;
    public bool isCompleted;
    // Start is called before the first frame update
    void Start()
    {
        _nextPosition = 0;
        isCompleted = false;
        _tilesInSameLine = new List<Tile>();
        //_currentSelectedTilePosition = this.transform.position;
    }
    public void DeleteAllPoints()
    {
        _points.Clear();
        _lineRenderer.positionCount = 0;
        ToggleHighlightOfTiles(false);
    }
    public void SetColor(Color color)
    {
        _lineRenderer.startColor = color;
        _lineRenderer.endColor = color;
    }
    public bool IsEndTile(Tile tile)
    {
        if(GetColor() == tile._startingTileColor && tile._isStartingTile && tile!=_currentStartTile)
        {
            return true;
        }
        return false;
    }
    public Color GetColor()
    {
        return _lineRenderer.startColor;
    }
    public void AddNewPoint(Tile endTile)
    {
        if((!_points.Contains(endTile.transform.position)) && (IsNextPositionInSameRow(endTile.transform.position) || IsNextPositionInSameColumn(endTile.transform.position)))
        {
            foreach(Tile tile in _tilesInSameLine)
            {
                if(!_points.Contains(tile.transform.position) && (tile != _currentSelectedTile))
                {
                    //Debug.Log("tile " + tile.gameObject.name + " position :" + tile.transform.position);
                    _lineRenderer.positionCount++;
                    //Debug.Log("position count after increment " + _lineRenderer.positionCount);
                    _points.Add(tile.transform.position);
                    tile._isLineDrawnThroughTile = true;
                    
                }                
            }
            _lineRenderer.SetPositions(_points.ToArray());
            _currentSelectedTile = endTile;
        } 
    }
    public void FinishLine(Tile tile)
    {
        AddNewPoint(tile);
        ToggleHighlightOfTiles(false);
        _currentSelectedTile = null;
        _currentStartTile = null;
        LevelManager.levelManager.ColorCompleted();
        LevelManager.levelManager.CheckIfAllLinesConnected();
    }
    public void DeselectCurrentStartTile()
    {
        if(_currentStartTile)
        {
            _currentStartTile.ToggleTileHighlight(false);
        }
        ResetHighlight();
        //_currentStartTile.ToggleTileHighlight(false);
        _currentSelectedTile = null;
        _currentStartTile = null;
    }
    public void AddStartingTileToLine(Tile tile)
    {
        tile._isLineDrawnThroughTile = true;
        _lineRenderer.positionCount++;
        _points.Add(tile.transform.position);
        _lineRenderer.SetPositions(_points.ToArray());
    }
    bool IsNextPositionInSameRow(Vector3 position)
    {
        if(_currentSelectedTile.transform.position.y == position.y)
        {
            return true;
        }
        return false;
    }
    bool IsNextPositionInSameColumn(Vector3 position)
    {
        if (_currentSelectedTile.transform.position.x == position.x)
        {
            return true;
        }
        return false;
    }
    bool CanLineBeDrawn(Vector3 position)
    {
        //if(_currentSelectedTilePosition == position)
        {
           // return false;
        }
        if(IsNextPositionInSameRow(position))
        {
            FindTilesInSameRow(position);
            return true;
        }
        if(IsNextPositionInSameColumn(position))
        {
            FindTilesInSameColumn(position);
            return true;
        }
        ClearTilesInSameLine();
        return false;
    }
    void ClearTilesInSameLine()
    {
        ToggleHighlightOfTiles(false);
    }
    bool TileCannotBeFilled(Tile tile)
    {
        if ((tile._isLineDrawnThroughTile && (tile!=_currentSelectedTile)) || tile._isStartingTile && tile._startingTileColor != GetColor())
            return true;
        return false;
    }
    void FindTilesInSameRow(Vector3 position)
    {
        if(_currentSelectedTile.transform.position.x < position.x)
        {
            ResetHighlight();
            float currentStartTilePosition = _currentSelectedTile.transform.position.x;
            while(currentStartTilePosition <= position.x)
            {
                var TileAtPosition = GridManager.gridManager.GetTileAtPosition(new Vector2(currentStartTilePosition, position.y));
                if (TileAtPosition)
                {
                    if (TileCannotBeFilled(TileAtPosition))
                    {
                        break;
                    }
                    _tilesInSameLine.Add(TileAtPosition);
                    currentStartTilePosition += 1;
                    if(IsEndTile(TileAtPosition))
                    {
                        break;
                    }
                }
            }
        }
        else if(_currentSelectedTile.transform.position.x > position.x)
        {
            ResetHighlight();
            float currentStartTilePosition = _currentSelectedTile.transform.position.x;
            while (currentStartTilePosition >= position.x)
            {
                var TileAtPosition = GridManager.gridManager.GetTileAtPosition(new Vector2(currentStartTilePosition, position.y));
                if (TileAtPosition)
                {
                    if (TileCannotBeFilled(TileAtPosition))
                    {
                        break;
                    }
                    _tilesInSameLine.Add(TileAtPosition);
                    currentStartTilePosition -= 1;
                    if (IsEndTile(TileAtPosition))
                    {
                        break;
                    }
                }               
            }
        }
    }
    void FindTilesInSameColumn(Vector3 position)
    {
        if (_currentSelectedTile.transform.position.y < position.y)
        {
            ResetHighlight();
            float currentStartTilePosition = _currentSelectedTile.transform.position.y;
            while (currentStartTilePosition <= position.y)
            {
                var TileAtPosition = GridManager.gridManager.GetTileAtPosition(new Vector2(position.x, currentStartTilePosition));
                if (TileAtPosition)
                {
                    if (TileCannotBeFilled(TileAtPosition))
                    {
                        break;
                    }
                    _tilesInSameLine.Add(TileAtPosition);
                    currentStartTilePosition += 1;
                    if (IsEndTile(TileAtPosition))
                    {
                        break;
                    }
                }                         
            }
        }
        else if(_currentSelectedTile.transform.position.y > position.y)
        {
            ResetHighlight();
            float currentStartTilePosition = _currentSelectedTile.transform.position.y;
            while (currentStartTilePosition >= position.y)
            {
                var TileAtPosition = GridManager.gridManager.GetTileAtPosition(new Vector2(position.x, currentStartTilePosition));               
                if (TileAtPosition)
                {
                    if (TileCannotBeFilled(TileAtPosition))
                    {
                        break;
                    }
                    _tilesInSameLine.Add(TileAtPosition);
                    currentStartTilePosition -= 1;
                    if (IsEndTile(TileAtPosition))
                    {
                        break;
                    }
                }                     
            }
        }
    }
    void ResetHighlight()
    {
        ToggleHighlightOfTiles(false);
        _tilesInSameLine.Clear();
    }
    public void HighlightPossiblePath(Tile hoveredTile)
    {
        if(CanLineBeDrawn(hoveredTile.transform.position))
        {
            ToggleHighlightOfTiles(true);
        }
    }
    void ToggleHighlightOfTiles(bool on)
    {
        foreach (Tile tile in _tilesInSameLine)
        {
            tile.ToggleTileHighlight(on);
        }
    }
    /*
    public void AddNewPoint(Vector3 position)
    {
        if(_nextPosition <= 0)
        {
            InitializeLine(position);
        }
        else
        {
            _lineRenderer.SetPosition(_nextPosition, position);
        }     
        _nextPosition++;
    }
    
    void InitializeLine(Vector3 startPosition)
    {
        var currentIndex = 0;
        while(currentIndex < _lineRenderer.positionCount)
        {
            _lineRenderer.SetPosition(currentIndex, startPosition);
            currentIndex++;
        }
    }*/
    // Update is called once per frame
    void Update()
    {
        
    }
}
