using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Visometry.VisionLib.SDK.Core;

public class Instructions : MonoBehaviour
{
    [HideInInspector] public GameObject FurnitureObject;

    [SerializeField] public TMP_Text currentPartDisplay; // REMOVE PROBS
    [SerializeField] public TMP_Text trackingDisplay; // REMOVE PROBS
    [SerializeField] public ModelTracker modelTracker;
    [SerializeField] public bool partsFirst;
    [SerializeField] public bool showOutlines;
    [SerializeField] private GameObject inputObject;
    [HideInInspector] public Vector3 asidePosition;
    [SerializeField] public List<Component> components = new List<Component>();
    [SerializeField] public List<Part> parts = new List<Part>();
    [HideInInspector] public Furniture furniture;

    void Awake() {

        DeactivateTrackingParts();

        TrackingAnchor trackingAnchor = GameObject.Find("VLTrackingAnchor").GetComponent<TrackingAnchor>();
        trackingAnchor.OnTracked.AddListener(UpdateTrackingTextTrue);
        trackingAnchor.OnTrackingLost.AddListener(UpdateTrackingTextFalse);

        furniture = new Furniture(FurnitureObject);

        furniture.CombineParts(components, parts, partsFirst);

        Dictionary<Component, int> componentQueue = new Dictionary<Component, int>();
        foreach (Part p in furniture.GetParts()) {
            
            p.showOutline = showOutlines;

            if (p is Component) {
                // Adding the steps of the component
                Component c = (Component)p;
                foreach (Part sp in c.GetParts()) {
                    Step newStep = c.AddStep(sp);
                    if (c.putAside) {
                        //Debug.Log(sp.GetGameObject());
                        newStep.SetNewParentPosition(asidePosition);
                    }
                }

                furniture.AddStep(p);

                /* // This step here should be determined by whether put aside is selected or not
                // If it is selected then it should be inserted later into the list
                if (c.putAside) {
                    componentQueue.Add(c, c.whenToAdd);
                } else {
                    furniture.AddStep(p);
                } */
            } else {
                furniture.AddStep(p);
            }

            /* List<Component> tempKeys = new List<Component>(componentQueue.Keys);
            foreach (Component c in tempKeys) {
                if (componentQueue[c] == 0) {
                    //Debug.Log("Component step added");
                    furniture.AddStep(c);
                    componentQueue.Remove(c);
                } else {
                    componentQueue[c] -= 1;
                    //Debug.Log(componentQueue[c]);
                }
            } */
        }
        furniture.AdjustSteps();
        furniture.GetCurrentStep().ActivateStep();
        Debug.Log(furniture.DisplaySteps());
        modelTracker.ResetTrackingHard();
    }

    void DeactivateTrackingParts() {
        foreach (Transform t in GameObject.Find("VLTrackingAnchor").transform) {
            t.gameObject.SetActive(false);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void UpdateTrackingTextTrue() {
        trackingDisplay.text = "Tracking";
    }

    private void UpdateTrackingTextFalse() {
        trackingDisplay.text = "Not Tracking";
    }

    // Update is called once per frame
    void Update()
    {
        currentPartDisplay.text = furniture.GetCurrentStep().GetPart().GetGameObject().name;

        furniture.GetCurrentStep().CheckOverlap();
        if (furniture.GetCurrentStep().GetPart().GetState() == States.CORRECT) {
            StartCoroutine(CheckNextStep());
        }
        if (furniture.GetCurrentStep().GetPart().GetState() == States.CORRECT)
        {
            inputObject.SetActive(true);
        } else {
            inputObject.SetActive(false);
        }
    }

    public void NextStep() {
        if (furniture.GetCurrentStep().GetPart().GetState() == States.CORRECT)
        {
            furniture.NextStep();
            modelTracker.ResetTrackingHard();
        }
    }

    IEnumerator CheckNextStep() {
        bool breakOut = false;
        for (int i = 0; i <= 3; i++) {
            if (furniture.GetCurrentStep().GetPart().GetState() != States.CORRECT) {
                breakOut = true;
                break;
            }
            yield return new WaitForSeconds(1);
        }
        if (!breakOut) {
            NextStep();
        }
    }
}

