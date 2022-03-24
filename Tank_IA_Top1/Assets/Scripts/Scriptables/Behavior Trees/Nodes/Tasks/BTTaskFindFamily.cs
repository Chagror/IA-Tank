using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Complete;

[CreateAssetMenu(menuName = "AI/Behavior Tree/Task/Find Family")]
public class BTTaskFindFamily : BTTask
{
    public override bool Execute()
    {
        GameObject helipad = blackboard.GetValue<GameObject>("Helipad");
        TankMovement tankMovement = controller.GetComponent<TankMovement>();

        GameObject family = null;
        foreach (var tank in tankMovement.tankTeam.Tanks)
        {
            if (tank.behaviorTree.name == "BT_RushZone" || tank.behaviorTree.name == "BT_RushZone 1") family = tank.m_Instance;
        }

        if(family != null)
            if(Vector3.Distance(family.transform.position, helipad.transform.position) < blackboard.GetValue<float>("AcceptableDistanceBetweenRusherAndHelipad"))
                blackboard.SetValue("FamilyRusher", family);
            else blackboard.SetValue<GameObject>("FamilyRusher", null);

        success = true;

        return base.Execute();
    }
}