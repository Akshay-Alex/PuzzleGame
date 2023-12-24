using System;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

[Serializable]
public class Level
{
    [SerializeField]
    public string _levelName;
    [SerializeField]
    public Dictionary<string, string> _startPoints;
    public Level()
    {
        _startPoints = new Dictionary<string, string>();
    }
}
