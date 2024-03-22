using System;
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
    private bool viewing;
    private Transform originalParent;

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

        originalParent = gameObject.transform.parent;

        gameObject.SetActive(true);

    }

    protected override Color GetColor()
    {
        return animationColors[(int)GetState()];
    }

    public override string GetMaterial()
    {
        return animationMaterials[(int)GetState()];
    }

    public IEnumerator AnimatePart() {
        while (true) {
            if (!viewing) {
                float distance = Vector3.Distance(targetPosition, GetGameObject().transform.position);

                if (distance < 0.001f) {
                    GetGameObject().transform.position = originalPosition;
                    GetGameObject().transform.rotation = originalRotation;
                } else {
                    GetGameObject().transform.position = Vector3.Lerp(GetGameObject().transform.position, targetPosition, Time.deltaTime * 3);
                    GetGameObject().transform.rotation = Quaternion.Lerp(GetGameObject().transform.rotation, targetRotation, Time.deltaTime * 3);
                }
            } else {
                //GetGameObject().transform.rotation = Quaternion.RotateTowards(GetGameObject().transform.rotation, Quaternion.LookRotation(Vector3.right), Time.deltaTime * 10);
                GetGameObject().transform.RotateAround(GetGameObject().transform.position, Vector3.up, Time.deltaTime * 20);
            }

            yield return new WaitForFixedUpdate();
        }
    }

    public void ViewPart() {
        if (!viewing) {
            viewing = true;
            GetGameObject().transform.SetParent(GameObject.Find("Canvas").transform);
            GetGameObject().transform.localPosition = new Vector3(0,0,CalculateDistance());
            GetGameObject().transform.rotation = Quaternion.Euler(30f, 0, 30f);
            //float scale = CalculateDistance();
            //GetGameObject().transform.localScale = new Vector3(scale,scale,scale);
        } else {
            viewing = false;
            GetGameObject().transform.SetParent(originalParent);
            GetGameObject().transform.position = originalPosition;
            GetGameObject().transform.rotation = originalRotation;
            //GetGameObject().transform.localScale = new Vector3(1,1,1);
        }
    }

    public float CalculateDistance() {
        Mesh mesh;
        if (GetGameObject().GetComponent<MeshFilter>() != null) {
            mesh = GetGameObject().GetComponent<MeshFilter>().mesh;
        } else {
            mesh = GetGameObject().GetComponentInChildren<MeshFilter>().mesh;
        }

        float size = mesh.bounds.size.y;
        float distance = -(1.0f / size) * 14; // You can adjust the scale factor to control the effect


        Debug.Log(distance);

        return distance;
    }

    public void DestroyAnimation() {
        //GameObject.Destroy(GetGameObject());
        GetGameObject().SetActive(false);
    }

}