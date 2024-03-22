using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Visometry.VisionLib.SDK.Core;

[System.Serializable]
public class Step : ScriptableObject
{

    int stepNo;
    Part part;
    TrackingPart trackingPart;
    AnimationPart animationPart;
    Step nextStep;

    bool useTracking;

    Vector3 parentPosition;
    Vector3 originalParentPosition;

    Vector3 parentRotation;
    Vector3 originalParentRotation;

    Vector3 asidePosition;
    bool done;

    public void Init(Part part, int num, Vector3 parentPosition, Vector3 parentRotation) {
        this.part = part;
        //Debug.Log(num);
        this.stepNo = num;
        useTracking = part.UsesTracking();
        this.parentPosition = parentPosition;
        originalParentPosition = this.part.GetGameObject().transform.parent.position;
        this.parentRotation = parentRotation;
        originalParentRotation = this.part.GetGameObject().transform.parent.rotation.eulerAngles;
        asidePosition = originalParentPosition;
    }

    //Overrides Unity ScriptableObject CreateInstance to take parameters and act as a constructor
    public static Step CreateInstance(Part part, int num, Vector3 parentPosition, Vector3 parentRotation) {
        Step step = ScriptableObject.CreateInstance<Step>();
        step.Init(part, num, parentPosition, parentRotation);
        return step;
    }

    public int GetStepNo(){
        return stepNo;
    }

    public Part GetPart() {
        return part;
    }

    public TrackingPart GetTrackingPart() {
        return trackingPart;
    }

    public AnimationPart GetAnimationPart() {
        return animationPart;
    }

    public void SetNewParentPosition(Vector3 newParentPosition) {
        this.asidePosition = newParentPosition;
    }

    public Step GetNextStep() {
        return nextStep;
    }

    public void SetNextStep(Step nextStep) {
        this.nextStep = nextStep;
    }

    public bool UsesTracking() {
        return useTracking;
    }

    public bool IsComplete() {
        return done;
    }

    public void ActivateStep() {
        //name object in game "trackingObject"
        //give outline material
        //show and outline part on furniture
        GameObject partObject = part.GetGameObject();
        partObject.SetActive(true);
        // Set the position and rotation of the parent object while the step is active
        partObject.transform.parent.position = parentPosition;
        partObject.transform.parent.rotation = Quaternion.Euler(parentRotation);
        part.SetState(States.WAITING);

        if (useTracking) {

            GameObject trackingAnchor = GameObject.Find("VLTrackingAnchor");
            if (trackingAnchor != null && partObject.name != "Initial") {
                trackingPart = new TrackingPart(trackingAnchor.transform.Find(partObject.name).gameObject);
            } else {
                trackingPart = new TrackingPart(Instantiate(partObject));
            }

        } else {
            GameObject animationParts = GameObject.Find("AnimationParts");
            if (animationParts != null && partObject.name != "Initial") {
                Debug.Log(partObject.name);
                animationPart = new AnimationPart(animationParts.transform.Find(partObject.name).gameObject, partObject.transform);
            } else {
                animationPart = new AnimationPart(Instantiate(partObject), partObject.transform);
            }
        }

        GetSecondPart().SetState(States.WAITING);
    }

    public void EndStep() {
        if (part is Component) {
            Component c = (Component)part;
            part.GetGameObject().transform.parent.position = originalParentPosition;
            part.GetGameObject().transform.parent.rotation = Quaternion.Euler(originalParentRotation);
            if (c.putAside) {
                Debug.Log(c);
                Debug.Log(c.GetGameObject());
                Debug.Log(c.GetSteps());
                Debug.Log(c.GetSteps()[^1].originalParentPosition);
                c.GetGameObject().transform.position = c.GetSteps()[^1].originalParentPosition;
            }
        } else {
            //part.GetGameObject().transform.parent.position = asidePosition;
            part.GetGameObject().transform.parent.position = originalParentPosition;
            part.GetGameObject().transform.parent.rotation = Quaternion.Euler(originalParentRotation);
        }
        
        part.SetState(States.FINISHED);
        GetSecondPart().SetState(States.FINISHED);

        done = true;

        if (useTracking) {
            trackingPart.DestroyTracking();
        } else {
            animationPart.DestroyAnimation();
        }
    }

    //CHECK COMPLETED: check for similar position and rotation
    public void CheckOverlap() {
        float distanceThreshold = 0.07f;
        float distance;
        if (useTracking) {
            Vector3 pos = GetSecondPart().GetGameObject().transform.position;
            distance = Vector3.Distance(part.GetGameObject().transform.position, new Vector3(pos.x, pos.y + 1.59f, pos.z));
        } else {
            distance = Vector3.Distance(part.GetGameObject().transform.position, GetSecondPart().GetGameObject().transform.position);
        }

        float angleThreshold = 5f;
        float angle = Quaternion.Angle(part.GetGameObject().transform.rotation, GetSecondPart().GetGameObject().transform.rotation);
        bool close = (distance <= distanceThreshold) && (angle <= angleThreshold);


        if (close && (part.GetState() != States.CORRECT)) {
            part.SetState(States.CORRECT);
            GetSecondPart().SetState(States.CORRECT);
        } else if (!close && (part.GetState() != States.WAITING)) {
            part.SetState(States.WAITING);
            GetSecondPart().SetState(States.WAITING);
        }
    }

    public Part GetSecondPart() {
        if (useTracking) {
            return trackingPart;
        } else {
            return animationPart;
        }
    }
    
}