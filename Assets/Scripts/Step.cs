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

    public void Init(Part part, int num, Vector3 parentPosition, Vector3 parentRotation, bool useTracking) {
        this.part = part;
        //Debug.Log(num);
        this.stepNo = num;
        this.useTracking = useTracking;
        this.parentPosition = parentPosition;
        originalParentPosition = this.part.GetGameObject().transform.parent.position;
        this.parentRotation = parentRotation;
        originalParentRotation = this.part.GetGameObject().transform.parent.rotation.eulerAngles;
        asidePosition = originalParentPosition;
    }

    //Overrides Unity ScriptableObject CreateInstance to take parameters and act as a constructor
    public static Step CreateInstance(Part part, int num, Vector3 parentPosition, Vector3 parentRotation, bool useTracking) {
        Step step = ScriptableObject.CreateInstance<Step>();
        step.Init(part, num, parentPosition, parentRotation, useTracking);
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
        if (useTracking) {
            trackingPart.DestroyTracking();
        } else {
            animationPart.DestroyTracking();
        }
    }

    //CHECK COMPLETED: check for similar position and rotation
    public void CheckOverlap() {
        float distanceThreshold = 0.01f;
        float distance = Vector3.Distance(part.GetGameObject().transform.position, GetSecondPart().GetGameObject().transform.position);

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

    Part GetSecondPart() {
        if (useTracking) {
            return trackingPart;
        } else {
            return animationPart;
        }
    }
    
}
