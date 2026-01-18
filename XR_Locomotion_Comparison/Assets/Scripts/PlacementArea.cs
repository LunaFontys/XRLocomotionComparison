using UnityEngine;


[RequireComponent(typeof(Collider))]
public class PlacementArea : MonoBehaviour
{
    private PickupSequenceManager _manager;
    private int _stepIndex;

    private void Reset()
    {
        Collider c = GetComponent<Collider>();
        c.isTrigger = true;
    }

    public void Bind(PickupSequenceManager manager, int stepIndex)
    {
        _manager = manager;
        _stepIndex = stepIndex;

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

        if (other.gameObject != expectedObj && !other.transform.IsChildOf(expectedObj.transform))
            return;

        UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grab = expectedObj.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
        if (grab != null && grab.isSelected)
            return;

        _manager.CompleteStep(_stepIndex);
    }
}
