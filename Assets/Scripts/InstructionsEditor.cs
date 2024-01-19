using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(Instructions)), CanEditMultipleObjects]
public class InstructionsEditor : Editor
{

    private SerializedProperty gameObjectField;

    private void OnEnable()
    {
        // Initialize SerializedProperty for the field you want to monitor
        gameObjectField = serializedObject.FindProperty("FurnitureObject");
    }

    public override void OnInspectorGUI()
    {

        //base.OnInspectorGUI();

        serializedObject.Update();

        Instructions instructions = (Instructions)target;

        List<Component> components = instructions.components;

        List<Part> parts = instructions.parts;

        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(gameObjectField);
        if (EditorGUI.EndChangeCheck()) {
            components.Clear();
            parts.Clear();
            foreach (Transform c in instructions.FurnitureObject.transform) {
                if (c.childCount > 0) {
                    List<Part> componentParts = new List<Part>();
                    Component newComponent = new Component(c.gameObject);
                    components.Add(newComponent);
                    foreach (Transform sc in c) {
                        componentParts.Add(new Part(sc.gameObject));
                    }
                    newComponent.SetParts(componentParts);
                } else {
                    parts.Add(new Part(c.gameObject));
                }
            }
        }

        DrawDefaultInspector();

        serializedObject.ApplyModifiedProperties();
    }
}
