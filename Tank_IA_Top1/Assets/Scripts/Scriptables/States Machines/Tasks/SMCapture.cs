using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "AI/State Machine/Tasks/Capture")]
public class SMCapture : SMTask
{
    public float captureSpeed = 1;
    private Slider captureSlider;
    private Image captureFillImage;
    private PointZoneManager pointZoneManager;

    public override void Initialize(StateMachineController controller)
    {
        base.Initialize(controller);
        pointZoneManager = controller.GetComponent<PointZoneManager>();
        if (pointZoneManager == null) return;
        captureSlider = pointZoneManager.captureSlider;
        captureFillImage = pointZoneManager.captureSliderImage;
    }
    
    public override void RunTask()
    {
        if (pointZoneManager == null) return;

        if (pointZoneManager.newTeam != null)
        {
            pointZoneManager.currentTeam = pointZoneManager.newTeam;
            
            captureFillImage.color = pointZoneManager.currentTeam.teamColor;
            captureSlider.value += Time.deltaTime * captureSpeed;
            captureSlider.value = Mathf.Clamp(captureSlider.value, captureSlider.minValue, captureSlider.maxValue);
            pointZoneManager.captureLevel = captureSlider.value;
        }
        else Debug.Log("Conflict on point !");

    }

    
}