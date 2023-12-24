using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LineRendererManager : MonoBehaviour
{
    public Color[] _colors;
    public ColorLine _currentLine;
    public List<ColorLine> _colorLines;
    public ColorLine _colorLinePrefab;
    public static LineRendererManager lineRendererManager;
    // Start is called before the first frame update
    void Start()
    {
        lineRendererManager = this;
        //CreateColorLines();
    }
    void CreateColorLines()
    {
        foreach (Color color in _colors)
        {
            var colorLine = Instantiate(_colorLinePrefab, Vector3.one, Quaternion.identity, this.transform);
            SetLineColor(colorLine, color);
            _colorLines.Add(colorLine);
        }
        //_currentLine = _colorLines.LastOrDefault();
    }
    public void CreateColorLine(Color color, Tile startTile)
    {
        var colorLine = Instantiate(_colorLinePrefab, Vector3.one, Quaternion.identity, this.transform);
        SetLineColor(colorLine, color);
        _colorLines.Add(colorLine);
        _currentLine = colorLine;
        _currentLine._currentSelectedTile = startTile;
        _currentLine.AddStartingTileToLine(startTile);
    }
    public void GetCurrentLineColor()
    {
        _currentLine.GetColor();
    }
    void SetLineColor(ColorLine colorLine,Color color)
    {
        colorLine.SetColor(color);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
