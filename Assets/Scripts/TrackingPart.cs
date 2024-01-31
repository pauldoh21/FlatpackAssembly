using System.Collections.Generic;
using UnityEngine;

public class TrackingPart : Part {

    private static List<Color> trackingColors = new List<Color>
    {
        new Color(0, 0, 0, 0),                      //blank
        new Color(0.8f, 0.22f, 0.13f, 1),           //red
        new Color(0.22f, 0.95f, 0.25f, 1),          //green
        new Color(0.22f, 0.95f, 0.25f, 0)           //green (transparent)
    };

    private static List<string> trackingMaterials = new List<string>
    {
        @"Materials/disabled",
        @"Materials/tracking_waiting",      // Custom material for TrackingPart WAITING state
        @"Materials/correct",               // Custom material for TrackingPart CORRECT state
        @"Materials/inactive"               // Custom material for TrackingPart FINISHED state
    };

    public TrackingPart(GameObject gameObject) : base(gameObject)
    {
        // Additional Initialization for TrackingPart if needed
        //gameObject.transform.localScale = new Vector3(1,1,1);

        // Find way to automatically set up tracking mesh
        GameObject trackingAnchor = GameObject.Find("VLTrackingAnchor");
        if (trackingAnchor != null) {
            gameObject.transform.parent = trackingAnchor.transform;
        }

        
    }

    protected override Color GetColor()
    {
        return trackingColors[(int)GetState()];
    }

    protected override string GetMaterial()
    {
        return trackingMaterials[(int)GetState()];
    }

    public void DestroyTracking() {
        GameObject.Destroy(GetGameObject());
    }

}