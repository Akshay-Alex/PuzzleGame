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
        _dotSprite.gameObject.SetActive(true);
    }
    void OnMouseEnter()
    {
        //_highlight.SetActive(true);
        if (LineRendererManager.lineRendererManager._currentLine)
        {
            LineRendererManager.lineRendererManager._currentLine.HighlightPossiblePath(this);
            //HighlightCurrentEndTile();
        }
    }
    void HighlightCurrentEndTile()
    {
        if (_isStartingTile && LineRendererManager.lineRendererManager._currentLine.GetColor() == _startingTileColor)
        {
            ToggleTileHighlight(true);
        }
    }
    void UnHighlightCurrentEndTile()
    {
        if (_isStartingTile && LineRendererManager.lineRendererManager._currentLine.GetColor() == _startingTileColor)
        {
            ToggleTileHighlight(false);
        }
    }

    void OnMouseExit()
    {
        //UnHighlightCurrentEndTile();
    }
    private void OnMouseDown()
    {
        if(LineRendererManager.lineRendererManager._currentLine && LineRendererManager.lineRendererManager._currentLine._currentSelectedTile == this)
        {
            return;
        }
        if (_isStartingTile)
        {
            if (LineRendererManager.lineRendererManager._currentLine)
            {
                if (LineRendererManager.lineRendererManager._currentLine.GetColor() != _startingTileColor)
                {
                    LineRendererManager.lineRendererManager._currentLine.DeleteAllPoints();
                    LineRendererManager.lineRendererManager.CreateColorLine(_startingTileColor, this);
                }
                else
                {
                    Debug.Log("Should complete now");
                    LineRendererManager.lineRendererManager._currentLine.FinishLine(this);                  
                    LineRendererManager.lineRendererManager._currentLine._currentSelectedTile = null;
                    LineRendererManager.lineRendererManager._currentLine = null;
                }
            }
            else
            {
                LineRendererManager.lineRendererManager.CreateColorLine(_startingTileColor,this);
                //Debug.Log("Starting tile position :"+this.transform.position);
            }

        }
        else
        {
            if(!_isLineDrawnThroughTile && LineRendererManager.lineRendererManager._currentLine)
            {
                LineRendererManager.lineRendererManager._currentLine.AddNewPoint(this);
            }
        }
    }
}