using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

public class CarGeneratorWindow : EditorWindow
{
    private ProBuilderCarGenerator.CarConfig carConfig = new ProBuilderCarGenerator.CarConfig();
    private ProBuilderCarGenerator generator;

    [MenuItem("Window/Car Generator")]
    public static void ShowWindow()
    {
        GetWindow<CarGeneratorWindow>("Car Generator");
    }

    private void OnGUI()
    {
        GUILayout.Label("ProBuilder Car Generator", EditorStyles.boldLabel);

        GUILayout.Space(10);

        carConfig.type = (ProBuilderCarGenerator.CarType)EditorGUILayout.EnumPopup("Car Type", carConfig.type);
        carConfig.bodyColor = EditorGUILayout.ColorField("Body Color", carConfig.bodyColor);
        carConfig.wheelColor = EditorGUILayout.ColorField("Wheel Color", carConfig.wheelColor);
        carConfig.scale = EditorGUILayout.Slider("Scale", carConfig.scale, 0.5f, 2f);

        GUILayout.Space(15);

        if (GUILayout.Button("Generate Car", GUILayout.Height(30)))
        {
            GenerateCar();
        }

        if (GUILayout.Button("Generate Basic Car", GUILayout.Height(25)))
        {
            carConfig.type = ProBuilderCarGenerator.CarType.Basic;
            GenerateCar();
        }

        if (GUILayout.Button("Generate Sport Car", GUILayout.Height(25)))
        {
            carConfig.type = ProBuilderCarGenerator.CarType.Sport;
            GenerateCar();
        }

        if (GUILayout.Button("Generate Luxury Car", GUILayout.Height(25)))
        {
            carConfig.type = ProBuilderCarGenerator.CarType.Luxury;
            GenerateCar();
        }
    }

    private void GenerateCar()
    {
        if (generator == null)
        {
            generator = CreateTemp();
        }

        generator.carConfig = carConfig;
        GameObject car = generator.GenerateCar();

        if (car != null)
        {
            EditorGUIUtility.PingObject(car);
            Debug.Log($"Generated {carConfig.type} Car!");
        }
        else
        {
            Debug.LogError("Failed to generate car!");
        }
    }

    private ProBuilderCarGenerator CreateTemp()
    {
        GameObject tempGO = new GameObject("_CarGeneratorTemp");
        var gen = tempGO.AddComponent<ProBuilderCarGenerator>();
        tempGO.hideFlags = HideFlags.HideInHierarchy;
        return gen;
    }
}
#endif
