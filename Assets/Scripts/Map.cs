using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    //create class for maps :
    #region attributs
    private string nameMap;
    private int numberMap;
    private int[,] map; // Either 10x10 or 15x15 (4 or 9 sectors per map of size 5x5)
    private bool realTime; //True if realtime, false if turn by turn
    #endregion

    #region Get
    public string GetNameMap() { return nameMap; }
    public int GetNumberMap() { return numberMap; }
    public int[,] GetMap() { return map; }
    public bool GetRealTime() { return realTime; }
    #endregion

    #region Set
    public void SetNameMap(string name) { nameMap = name; }
    public void SetNumberMap(int number) { numberMap = number; }
    public void SetMap(int[,] chosenMap) { map = chosenMap; }
    public void SetRealTime(bool isRealTime) { realTime = isRealTime; }
    #endregion

    #region Constructeurs
    public Map(int chosenMap, bool mode)
    {
        numberMap = chosenMap;
        realTime = mode;

        if (realTime) map = new int[15, 15];
        else map = new int[10, 10];

        if (numberMap==1)
        {
            nameMap = "Alpha-2";
            if (realTime)
            {
                map[1, 1] = 1;
                map[1, 2] = 1;
                map[1, 6] = 1;
                map[1, 11] = 1;
                map[2, 1] = 1;
                map[2, 8] = 1;
                map[2, 11] = 1;
                map[3, 8] = 1;

                map[5, 4] = 1;
                map[5, 14] = 1;
                map[6, 2] = 1;
                map[6, 11] = 1;
                map[6, 1] = 1;
                map[7, 6] = 1;
                map[7, 7] = 1;
                map[7, 8] = 1;
                map[7, 11] = 1;
                map[8, 3] = 1;
                map[8, 12] = 1;
                map[9, 4] = 1;

                map[10, 1] = 1;
                map[10, 7] = 1;
                map[10, 10] = 1;
                map[11, 1] = 1;
                map[11, 3] = 1;
                map[11, 13] = 1;
                map[12, 3] = 1;
                map[12, 6] = 1;
                map[13, 3] = 1;
                map[13, 7] = 1;
                map[13, 13] = 1;
            }
            else
            {
                map[1, 5] = 1;
                map[2, 1] = 1;
                map[3, 8] = 1;
                map[4, 3] = 1;
                map[5, 1] = 1;
                map[6, 8] = 1;
                map[7, 3] = 1;
                map[7, 5] = 1;
            }
        }
        else if (numberMap == 2)
        {
            this.nameMap = "Bravo-2";
            if (realTime)
            {
            }
            else
            {
                map[1, 8] = 1;
                map[2, 1] = 1;
                map[3, 6] = 1;
                map[4, 4] = 1;
                map[6, 6] = 1;
                map[7, 2] = 1;
            }
        }
        else if (numberMap == 3)
        {
            nameMap = "Charlie-2";
            if (realTime)
            {
            }
            else
            {
                map[2, 3] = 1;
                map[2, 4] = 1;
                map[2, 5] = 1;
                map[6, 1] = 1;
                map[7, 6] = 1;
                map[8, 6] = 1;
                map[8, 7] = 1;
            }
        }

    }
    #endregion

    public int Find_Sector(Position position)
    {
        int sector;
        if (realTime)
        {
            if (position.x <4 && position.y<4) sector = 1;
            else if (position.x < 4 && position.y < 9) sector = 2;
            else if (position.x < 4) sector = 3;

            else if (position.x < 9 && position.y < 4) sector = 4;
            else if (position.x < 9 && position.y < 9) sector = 5;
            else if (position.x < 9) sector = 6;

            else if (position.y < 4) sector = 7;
            else if (position.y < 9) sector = 8;
            else sector = 9;
        }
        else
        {
            if (position.x < 4 && position.y < 4) sector = 1;
            else if (position.x < 4) sector = 2;

            else if (position.y < 4) sector = 3;
            else sector = 4;
        }
        return sector;
    }
}
