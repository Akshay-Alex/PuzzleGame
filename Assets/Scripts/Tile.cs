using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private Color _baseColor, _offsetColor;
    [SerializeField] private SpriteRenderer _renderer,_dotSprite;
    [SerializeField] private GameObject _highlight;
    
    public bool _isLineDrawnThroughTile;
    public bool _isStartingTile;
    public Color _startingTileColor;

    public void Init(bool isOffset)
    {
        _renderer.color = isOffset ? _offsetColor : _baseColor;
        _isLineDrawnThroughTile = false;
        _isStartingTile = false;
    }
    public void ToggleTileHighlight(bool on)
    {
        _highlight.SetActive(on);
    }
    public void SetAsStartTile(Color color)
    {
        _startingTileColor = color;
        _dotSprite.color = color;
        ToggleCircle(true);
    }
    public void ToggleCircle(bool enabled)
    {
        _dotSprite.gameObject.SetActive(enabled);
    }
    void OnMouseEnter()
    {
        GameLogicManager.gameLogicManager.MouseHoveredOverTile(this);
    }

    void OnMouseExit()
    {
        //UnHighlightCurrentEndTile();
    }
    private void OnMouseDown()
    {
        GameLogicManager.gameLogicManager.TileSelected(this);
        /*
        if (GameLogicManager.gameLogicManager._currentLine && GameLogicManager.gameLogicManager._currentLine._currentSelectedTile == this)
        {
            return;
        }
        if (_isStartingTile)
        {
            if (GameLogicManager.gameLogicManager._currentLine)
            {
                if (GameLogicManager.gameLogicManager._currentLine.GetColor() != _startingTileColor)
                {
                    GameLogicManager.gameLogicManager._currentLine.DeleteAllPoints();
                    GameLogicManager.gameLogicManager.CreateColorLine(_startingTileColor, this);
                }
                else
                {
                    Debug.Log("Should complete now");
                    GameLogicManager.gameLogicManager._currentLine.FinishLine(this);                  
                    GameLogicManager.gameLogicManager._currentLine._currentSelectedTile = null;
                    GameLogicManager.gameLogicManager._currentLine = null;
                }
            }
            else
            {
                GameLogicManager.gameLogicManager.CreateColorLine(_startingTileColor,this);
                //Debug.Log("Starting tile position :"+this.transform.position);
            }

        }
        else
        {
            if(!_isLineDrawnThroughTile && GameLogicManager.gameLogicManager._currentLine)
            {
                GameLogicManager.gameLogicManager._currentLine.AddNewPoint(this);
            }
        }
        */
    }
}