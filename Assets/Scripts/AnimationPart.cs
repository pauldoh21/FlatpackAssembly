using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Visometry.VisionLib.SDK.Core;

public class AnimationPart : Part {

    //private TrackingMesh model;
    private Vector3 targetPosition;
    private Quaternion targetRotation;

    private Vector3 originalPosition;
    private Quaternion originalRotation;

    private static List<Color> animationColors = new List<Color>
    {
        new Color(0, 0, 0, 0),                      //blank
        new Color(0.8f, 0.22f, 0.13f, 1),           //red
        new Color(0.22f, 0.95f, 0.25f, 1),          //green
        new Color(0.22f, 0.95f, 0.25f, 0)           //green (transparent)
    };

    private static List<string> animationMaterials = new List<string>
    {
        @"Materials/disabled",
        @"Materials/tracking_waiting",      // Custom material for TrackingPart WAITING state
        @"Materials/correct",               // Custom material for TrackingPart CORRECT state
        @"Materials/inactive"               // Custom material for TrackingPart FINISHED state
    };

    public AnimationPart(GameObject gameObject, Transform targetTransform) : base(gameObject)
    {
        targetPosition = targetTransform.position;
        targetRotation = targetTransform.rotation;

        originalPosition = gameObject.transform.position;
        originalRotation = gameObject.transform.rotation;

        gameObject.SetActive(true);

    }

    protected override Color GetColor()
    {
        return animationColors[(int)GetState()];
    }

    protected override string GetMaterial()
    {
        return animationMaterials[(int)GetState()];
    }

    public IEnumerator AnimatePart() {
        while (true) {
            float distance = Vector3.Distance(targetPosition, GetGameObject().transform.position);

            if (distance < 0.001f) {
                GetGameObject().transform.position = originalPosition;
                GetGameObject().transform.rotation = originalRotation;
            } else {
                GetGameObject().transform.position = Vector3.Lerp(GetGameObject().transform.position, targetPosition, Time.deltaTime * 3);
                GetGameObject().transform.rotation = Quaternion.Lerp(GetGameObject().transform.rotation, targetRotation, Time.deltaTime * 3);
            }

            yield return new WaitForFixedUpdate();
        }
    }

    public void DestroyAnimation() {
        //GameObject.Destroy(GetGameObject());
        GetGameObject().SetActive(false);
    }

}