using System;
using UnityEngine;

public class PickupSequenceManager : MonoBehaviour
{

    public Material placementActiveMaterial;
    public Material placementInactiveMaterial;

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

    public void CompleteStep(int stepIndex)
    {
        if (stepIndex != _currentIndex) return;

        Step s = steps[_currentIndex];

        if (s.pickupObject != null)
            s.pickupObject.SetActive(false);

        if (s.placementArea != null)
            s.placementArea.gameObject.SetActive(false);

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
        {
            GameObject areaObject = steps[i].placementArea.gameObject;
            areaObject.SetActive(isCurrent);

            Renderer r = areaObject.GetComponent<Renderer>();
            if (r != null)
            {
                r.material = isCurrent
                    ? placementActiveMaterial
                    : placementInactiveMaterial;
            }
        }
    }
}
}
