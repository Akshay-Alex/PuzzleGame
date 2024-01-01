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
        colorLine._currentSelectedTile = startTile;
        colorLine._currentStartTile = startTile;
        colorLine.AddStartingTileToLine(startTile);
        _currentLine = colorLine;

    }
    public bool IsNotStartingTile(Tile tile)
    {
        if (!tile._isStartingTile)
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
    /*
    //start of logic
    public void TileSelected(Tile _selectedtile)
    {
        if (!_currentLevelComplete)
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
    */
    public void OnTileClicked(Tile tile)
    {
        if (!_currentLevelComplete)
        {
            SfxManager.sfxManager.PlayClickAudio();
            TryToSelectStartTile(tile);
            UpdateBoardFillPercentageText();
        }

    }
    public void OnDraggedOverTile(Tile tile)
    {
        Debug.Log("OnDraggedOverTile called");
        if (!_currentLevelComplete)
        {
            //SfxManager.sfxManager.PlayClickAudio();
            if (_startTileSelected && _currentLine.IsNextPositionAdjacent(tile.transform.position))
            {
                TryToCreatePath(tile);
            }
            UpdateBoardFillPercentageText();
        }
    }
    void TryToCreatePath(Tile tile)
    {
        if (tile == _currentLine._currentSelectedTile || IsAnotherStartingTile(tile))
        {
            //do nothing. managing the edge case where user clicks the selected tile again.
        }
        else
        {
            if (IsEndTile(tile))
            {
                //_currentLine.AddNewPoint(tile);
                _startTileSelected = false;
                _currentLine.FinishLine(tile);
            }
            else
            {
                _currentLine.AddNewPoint(tile);
            }

        }
    }


    bool IsEndTile(Tile tile)
    {
        if (tile._isPermanentStartTile && (TileIsNotInAnyLine(tile)) && (_currentLine && (_currentLine.GetColor() == tile._startingTileColor)))
        {
            return true;
        }
        return false;
    }
    bool TileIsNotInAnyLine(Tile tile)
    {
        foreach (ColorLine line in _colorLines)
        {
            if(line._points.Contains(tile.transform.position))
            {
                return false;
            }
        }
        return true;
    }
    void ReplaceStartingTile(Tile tile)
    {
        //ResetTilesOfCurrentLine();
        //_currentLine.DeleteAllPoints();
        //tile.ToggleTileHighlight(true);
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
        if (tile._isStartingTile && tile._startingTileColor != _currentLine.GetColor())
        {
            return true;
        }
        return false;
    }
    /*
    void TryToSelectStartTile(Tile tile)
    {
        if (tile._isStartingTile)
        {
            _startTileSelected = true;
            if (IsEndTile(tile))
            {
                if(_currentLine)
                    _currentLine.ResetLine();
                CreateColorLine(tile._startingTileColor, tile);
            }
            else
            {
                if (CheckIfLineAlreadyExists(tile))
                {   
                    SwapCurrentLine(tile, tile._startingTileColor);
                }
                else
                {
                    CreateColorLine(tile._startingTileColor, tile);
                    //_currentLine._tilesFilled.Add(tile);
                }
            }
            

            //tile.ToggleTileHighlight(true);

        }
        else
        {
            //do nothing. Ignore the click
        }
    }
    */
    void TryToSelectStartTile(Tile tile)
    {
        if(tile._isStartingTile)
        {
            if(CheckIfLineAlreadyExists(tile))
            {
                ColorLine existingColorLine = FindColorLineOfColor(tile._startingTileColor);
                if (tile._isPermanentStartTile)
                {
                    existingColorLine.ResetLine();
                    CreateColorLine(tile._startingTileColor, tile);
                }
                else
                {
                    _currentLine = existingColorLine;
                    _currentLine._currentSelectedTile = tile;
                }
            }
            else
            {
                CreateColorLine(tile._startingTileColor, tile);
            }
            _startTileSelected = true;
        }
    }
    public void SwapCurrentLine(Tile clickedTile, Color newColor)
    {
        _currentLine._currentSelectedTile = null;
        ColorLine newCurrentline = FindColorLineOfColor(newColor);
        newCurrentline.SetTileAsCurrentSelectedTile(clickedTile);
        _currentLine = newCurrentline;          
    }
    bool CheckIfLineAlreadyExists(Tile tile)
    {
        foreach (ColorLine line in _colorLines)
        {
            if (line.GetColor() == tile._startingTileColor)
                return true;
        }
        return false;
    }
    public ColorLine FindColorLineOfColor(Color color)
    {
        foreach (ColorLine colorLine in _colorLines)
        {
            if (colorLine.GetColor() == color)
                return colorLine;
        }
        return null;
    }
    void UpdateBoardFillPercentageText()
    {
        LevelManager.levelManager._boardFillPercentageText.text = "Board fill percentage : " + GridManager.gridManager.CalculateBoardFillPercentage() + "%";
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
        /*
        if (_currentLine)
        {
            _currentLine.DeselectCurrentStartTile();
        }
        */
        foreach (ColorLine colorLine in _colorLines)
        {
            Destroy(colorLine.gameObject);
        }
        _colorLines.Clear();
    }
    /*
    public void MouseHoveredOverTile(Tile tile)
    {
        if (_currentLine && _currentLine._currentSelectedTile)
        {
            _currentLine.HighlightPossiblePath(tile);
        }
    }
    */
    // Update is called once per frame
    void Update()
    {

    }
}
