using UnityEngine;


[RequireComponent(typeof(Collider))]
public class PlacementArea : MonoBehaviour
{
    private PickupSequenceManager _manager;
    private int _stepIndex;

    private void Reset()
    {
        // Ensure the collider is a trigger (required for placement detection).
        Collider c = GetComponent<Collider>();
        c.isTrigger = true;
    }

    public void Bind(PickupSequenceManager manager, int stepIndex)
    {
        _manager = manager;
        _stepIndex = stepIndex;

        // Enforce trigger collider
        Collider c = GetComponent<Collider>();
        c.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_manager == null) return;
        if (_manager.CurrentIndex != _stepIndex) return;

        var steps = _manager.steps;
        if (_stepIndex < 0 || _stepIndex >= steps.Length) return;

        GameObject expectedObj = steps[_stepIndex].pickupObject;
        if (expectedObj == null) return;

        // Must be the expected object (or a child collider of it).
        if (other.gameObject != expectedObj && !other.transform.IsChildOf(expectedObj.transform))
            return;

        // Only count placement if it is NOT currently held.
        UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grab = expectedObj.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
        if (grab != null && grab.isSelected)
            return;

        _manager.CompleteStep(_stepIndex);
    }
}
