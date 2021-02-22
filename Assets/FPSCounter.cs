using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class FPSCounter : MonoBehaviour
{
    const float fpsMeasurePeriod = 1.0f;

    int totalFrames;
    float nextFlushTime;
    float realTimeSinceStart;

    [SerializeField] Text text;

    private void Start()
    {
        Reset();
    }

    public void Reset()
    {
        realTimeSinceStart = 0;
        nextFlushTime = realTimeSinceStart + fpsMeasurePeriod;
    }

    private void Update()
    {
        realTimeSinceStart += Time.unscaledDeltaTime;
        totalFrames++;
        if (realTimeSinceStart >= nextFlushTime)
        {
            float delta = nextFlushTime - realTimeSinceStart;
            float fps = totalFrames / (fpsMeasurePeriod + delta);
            totalFrames = 0;
            nextFlushTime = realTimeSinceStart + fpsMeasurePeriod + delta; ;
            text.text = fps.ToString("F1") + "fps";
        }
    }
}