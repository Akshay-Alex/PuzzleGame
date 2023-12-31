using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameLogicManager : MonoBehaviour
{
    public Color[] _colors;
    public ColorLine _currentLine;
    public List<ColorLine> _colorLines;
    public ColorLine _colorLinePrefab;
    public static GameLogicManager gameLogicManager;
    public bool _startTileSelected;
    public bool _currentLevelComplete;
    // Start is called before the first frame update
    void Start()
    {
        _startTileSelected = false;
        gameLogicManager = this;
        _currentLevelComplete = false;
        //CreateColorLines();
    }
    public void CreateColorLine(Color color, Tile startTile)
    {
        var colorLine = Instantiate(_colorLinePrefab, Vector3.one, Quaternion.identity, this.transform);
        SetLineColor(colorLine, color);
        _colorLines.Add(colorLine);
        _currentLine = colorLine;
        _currentLine._currentSelectedTile = startTile;
        _currentLine._currentStartTile = startTile;
        _currentLine.AddStartingTileToLine(startTile);
    }
    public bool IsNotStartingTile(Tile tile)
    {
        if(!tile._isStartingTile)
        {
            return true;
        }
        return false;
    }
    public void GetCurrentLineColor()
    {
        _currentLine.GetColor();
    }
    void SetLineColor(ColorLine colorLine, Color color)
    {
        colorLine.SetColor(color);
    }

    //start of logic
    public void TileSelected(Tile _selectedtile)
    {
        if(!_currentLevelComplete)
        {
            SfxManager.sfxManager.PlayClickAudio();
            if (_startTileSelected)
            {
                TryToCreatePath(_selectedtile);
            }
            else
            {
                TryToSelectStartTile(_selectedtile);
            }
        }
        UpdateBoardFillPercentageText();
    }

    void TryToCreatePath(Tile tile)
    {
        if(tile == _currentLine._currentSelectedTile)
        {
            //do nothing. managing the edge case where user clicks the selected tile again.
        }
        else
        {
            if(IsAnotherStartingTile(tile))
            {
                ReplaceStartingTile(tile);
                //TryToSelectStartTile(tile);
            }
            else if(IsNotStartingTile(tile))
            {
                _currentLine.AddNewPoint(tile);
            }
            else if(IsEndTile(tile))
            {
                if(_currentLine._tilesInSameLine.Contains(tile))
                {
                    _startTileSelected = false;
                    _currentLine.FinishLine(tile);
                }            
            }
            
        }
    }
    bool IsEndTile(Tile tile)
    {
        if(tile._isStartingTile && !(_currentLine._points.Contains(tile.transform.position)))
        {
            return true;
        }
        return false;
    }
    void ReplaceStartingTile(Tile tile)
    {
        ResetTilesOfCurrentLine();
        _currentLine.DeleteAllPoints();
        tile.ToggleTileHighlight(true);
        CreateColorLine(tile._startingTileColor, tile);
    }
    void ResetTilesOfCurrentLine()
    {
        foreach (Vector3 position in _currentLine._points)
        {
            var tile = GridManager.gridManager.GetTileAtPosition(new Vector2(position.x, position.y));
            tile._isLineDrawnThroughTile = false;
        }
    }
    public bool IsAnotherStartingTile(Tile tile)
    {
        if(tile._isStartingTile && tile._startingTileColor != _currentLine.GetColor())
        {
            return true;
        }
        return false;
    }
    void TryToSelectStartTile(Tile tile)
    {
        if(tile._isStartingTile)
        {
            _startTileSelected = true;
            tile.ToggleTileHighlight(true);
            CreateColorLine(tile._startingTileColor, tile);
        }
        else
        {
            //do nothing. Ignore the click
        }
    }
    void UpdateBoardFillPercentageText()
    {
        LevelManager.levelManager._boardFillPercentageText.text = "Board fill percentage : "+ GridManager.gridManager.CalculateBoardFillPercentage() + "%";
    }
    public void ResetBoard()
    {
        ResetLines();
        _startTileSelected = false;
        _currentLine = null;   
        GridManager.gridManager.ResetTiles();
        UpdateBoardFillPercentageText();
    }
    public void FinishLevel()
    {
        SfxManager.sfxManager.PlayWinAudio();
        _currentLevelComplete = true;
        LevelManager.levelManager.FinishLevel();
    }
    void ResetLines()
    {
        if(_currentLine)
        {
            _currentLine.DeselectCurrentStartTile();
        }    
        foreach (ColorLine colorLine in _colorLines)
        {
            Destroy(colorLine.gameObject);
        }
        _colorLines.Clear();
    }
    public void MouseHoveredOverTile(Tile tile)
    {
        if(_currentLine && _currentLine._currentSelectedTile)
        {
            _currentLine.HighlightPossiblePath(tile);
        }
    }
    // Update is called once per frame
    void Update()
    {

    }
}
