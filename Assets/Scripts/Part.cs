using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum States {
    DISABLED,
    WAITING,
    CORRECT,
    FINISHED
}

[System.Serializable]
public class Part
{
    [SerializeField] GameObject gameObject;
    [SerializeField] bool useTracking;
    List<GameObject> meshes;
    States state;
    Material material;

    // Whether or not the part should be screwed in
    [SerializeField] public bool screws;

    // The position that this part is a sub part of should be while this part is being added
    [SerializeField] public Vector3 parentPosition;
    // Same as parentPosition but for rotation
    [SerializeField] public Vector3 parentRotation;

    [HideInInspector] public bool showOutline;

    private static List<Color> colors = new List<Color>
    {
        new Color(0, 0, 0, 0),                   //blank
        new Color(0.57f, 0.83f, 0.99f, 1),       //blue
        new Color(0.22f, 0.95f, 0.25f, 1),       //green
        new Color(0.8f, 0.22f, 0.13f, 0)         //red (transparent)
    };

    private static List<string> materials = new List<string>
    {
        @"Materials/disabled",
        @"Materials/waiting",
        @"Materials/correct",
        @"Materials/inactive"
    };

    public Part(GameObject gameObject) {
        //Debug.Log("Constructor called");
        this.gameObject = gameObject;
        state = States.DISABLED;
        SetMeshes();
    }

    public Part() {

    }

    public GameObject GetGameObject() {
        return gameObject;
    }

    protected virtual Color GetColor() {
        return colors[(int)GetState()];
    }

    protected virtual string GetMaterial()
    {
        return materials[(int)GetState()];
    }

    public bool UsesTracking() {
        return useTracking;
    }

    public bool IsScrewed() {
        return screws;
    }

    //Returns a list of all objects which have a mesh. If parent object has a mesh it will not return children
    public void SetMeshes() {

        MeshRenderer[] meshComponents = gameObject.GetComponentsInChildren<MeshRenderer>(true);
        meshes = new List<GameObject>();

        foreach (MeshRenderer m in meshComponents) {
            meshes.Add(m.gameObject);
        }
        var testString = "";
        foreach(GameObject g in meshes) {
            testString += g;
        }
        //Debug.Log(testString);
    }

    public void SetOutline(){

        foreach (GameObject g in meshes) {
            Outline outline = g.GetComponent<Outline>();
            if (outline == null) {
                outline = g.AddComponent<Outline>() as Outline;
            }
            outline.enabled = showOutline;
            outline.OutlineWidth = 10;
            outline.OutlineColor = GetColor();
        }
        
    }

    public void SetMaterial() {
        foreach (GameObject g in meshes) {
            Outline outline = g.GetComponent<Outline>();
            if (outline == null) {
                outline = g.AddComponent<Outline>() as Outline;
            }
            outline.enabled = false;
            material = Resources.Load<Material>(GetMaterial());
            Material[] newMaterials = g.GetComponent<MeshRenderer>().materials;
            newMaterials[0] = material;
            g.GetComponent<MeshRenderer>().materials = newMaterials;
        }
        //Debug.Log(material);
    }

    public void SetState(States state) {
        this.state = state;
        //Debug.Log(state);
        SetMeshes();
        SetMaterial();
        SetOutline();
    }

    public States GetState() {
        return state;
    }

}
