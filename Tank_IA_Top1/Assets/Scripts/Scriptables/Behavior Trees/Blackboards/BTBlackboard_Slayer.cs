using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Behavior Tree/Blackboard/Slayer Blackboard")]
public class BTBlackboard_Slayer : BTBlackboard
{
    public override void Init(BTController controller)
    {
        values.Clear();
        
        values.Add("TargetEnemy", null);
        values.Add("RangeDetectEnemy", Mathf.Infinity);
        values.Add("RangeDetectShells", 15f);
        values.Add("ShellAvoidanceTolerance", .5f);
        values.Add("DodgeDistance", 0f);
        values.Add("CurrentAvoidedShell", null);
        values.Add("ShootDistance", 10f);
        values.Add("IsAtRangeToShoot", false);
        values.Add("ShootPrecision", 15f);

        List<Vector3> currentPath = new List<Vector3>();
        values.Add("CurrentPath", currentPath);
        
        
        Complete.TankMovement currentTank = controller.GetComponent<Complete.TankMovement>();
        List<GameObject> enemies = new List<GameObject>();
        List<GameObject> allies = new List<GameObject>();
        if (Complete.GameManager.instance.team1 == currentTank.tankTeam)
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
