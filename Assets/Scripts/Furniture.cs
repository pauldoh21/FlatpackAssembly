using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class Furniture : Component
{

    public Furniture(GameObject gameObject) : base(gameObject) {
        //components = new List<Component>();
    }

    private void InitComponents() {
        GetParts().Add(new Component(new GameObject()));
        //foreach (Component c in components) {
        //    AddStep(c);
        //}
    }

    public void CombineParts(List<Component> components, List<Part> parts, bool partsFirst) {
        List<Part> newParts = new List<Part>();

        if (partsFirst) {
            foreach (Part p in parts) {
                newParts.Add(p);
            }
        } 

        foreach (Component c in components) {
            newParts.Add(c);
        }

        if (!partsFirst) {

            // Adds an initial filler object to the assembly to assure that the first part is not a component
            GameObject initialObject = GameObject.Find("Initial");
            initialObject.transform.SetParent(GetGameObject().transform);
            Part initialPart = new Part(initialObject);
            initialPart.parentPosition = new Vector3(0,0,0);
            newParts.Add(initialPart);

            foreach (Part p in parts) {
                newParts.Add(p);
            }
        }

        SetParts(newParts);
    }

    public void PresetInstructions(string name, List<Component> components, List<Part> parts) {
        // Not a very good way of doing this but it is a simple workaround to guarantee
        // that parts and components can be done one after the other.
        // Ideally a new inspector could be created that can display both components and
        // parts in same list which would nullify this step, however this is a limitation
        // of the default Unity inspector system.
        List<Part> newParts = new List<Part>();

        // Adds an initial filler object to the assembly to assure that the first part is not a component
        GameObject initialObject = GameObject.Find("Initial");
        initialObject.transform.SetParent(GetGameObject().transform);
        Part initialPart = new Part(initialObject);
        initialPart.parentPosition = new Vector3(0,0,0);
        newParts.Add(initialPart);

        if (name.Contains("bedsideTable")) {
            newParts.Add(components[0]);
            for (int i = 0; i <= 12; i++) {
                newParts.Add(parts[i]);
            }
            newParts.Add(components[1]);
            for (int i = 13; i <= 18; i++) {
                newParts.Add(parts[i]);
            }
            newParts.Add(components[2]);
            for (int i = 19; i <= 22; i++) {
                newParts.Add(parts[i]);
            }
            newParts.Add(components[3]);

            SetParts(newParts);
        }
    }


}