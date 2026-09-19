using UnityEngine;
#if UNITY_EDITOR
using UnityEditor.ProBuilder;
using UnityEditor.ProBuilder.AssetUtils;
using UnityEngine.ProBuilder;
using UnityEngine.ProBuilder.MeshOperations;
#endif

public class ProBuilderCarGenerator : MonoBehaviour
{
    public enum CarType
    {
        Basic,
        Sport,
        Luxury
    }

    [System.Serializable]
    public class CarConfig
    {
        public CarType type = CarType.Basic;
        public Color bodyColor = Color.red;
        public Color wheelColor = Color.black;
        public float scale = 1f;
    }

    [SerializeField] private CarConfig carConfig = new CarConfig();

    public GameObject GenerateCar()
    {
        #if UNITY_EDITOR
        GameObject carRoot = new GameObject("Car_" + carConfig.type);
        carRoot.transform.position = Vector3.zero;

        switch (carConfig.type)
        {
            case CarType.Basic:
                GenerateBasicCar(carRoot);
                break;
            case CarType.Sport:
                GenerateSportCar(carRoot);
                break;
            case CarType.Luxury:
                GenerateLuxuryCar(carRoot);
                break;
        }

        AddAnimator(carRoot);
        AddCarController(carRoot);

        return carRoot;
        #else
        Debug.LogError("ProBuilder Car Generator only works in Editor!");
        return null;
        #endif
    }

    #if UNITY_EDITOR
    private void GenerateBasicCar(GameObject carRoot)
    {
        // Chassis
        GameObject chassis = CreateChassis(carRoot, new Vector3(2f, 0.8f, 4f), Color.red);

        // Cabin
        GameObject cabin = CreateCabin(carRoot, new Vector3(1.6f, 0.6f, 1.8f), new Vector3(0, 0.6f, -0.3f), Color.red);

        // Wheels
        CreateWheels(carRoot, 1.4f, 0.5f, 3f);

        // Windows
        CreateWindows(carRoot);

        carRoot.tag = "ClickZone";
    }

    private void GenerateSportCar(GameObject carRoot)
    {
        // Chassis (longer, lower)
        GameObject chassis = CreateChassis(carRoot, new Vector3(2.2f, 0.7f, 4.5f), Color.blue);

        // Cabin (smaller, sportier)
        GameObject cabin = CreateCabin(carRoot, new Vector3(1.8f, 0.5f, 1.5f), new Vector3(0, 0.5f, -0.2f), Color.blue);

        // Spoiler
        CreateSpoiler(carRoot);

        // Wheels (bigger)
        CreateWheels(carRoot, 1.6f, 0.6f, 3.2f);

        // Windows
        CreateWindows(carRoot);

        carRoot.tag = "ClickZone";
    }

    private void GenerateLuxuryCar(GameObject carRoot)
    {
        // Chassis (wide, tall)
        GameObject chassis = CreateChassis(carRoot, new Vector3(2.4f, 0.9f, 4.2f), Color.yellow);

        // Cabin (larger)
        GameObject cabin = CreateCabin(carRoot, new Vector3(2f, 0.7f, 2f), new Vector3(0, 0.7f, -0.3f), Color.yellow);

        // Wheels (luxurious)
        CreateWheels(carRoot, 1.5f, 0.55f, 3.1f);

        // Windows (larger)
        CreateWindows(carRoot, 1.8f);

        // Bumpers (luxury details)
        CreateBumpers(carRoot);

        carRoot.tag = "ClickZone";
    }

    private GameObject CreateChassis(GameObject parent, Vector3 size, Color color)
    {
        GameObject chassis = new GameObject("Chassis");
        chassis.transform.parent = parent.transform;
        chassis.transform.localPosition = Vector3.zero;

        var pb = ProBuilderMesh.Create(chassis, PrimitiveType.Cube);
        pb.transform.localScale = size * carConfig.scale;

        ApplyMaterial(pb, color);
        pb.Refresh();

        return chassis;
    }

    private GameObject CreateCabin(GameObject parent, Vector3 size, Vector3 position, Color color)
    {
        GameObject cabin = new GameObject("Cabin");
        cabin.transform.parent = parent.transform;
        cabin.transform.localPosition = position * carConfig.scale;

        var pb = ProBuilderMesh.Create(cabin, PrimitiveType.Cube);
        pb.transform.localScale = size * carConfig.scale;

        ApplyMaterial(pb, color);
        pb.Refresh();

        return cabin;
    }

    private void CreateWheels(GameObject parent, float wheelRadius, float wheelWidth, float wheelDistance)
    {
        Vector3[] wheelPositions = new Vector3[]
        {
            new Vector3(wheelDistance / 2f, 0, wheelDistance / 2f),      // Front Right
            new Vector3(-wheelDistance / 2f, 0, wheelDistance / 2f),     // Front Left
            new Vector3(wheelDistance / 2f, 0, -wheelDistance / 2f),     // Rear Right
            new Vector3(-wheelDistance / 2f, 0, -wheelDistance / 2f)     // Rear Left
        };

        string[] wheelNames = { "WheelFR", "WheelFL", "WheelRR", "WheelRL" };

        for (int i = 0; i < wheelPositions.Length; i++)
        {
            GameObject wheel = new GameObject(wheelNames[i]);
            wheel.transform.parent = parent.transform;
            wheel.transform.localPosition = wheelPositions[i] * carConfig.scale;
            wheel.transform.localRotation = Quaternion.Euler(0, 0, 90);

            var pb = ProBuilderMesh.Create(wheel, PrimitiveType.Cylinder);
            pb.transform.localScale = new Vector3(wheelWidth, wheelRadius, wheelRadius) * carConfig.scale;

            ApplyMaterial(pb, carConfig.wheelColor);
            pb.Refresh();
        }
    }

    private void CreateWindows(GameObject parent, float windowScale = 1f)
    {
        // Front window
        GameObject frontWindow = new GameObject("WindowFront");
        frontWindow.transform.parent = parent.transform;
        frontWindow.transform.localPosition = new Vector3(0, 0.8f, 1.2f) * carConfig.scale;

        var pbFront = ProBuilderMesh.Create(frontWindow, PrimitiveType.Cube);
        pbFront.transform.localScale = new Vector3(1.4f * windowScale, 0.6f, 0.2f) * carConfig.scale;

        ApplyGlassMaterial(pbFront);
        pbFront.Refresh();

        // Rear window
        GameObject rearWindow = new GameObject("WindowRear");
        rearWindow.transform.parent = parent.transform;
        rearWindow.transform.localPosition = new Vector3(0, 0.8f, -1f) * carConfig.scale;

        var pbRear = ProBuilderMesh.Create(rearWindow, PrimitiveType.Cube);
        pbRear.transform.localScale = new Vector3(1.2f * windowScale, 0.5f, 0.2f) * carConfig.scale;

        ApplyGlassMaterial(pbRear);
        pbRear.Refresh();
    }

    private void CreateSpoiler(GameObject parent)
    {
        GameObject spoiler = new GameObject("Spoiler");
        spoiler.transform.parent = parent.transform;
        spoiler.transform.localPosition = new Vector3(0, 0.5f, -2.3f) * carConfig.scale;

        var pb = ProBuilderMesh.Create(spoiler, PrimitiveType.Cube);
        pb.transform.localScale = new Vector3(2f, 0.8f, 0.3f) * carConfig.scale;

        ApplyMaterial(pb, carConfig.bodyColor);
        pb.Refresh();
    }

    private void CreateBumpers(GameObject parent)
    {
        // Front bumper
        GameObject frontBumper = new GameObject("BumperFront");
        frontBumper.transform.parent = parent.transform;
        frontBumper.transform.localPosition = new Vector3(0, 0.3f, 2.2f) * carConfig.scale;

        var pbFront = ProBuilderMesh.Create(frontBumper, PrimitiveType.Cube);
        pbFront.transform.localScale = new Vector3(2.4f, 0.3f, 0.2f) * carConfig.scale;

        ApplyMaterial(pbFront, Color.gray);
        pbFront.Refresh();

        // Rear bumper
        GameObject rearBumper = new GameObject("BumperRear");
        rearBumper.transform.parent = parent.transform;
        rearBumper.transform.localPosition = new Vector3(0, 0.3f, -2.2f) * carConfig.scale;

        var pbRear = ProBuilderMesh.Create(rearBumper, PrimitiveType.Cube);
        pbRear.transform.localScale = new Vector3(2.4f, 0.3f, 0.2f) * carConfig.scale;

        ApplyMaterial(pbRear, Color.gray);
        pbRear.Refresh();
    }

    private void ApplyMaterial(ProBuilderMesh pb, Color color)
    {
        Material mat = new Material(Shader.Find("Standard"));
        mat.color = color;
        pb.GetComponent<Renderer>().material = mat;
    }

    private void ApplyGlassMaterial(ProBuilderMesh pb)
    {
        Material mat = new Material(Shader.Find("Standard"));
        mat.color = new Color(0.7f, 0.9f, 1f, 0.5f);
        mat.SetFloat("_Mode", 3);
        mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        mat.SetInt("_ZWrite", 0);
        mat.DisableKeyword("_ALPHATEST_ON");
        mat.EnableKeyword("_ALPHABLEND_ON");
        mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        mat.renderQueue = 3000;
        pb.GetComponent<Renderer>().material = mat;
    }

    private void AddAnimator(GameObject carRoot)
    {
        if (carRoot.GetComponent<Animator>() == null)
        {
            carRoot.AddComponent<Animator>();
        }
    }

    private void AddCarController(GameObject carRoot)
    {
        if (carRoot.GetComponent<CarController>() == null)
        {
            var controller = carRoot.AddComponent<CarController>();
            controller.baseSpeed = 5f;
            controller.clickAnimationDuration = 0.5f;
        }
    }
    #endif
}
