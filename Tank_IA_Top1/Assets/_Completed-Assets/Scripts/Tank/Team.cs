using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class Team 
{
    public string teamName;
    public Color teamColor;
    public float points = 0;
    public Text teamNameTextUI;
    public Text teamPointsTextUI;
    public Complete.TankManager[] Tanks;
    public int teamPoints { get { return (int)points; } }

    public void AddPoint(float point)
    {
        points += point;
        teamPointsTextUI.text = FormatPoints();
    }


    string FormatPoints()
    {
        int thousands = teamPoints / 1000;
        string rest = (teamPoints % 1000).ToString();
        if (thousands > 0)
        {
            switch (rest.Length)
            {
                case 1:
                    rest = $"00{rest}";
                    break;
                case 2:
                    rest = $"0{rest}";
                    break;
            }
        }
        return thousands == 0 ? $"{rest}" : $"{thousands} {rest}";
    }
    
}
