using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instructions : MonoBehaviour
{
    [HideInInspector] public GameObject FurnitureObject;
    [SerializeField] public Vector3 asidePosition;
    [SerializeField] public List<Component> components = new List<Component>();
    [SerializeField] public List<Part> parts = new List<Part>();
    [HideInInspector] public Furniture furniture;

    void Awake() {
        furniture = new Furniture(FurnitureObject);

        furniture.CombineParts(components, parts);

        Dictionary<Component, int> componentQueue = new Dictionary<Component, int>();
        foreach (Part p in furniture.GetParts()) {

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
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var currentPart = furniture.GetCurrentStep().GetPart();
        var currentTrackingPart = furniture.GetCurrentStep().GetTrackingPart();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (currentPart.GetState() == States.WAITING) {
                currentPart.SetState(States.CORRECT);
                currentTrackingPart.SetState(States.CORRECT);
            } else {
                furniture.NextStep();
            }
        }
    }
}
