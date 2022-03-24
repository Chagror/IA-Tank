using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Complete;
using System;

[CreateAssetMenu(menuName = "AI/Behavior Tree/Blackboard/Blackboard Rush Zone")]
public class BTBlackoardRushZone : BTBlackboard
{
    public override void Init(BTController controller)
    {
        values.Clear();
        values.Add("TargetEnemy", (GameObject)null);

        Complete.TankMovement currentTank = controller.GetComponent<Complete.TankMovement>();
        values.Add("RangeDetectEnemy", 20f);
        values.Add("Helipad", GameObject.Find("Helipad"));
        values.Add("ShootPrecision", 6f);
        
        List<GameObject> enemies = new List<GameObject>();
        List<GameObject> allies = new List<GameObject>();

        if(Complete.GameManager.instance.team1 == currentTank.tankTeam)
        {
            foreach (var tank in Complete.GameManager.instance.team2.Tanks)
            {
                enemies.Add(tank.m_Instance);
            }
            foreach (var tank in Complete.GameManager.instance.team1.Tanks)
            {
                allies.Add(tank.m_Instance);
            }
        }
        else
        {
            foreach (var tank in Complete.GameManager.instance.team1.Tanks)
            {
                enemies.Add(tank.m_Instance);
            }
            foreach (var tank in Complete.GameManager.instance.team2.Tanks)
            {
                allies.Add(tank.m_Instance);
            }
        }
        
        values.Add("Enemies", enemies);
        values.Add("Allies", allies);

        base.Init(controller);
    }
}
