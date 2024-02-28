using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Visometry.DesignSystem;
using Visometry.VisionLib.SDK.Core;

public class Manager : MonoBehaviour
{   
    public bool useTracking;
    public NotificationDisplay notificationDisplay;
    public TMP_Text currentPartDisplay;
    public TMP_Text trackingDisplay;
    public TMP_Text screwDisplay;
    public GameObject arrow;
    public ModelTracker modelTracker;
    public GameObject animationPrefab;
    public GameObject trackingPrefab;
    private Instructions instructions;

    private bool started;

    // Start is called before the first frame update
    void Awake()
    {
        DeactivateTrackingParts();
        DeactivateAnimationParts();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateArrow();
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
        notificationDisplay.AddNotification(new NotificationObject("Tracking", "Switched to Tracking Mode", Notification.Kind.Info, Notification.Type.Inline));
    }

    public void SetUseAnimation() {
        useTracking = false;
        notificationDisplay.AddNotification(new NotificationObject("Animation", "Switched to Animation Mode", Notification.Kind.Info, Notification.Type.Inline));
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

        notificationDisplay.AddNotification(new NotificationObject("Started", "Started Intructions", Notification.Kind.Info, Notification.Type.Inline));

    }

    public void NextStep() {
        instructions.NextStep();
    }

    public void ViewPart() {
        instructions.ViewPart();
    }

    private void UpdateArrow() {

        Image arrowImg = arrow.GetComponent<Image>();

        if (instructions != null) {
            if (instructions.furniture.GetCurrentStep() != null) {
                if ((!instructions.furniture.GetCurrentStep().UsesTracking()) && (instructions.furniture.GetCurrentStep().GetPart().GetGameObject() != null)) {
                    GameObject targetObject = instructions.furniture.GetCurrentStep().GetAnimationPart().GetGameObject();
                    Vector3 centre = arrow.transform.parent.position;

                    Vector3 v3Pos = Camera.main.WorldToViewportPoint(targetObject.transform.position);
                
                    if (v3Pos.x >= 0.0f && v3Pos.x <= 1.0f && v3Pos.y >= 0.0f && v3Pos.y <= 1.0f) {
                        arrowImg.color = new Color(1,1,1,0);
                    } else {
                        arrowImg.color = new Color(1,1,1,1);
                    }

                    // Calculate the direction from the arrow to the target
                    Vector3 direction = targetObject.transform.position - centre;

                    v3Pos.x -= 0.5f;  // Translate to use center of viewport
                    v3Pos.y -= 0.5f; 
                    v3Pos.z = 0;      // I think I can do this rather than do a 
                                    //   a full projection onto the plane

                    var targetPosLocal = Camera.main.transform.InverseTransformPoint(targetObject.transform.position);

                    // Calculate the angle in degrees
                    float angle = -Mathf.Atan2(targetPosLocal.x, targetPosLocal.y) * Mathf.Rad2Deg;

                    // Rotate the arrow to point towards the target
                    arrow.transform.localEulerAngles = new Vector3(0,0, angle);

                }
            } else {
                arrowImg.color = new Color(1,1,1,0);
            }
        }

    }

}
