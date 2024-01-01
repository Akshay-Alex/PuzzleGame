using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ColorLine : MonoBehaviour
{
    public LineRenderer _lineRenderer;
    public List<Vector3> _points;
    //public List<Tile> _tilesFilled;
    public int _nextPosition;
    public Tile _currentSelectedTile, _currentStartTile;
    public bool isCompleted;
    // Start is called before the first frame update
    void Start()
    {
        _nextPosition = 0;
        isCompleted = false;
        //_tilesFilled = new List<Tile>();
        //_currentSelectedTilePosition = this.transform.position;
    }
    public void DeleteAllPoints()
    {
        _points.Clear();
        _lineRenderer.positionCount = 0;
        //ToggleHighlightOfTiles(false);
    }
    public void SetColor(Color color)
    {
        _lineRenderer.startColor = color;
        _lineRenderer.endColor = color;
    }
    public bool IsEndTile(Tile tile)
    {
        if (GetColor() == tile._startingTileColor && tile._isStartingTile && tile != _currentStartTile)
        {
            return true;
        }
        return false;
    }
    public Color GetColor()
    {
        return _lineRenderer.startColor;
    }
    public void SetFirstAndLastTileAsStartingTile()
    {
        DisableStartTiles();
        Tile FirstTile = GridManager.gridManager.GetTileAtPosition(new Vector2(_points[0].x, _points[0].y));
        Tile LastTile = GridManager.gridManager.GetTileAtPosition(new Vector2(_points.Last().x, _points.Last().y));
        FirstTile.SetAsStartTile(GetColor());
        LastTile.SetAsStartTile(GetColor());
    }
    public void SetTileAsCurrentSelectedTile(Tile tile)
    {
        _currentSelectedTile = tile;
    }
    public void AddNewPoint(Tile endTile)
    {
        if ((!_points.Contains(endTile.transform.position)) && IsNextPositionAdjacent(endTile.transform.position) && TileCanBeFilled(endTile))
        {
            if (endTile._isLineDrawnThroughTile)
            {
                ColorLine OtherLine = GameLogicManager.gameLogicManager.FindColorLineOfColor(endTile.FilledColor);
                OtherLine.RemoveLineFromTilesFromSpecificTile(endTile);
                Tile LastTile = GridManager.gridManager.GetTileAtPosition(new Vector2(OtherLine._points.Last().x, OtherLine._points.Last().y));
                LastTile.SetAsStartTile(OtherLine.GetColor());
                //SetFirstAndLastTileAsStartingTile();
            }
            _lineRenderer.positionCount++;
            _points.Add(endTile.transform.position);
            endTile._isLineDrawnThroughTile = true;
            _lineRenderer.SetPositions(_points.ToArray());
            _currentSelectedTile = endTile;
            //_tilesFilled.Add(endTile);
            endTile.FilledColor = GetColor();
            SetFirstAndLastTileAsStartingTile();

        }
        else if (endTile._isPermanentStartTile)
        {
            SetFirstAndLastTileAsStartingTile();
            //Tile LastTile = GridManager.gridManager.GetTileAtPosition(new Vector2(_points.Last().x, _points.Last().y));
            //_currentSelectedTile = LastTile;
            //do nothing
        }
        else if (_points.Contains(endTile.transform.position))
        {
            RemoveLineFromTilesAfterSpecificTile(endTile);
        }
    }
    void RemoveLineFromTilesAfterSpecificTile(Tile tile)
    {
        Tile LastTile = GridManager.gridManager.GetTileAtPosition(new Vector2(_points.Last().x, _points.Last().y));
        if(!LastTile._isPermanentStartTile)
        {
            LastTile.DisableStartTile();
        }      
        tile.SetAsStartTile(GetColor());
        int _specificTileIndex = _points.FindIndex(tilePosition => tilePosition == tile.transform.position);
        int _numberOfPointsRemoved = 0;
        int _numberOfPoints = _points.Count;
        for (int startIndex = _specificTileIndex + 1; startIndex < _numberOfPoints; startIndex++)
        {
            Tile tileToRemoveLineFrom = GridManager.gridManager.GetTileAtPosition(_points[_specificTileIndex + 1]);
            tileToRemoveLineFrom._isLineDrawnThroughTile = false;
            _points.RemoveAt(_specificTileIndex + 1);
            _numberOfPointsRemoved++;
            //_lineRenderer.positionCount--;
        }
        _lineRenderer.positionCount -= _numberOfPointsRemoved;
        //_lineRenderer.positionCount--;
        _lineRenderer.SetPositions(_points.ToArray());
        _currentSelectedTile = tile;

    }
    void RemoveLineFromTilesFromSpecificTile(Tile tile)
    {
        Tile LastTile = GridManager.gridManager.GetTileAtPosition(new Vector2(_points.Last().x, _points.Last().y));
        if (!LastTile._isPermanentStartTile)
        {
            LastTile.DisableStartTile();
        }
        tile.SetAsStartTile(GetColor());
        int _specificTileIndex = _points.FindIndex(tilePosition => tilePosition == tile.transform.position);
        int _numberOfPointsRemoved = 0;
        int _numberOfPoints = _points.Count;
        for (int startIndex = _specificTileIndex; startIndex < _numberOfPoints; startIndex++)
        {
            Tile tileToRemoveLineFrom = GridManager.gridManager.GetTileAtPosition(_points[_specificTileIndex]);
            tileToRemoveLineFrom._isLineDrawnThroughTile = false;
            _points.RemoveAt(_specificTileIndex);
            _numberOfPointsRemoved++;
            //_lineRenderer.positionCount--;
        }
        _lineRenderer.positionCount -= _numberOfPointsRemoved;
        //_lineRenderer.positionCount--;
        _lineRenderer.SetPositions(_points.ToArray());
        _currentSelectedTile = tile;

    }
    public void DisableStartTiles()
    {
        foreach (Vector3 point in _points)
        {
            Tile tile = GridManager.gridManager.GetTileAtPosition(new Vector2(point.x, point.y));
            tile.DisableStartTile();
        }
    }
    public void FinishLine(Tile tile)
    {
        AddNewPoint(tile);
        //ToggleHighlightOfTiles(false);
        DisableStartingTileOfCurrentColor();
        _currentSelectedTile = null;
        _currentStartTile = null;
        LevelManager.levelManager.ColorCompleted();
        LevelManager.levelManager.CheckIfAllLinesConnected();
    }
    public void DisableStartingTileOfCurrentColor()
    {
        foreach (Vector3 point in _points)
        {
            var tile = GridManager.gridManager.GetTileAtPosition(new Vector2(point.x, point.y));
            tile._isStartingTile = false;
        }
    }
    /*
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
    */
    public void AddStartingTileToLine(Tile tile)
    {
        tile._isLineDrawnThroughTile = true;
        tile.FilledColor = GetColor();
        _lineRenderer.positionCount++;
        _points.Add(tile.transform.position);
        _lineRenderer.SetPositions(_points.ToArray());
        //_tilesFilled.Add(tile);
        //_tilesFilled.Add(tile);
    }
    bool IsNextPositionInSameRow(Vector3 position)
    {
        if (_currentSelectedTile.transform.position.y == position.y)
        {
            return true;
        }
        return false;
    }
    bool IsNextPositionAdjacent(Vector3 position)
    {
        if ((IsNextTileInSameColumn(position) && !IsNextTileInSameRow(position)) || (!IsNextTileInSameColumn(position) && IsNextTileInSameRow(position)))
            return true;
        return false;
    }
    bool IsNextTileInSameColumn(Vector3 position)
    {
        if (Mathf.Abs(Mathf.Abs(_currentSelectedTile.transform.position.y) - Mathf.Abs(position.y)) == 1)
            return true;
        return false;
    }
    bool IsNextTileInSameRow(Vector3 position)
    {
        if (Mathf.Abs(Mathf.Abs(_currentSelectedTile.transform.position.x) - Mathf.Abs(position.x)) == 1)
            return true;
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
    /*
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
    */
    bool TileCannotBeFilled(Tile tile)
    {
        if ((tile._isLineDrawnThroughTile && (tile != _currentSelectedTile)) || tile._isStartingTile && tile._startingTileColor != GetColor())
            return true;
        return false;
    }
    bool TileCanBeFilled(Tile tile)
    {
        if (tile._isPermanentStartTile && tile._startingTileColor != GetColor())
            return false;
        return true;
    }
    public void ResetLine()
    {
        foreach (Vector3 point in _points)
        {
            Tile tile = GridManager.gridManager.GetTileAtPosition(new Vector2(point.x, point.y));
            tile._isLineDrawnThroughTile = false;
            if (tile._isStartingTile && !tile._isPermanentStartTile)
            {
                tile._isStartingTile = false;
                tile.DisableStartTile();
            }
        }
        GameLogicManager.gameLogicManager._colorLines.Remove(this);
        Destroy(GameLogicManager.gameLogicManager._currentLine.gameObject);
        GameLogicManager.gameLogicManager._currentLine = null;
    }
    /*
    bool TileCanBeFilled(Tile tile)
    {
        if (tile._isStartingTile && tile._startingTileColor != GetColor())
            return false;
        return true;
    }
    */
    /*
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
    */
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
