using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EngineerDialData", menuName = "CaptainSonar/EngineerDialData")]
public class EngineerDialData : ScriptableObject
{
    public DialObjectData[] tab;
}

[System.Serializable]
public class DialObjectData
{
    public int index;
    public string color;
    public bool failureFlag;
    public int pathIndex;

    public DialObjectData(int index, string color, bool failureFlag, int pathIndex)
    {
        this.index = index;
        this.color = color;
        this.failureFlag = failureFlag;
        this.pathIndex = pathIndex;
    }
}