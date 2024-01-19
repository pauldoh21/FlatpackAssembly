using System.Collections;
using System.Collections.Generic;
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

    public void CombineParts(List<Component> components, List<Part> parts) {
        List<Part> newParts = new List<Part>();

        // Adds an initial filler object to the assembly to assure that the first part is not a component
        GameObject initialObject = GameObject.Find("Initial");
        initialObject.transform.SetParent(GetGameObject().transform);
        Part initialPart = new Part(initialObject);
        initialPart.parentPosition = new Vector3(0,0,0);
        newParts.Add(initialPart);

        foreach(Component c in components) {
            newParts.Add(c);
        }
        foreach(Part p in parts) {
            newParts.Add(p);
        }
        SetParts(newParts);
    }


}