using System;
using UnityEngine;

public class PickupSequenceManager : MonoBehaviour
{
    [Serializable]
    public class Step
    {
        public GameObject pickupObject;

        public PlacementArea placementArea;
    }

    public Step[] steps;

    [Header("Optional")]
    public bool resetOnStart = true;

    private int _currentIndex = 0;

    private void Start()
    {
        // Bind each placement area to this manager + its index.
        for (int i = 0; i < steps.Length; i++)
        {
            if (steps[i].placementArea != null)
            {
                steps[i].placementArea.Bind(this, i);
            }
        }

        if (resetOnStart)
        {
            ResetSequence();
        }
        else
        {
            ApplyActiveState();
        }
    }

    public int CurrentIndex => _currentIndex;

    public void ResetSequence()
    {
        _currentIndex = 0;
        ApplyActiveState();
    }

    // Called by PlacementArea when a correct placement is detected.
    public void CompleteStep(int stepIndex)
    {
        if (stepIndex != _currentIndex) return;

        // Disable the placed object and its placement area.
        Step s = steps[_currentIndex];

        if (s.pickupObject != null)
            s.pickupObject.SetActive(false);

        if (s.placementArea != null)
            s.placementArea.gameObject.SetActive(false);

        // Advance
        _currentIndex++;

        ApplyActiveState();
    }

    private void ApplyActiveState()
    {
        for (int i = 0; i < steps.Length; i++)
        {
            bool isCurrent = (i == _currentIndex);

            if (steps[i].pickupObject != null)
                steps[i].pickupObject.SetActive(isCurrent);

            if (steps[i].placementArea != null)
                steps[i].placementArea.gameObject.SetActive(isCurrent);
        }

        // If finished, everything stays off.
        if (_currentIndex >= steps.Length)
        {
            // Sequence complete. Nothing else needed for Devlog 2.
        }
    }
}
