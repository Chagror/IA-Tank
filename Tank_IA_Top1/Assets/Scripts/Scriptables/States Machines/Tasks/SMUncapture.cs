using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "AI/State Machine/Tasks/Uncapture")]
public class SMUncapture : SMTask
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
        
        captureSlider.value -= Time.deltaTime * captureSpeed;
        captureSlider.value = Mathf.Clamp(captureSlider.value, captureSlider.minValue, captureSlider.maxValue);
        pointZoneManager.captureLevel = captureSlider.value;
    }
}