using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Complete;
using System;

[CreateAssetMenu(menuName = "AI/Behavior Tree/Blackboard/Blackboard Switcher")]
public class BTBlackBoard_Switcher : BTBlackboard
{
    public override void Init(BTController controller)
    {
        values.Clear();
        Complete.TankMovement currentTank = controller.GetComponent<Complete.TankMovement>();
        values.Add("RangeDetectEnemy", 20f);
        values.Add("Helipad", GameObject.Find("Helipad"));
        values.Add("DistanceFromHelipad", 0f);
        values.Add("TargetEnemy", (GameObject)null);
        values.Add("FamilyRusher", (GameObject)null);
        values.Add("AcceptableDistanceBetweenRusherAndHelipad", 30f);
        values.Add("PatrolRange", 10f);
        values.Add("ShootPrecision", 6f);
        values.Add("DodgeDistance", 5f);
        values.Add("ShellAvoidanceTolerance", .5f);
        values.Add("RangeDetectShells", 15f);
        values.Add("CurrentAvoidedShell", null);
        
        List<Vector3> currentPath = new List<Vector3>();
        values.Add("CurrentPath", currentPath);
        
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