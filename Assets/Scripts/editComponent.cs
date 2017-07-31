using HoloToolkit.Unity.InputModule;
using UnityEngine;

public class editComponent : MonoBehaviour, IManipulationHandler
{
    [Tooltip("Speed at which the object is resized.")]
    [SerializeField]
    float ResizeSpeedFactor = 1.0f;

    [SerializeField]
    float ResizeScaleFactor = 0.5f;

    [Tooltip("Minimum resize scale allowed.")]
    float MinScale = 0.01f;

    [Tooltip("Maximum resize scale allowed.")]
    float MaxScale = 0.99f;

    [SerializeField]
    bool resizingEnabled = true;

    Vector3 lastScale;

    public void SetResizing(bool enabled)
    {
        resizingEnabled = enabled;
    }

    public void OnManipulationStarted(ManipulationEventData eventData)
    {
        InputManager.Instance.PushModalInputHandler(gameObject);
        lastScale = transform.localScale;
    }

    public void OnManipulationUpdated(ManipulationEventData eventData)
    {
        if (resizingEnabled)
        {

            Resize(eventData.CumulativeDelta);
        }
    }

    public void OnManipulationCompleted(ManipulationEventData eventData)
    {
        InputManager.Instance.PopModalInputHandler();
    }

    public void OnManipulationCanceled(ManipulationEventData eventData)
    {
        InputManager.Instance.PopModalInputHandler();
    }

    void ResizeHeight(Vector3 movement)
    {
        Debug.Log(movement);
    }


    void Resize(Vector3 newScale)
    {
        float resizeY;
        resizeY = newScale.y * ResizeScaleFactor;

        resizeY = Mathf.Clamp(lastScale.y + resizeY, MinScale, MaxScale);

        var initialPos = transform.localPosition;

        transform.localScale = new Vector3(lastScale.x, resizeY , lastScale.z);

        var yPos = -0.5f + (resizeY / 2);
        Vector3 cubePos = new Vector3(initialPos.x, yPos, initialPos.z);
        transform.localPosition = cubePos;

    }
}
