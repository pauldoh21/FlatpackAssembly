using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDetected : MonoBehaviour
{
    public bool tracked;

    private Outline outline;
    private Renderer mat;

    private Color redOutline = new Color(1f, 0.5f, 0.4f, 1f);
    private Color redFill = new Color(0.7f, 0.1f, 0f, 0.5f);

    private Color blueOutline = new Color(0f, 0.5f, 1f, 1f);
    private Color blueFill = new Color(0f, 0.5f, 1f, 0.5f);

    // Start is called before the first frame update
    void Start()
    {
        tracked = false;
        outline = GetComponent<Outline>();
        mat = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (tracked) {
            mat.material.SetColor("_Color", blueFill);
            outline.OutlineColor = blueOutline;
        } else {
            mat.material.SetColor("_Color", redFill);
            outline.OutlineColor = redOutline;
        }
    }
}
