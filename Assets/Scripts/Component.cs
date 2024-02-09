using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class Component : Part
{
    
    [SerializeField] List<Part> parts;
    [HideInInspector] public bool putAside;
    [HideInInspector] public int whenToAdd;
    Step currentStep;
    List<Step> steps;

    [HideInInspector] public bool built;

    public Component(GameObject gameObject) : base(gameObject) {
        parts = new List<Part>();
        steps = new List<Step>();
    } 

    public Step AddStep(Part part, bool useTracking) {

        if (steps == null) {
            steps = new List<Step>();
        }

        part.SetState(States.DISABLED);
        Step newStep = Step.CreateInstance(part, steps.Count, part.parentPosition, part.parentRotation, useTracking);
        steps.Add(newStep);

        if (steps.Count > 1) {
            // Points the previous step in the list to the first step of the component
            if (part is Component) {
                Component c = (Component)part;
                GetPreviousStep().SetNextStep(c.GetSteps()[0]);
                if (!c.putAside) {
                    c.GetSteps()[^1].SetNextStep(newStep);
                }
            } else {
                GetPreviousStep().SetNextStep(newStep);
            }
        }

        currentStep = steps[0];
        return newStep;
    }

    public void NextStep() {
        if (currentStep.UsesTracking()) {
            if (currentStep.GetTrackingPart().GetGameObject() != null) {
                currentStep.EndStep();
            }
        } else {
            if (currentStep.GetAnimationPart().GetGameObject() != null) {
                currentStep.EndStep();
            }
        }

        // If next step is component do steps of component first

        Step nextStep = currentStep.GetNextStep();

        if (nextStep == null) {
            Debug.Log("Done");
        } else {
            currentStep = nextStep;
            currentStep.ActivateStep();
        }
    }

    public List<Part> GetParts() {
        return parts;
    }

    public void SetParts(List<Part> parts) {
        this.parts = parts;
    }

    public Step GetCurrentStep() {
        return currentStep;
    }

    public List<Step> GetSteps() {
        return steps;
    }

    // Insert put aside components after their when to add value in the list
    // 
    public void AdjustSteps() {
        foreach (Step s in GetSteps()) {
            if (s.GetPart() is Component) {
                Component c = (Component)s.GetPart();
                if (c.putAside) {

                    for (int i=0; i < GetSteps().Count; i++) {
                        if (i == c.whenToAdd) {
                            Debug.Log("Inserting " + c.GetGameObject() + " after step " + i);
                            //s.SetNextStep(GetSteps()[i].GetNextStep());
                            GetSteps()[i].SetNextStep(s);
                            break;
                        }
                    }
                }
            }
        }
    }

    private Step GetPreviousStep() {
        if (GetSteps()[^2].GetPart() is Component) {
            Component c = (Component)GetSteps()[^2].GetPart();
            if (c.putAside){
                return c.GetSteps()[^1];
            } else {
                return GetSteps()[^2];
            }
        } else {
            return GetSteps()[^2];
        }
    }

    public string DisplaySteps() {
        string output = "";
        bool done = false;
        Step step = GetSteps()[0];

        while (!done) {
            output += step.GetPart().GetGameObject().name + " - " + step.GetStepNo() + ", ";
            if (step.GetNextStep() != null) {
                step = step.GetNextStep();
            } else {
                done = true;
            }
        }
        return output;
    }

}
