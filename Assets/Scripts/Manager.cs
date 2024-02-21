using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Visometry.VisionLib.SDK.Core;

public class Manager : MonoBehaviour
{   
    public bool useTracking;
    public TMP_Text currentPartDisplay;
    public TMP_Text trackingDisplay;
    public TMP_Text screwDisplay;
    public ModelTracker modelTracker;
    public GameObject animationPrefab;
    public GameObject trackingPrefab;
    private Instructions instructions;

    // Start is called before the first frame update
    void Awake()
    {
        DeactivateTrackingParts();
        DeactivateAnimationParts();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void DeactivateTrackingParts() {
        foreach (Transform t in GameObject.Find("VLTrackingAnchor").transform) {
            if (t.gameObject.name != "default")
            t.gameObject.SetActive(false);
        }
    }

    void DeactivateAnimationParts() {
        foreach (Transform t in GameObject.Find("AnimationParts").transform) {
            t.gameObject.SetActive(false);
        }
    }

    public void SetUseTracking() {
        useTracking = true;
    }

    public void SetUseAnimation() {
        useTracking = false;
    }

    public void StartInstructions() {
        GameObject gameObject;

        if (!useTracking) {
            gameObject = Instantiate(animationPrefab);
        } else {
            gameObject = Instantiate(trackingPrefab);
        }
        gameObject.name = "bedsideTable";
        instructions = gameObject.GetComponent<Instructions>();
        instructions.currentPartDisplay = currentPartDisplay;
        instructions.trackingDisplay = trackingDisplay;
        instructions.screwDisplay = screwDisplay;
        instructions.modelTracker = modelTracker;

    }

    public void NextStep() {
        instructions.NextStep();
    }

    public void ViewPart() {
        instructions.ViewPart();
    }
}
