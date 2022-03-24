using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Complete;

[CreateAssetMenu(menuName = "AI/Behavior Tree/Task/Set Target")]
public class BTTaskSetTarget : BTTask
{
    public override bool Execute()
    {
        GameObject closestEnemy = null;
        foreach (var enemy in blackboard.GetValue<List<GameObject>>("Enemies"))
        {
            if (!closestEnemy && enemy.activeInHierarchy) closestEnemy = enemy;
            else if(enemy.activeInHierarchy)
            {
                if(Vector3.Distance(controller.transform.position, enemy.transform.position) < Vector3.Distance(controller.transform.position, closestEnemy.transform.position) && enemy.activeInHierarchy)
                {
                    closestEnemy = enemy;
                }
            }
        }
        if(closestEnemy == null)
        {
            Debug.LogError("Closest enemy is null");
            return false;
        }
        if(Vector3.Distance(controller.transform.position, closestEnemy.transform.position) < blackboard.GetValue<float>("RangeDetectEnemy") && closestEnemy.activeInHierarchy)
        {
            blackboard.SetValue<GameObject>("TargetEnemy", closestEnemy);
        }
        else if(closestEnemy.activeInHierarchy)
        {
            blackboard.SetValue<GameObject>("TargetEnemy", (GameObject)null);
        }

        success = true;

        return base.Execute();
    }
}
