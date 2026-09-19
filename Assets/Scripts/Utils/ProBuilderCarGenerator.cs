using UnityEngine;

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

    public CarConfig carConfig = new CarConfig();

    public GameObject GenerateCar()
    {
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
    }

    private void GenerateBasicCar(GameObject carRoot)
    {
        CreateChassis(carRoot, new Vector3(2f, 0.8f, 4f), Vector3.zero, Color.red);
        CreateCabin(carRoot, new Vector3(1.6f, 0.6f, 1.8f), new Vector3(0, 0.6f, -0.3f), Color.red);
        CreateWheels(carRoot, 1.4f, 0.5f, 3f);
        CreateWindows(carRoot);
        carRoot.tag = "ClickZone";
    }

    private void GenerateSportCar(GameObject carRoot)
    {
        CreateChassis(carRoot, new Vector3(2.2f, 0.7f, 4.5f), Vector3.zero, Color.blue);
        CreateCabin(carRoot, new Vector3(1.8f, 0.5f, 1.5f), new Vector3(0, 0.5f, -0.2f), Color.blue);
        CreateSpoiler(carRoot);
        CreateWheels(carRoot, 1.6f, 0.6f, 3.2f);
        CreateWindows(carRoot);
        carRoot.tag = "ClickZone";
    }

    private void GenerateLuxuryCar(GameObject carRoot)
    {
        CreateChassis(carRoot, new Vector3(2.4f, 0.9f, 4.2f), Vector3.zero, Color.yellow);
        CreateCabin(carRoot, new Vector3(2f, 0.7f, 2f), new Vector3(0, 0.7f, -0.3f), Color.yellow);
        CreateWheels(carRoot, 1.5f, 0.55f, 3.1f);
        CreateWindows(carRoot, 1.8f);
        CreateBumpers(carRoot);
        carRoot.tag = "ClickZone";
    }

    private void CreateChassis(GameObject parent, Vector3 size, Vector3 position, Color color)
    {
        GameObject chassis = GameObject.CreatePrimitive(PrimitiveType.Cube);
        chassis.name = "Chassis";
        chassis.transform.parent = parent.transform;
        chassis.transform.localPosition = position;
        chassis.transform.localScale = size * carConfig.scale;

        RemoveCollider(chassis);
        ApplyMaterial(chassis, color);
    }

    private void CreateCabin(GameObject parent, Vector3 size, Vector3 position, Color color)
    {
        GameObject cabin = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cabin.name = "Cabin";
        cabin.transform.parent = parent.transform;
        cabin.transform.localPosition = position * carConfig.scale;
        cabin.transform.localScale = size * carConfig.scale;

        RemoveCollider(cabin);
        ApplyMaterial(cabin, color);
    }

    private void CreateWheels(GameObject parent, float wheelRadius, float wheelWidth, float wheelDistance)
    {
        Vector3[] wheelPositions = new Vector3[]
        {
            new Vector3(wheelDistance / 2f, 0, wheelDistance / 2f),
            new Vector3(-wheelDistance / 2f, 0, wheelDistance / 2f),
            new Vector3(wheelDistance / 2f, 0, -wheelDistance / 2f),
            new Vector3(-wheelDistance / 2f, 0, -wheelDistance / 2f)
        };

        string[] wheelNames = { "WheelFR", "WheelFL", "WheelRR", "WheelRL" };

        for (int i = 0; i < wheelPositions.Length; i++)
        {
            GameObject wheel = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            wheel.name = wheelNames[i];
            wheel.transform.parent = parent.transform;
            wheel.transform.localPosition = wheelPositions[i] * carConfig.scale;
            wheel.transform.localRotation = Quaternion.Euler(0, 0, 90);
            wheel.transform.localScale = new Vector3(wheelWidth, wheelRadius, wheelRadius) * carConfig.scale;

            RemoveCollider(wheel);
            ApplyMaterial(wheel, carConfig.wheelColor);
        }
    }

    private void CreateWindows(GameObject parent, float windowScale = 1f)
    {
        GameObject frontWindow = GameObject.CreatePrimitive(PrimitiveType.Cube);
        frontWindow.name = "WindowFront";
        frontWindow.transform.parent = parent.transform;
        frontWindow.transform.localPosition = new Vector3(0, 0.8f, 1.2f) * carConfig.scale;
        frontWindow.transform.localScale = new Vector3(1.4f * windowScale, 0.6f, 0.2f) * carConfig.scale;

        RemoveCollider(frontWindow);
        ApplyGlassMaterial(frontWindow);

        GameObject rearWindow = GameObject.CreatePrimitive(PrimitiveType.Cube);
        rearWindow.name = "WindowRear";
        rearWindow.transform.parent = parent.transform;
        rearWindow.transform.localPosition = new Vector3(0, 0.8f, -1f) * carConfig.scale;
        rearWindow.transform.localScale = new Vector3(1.2f * windowScale, 0.5f, 0.2f) * carConfig.scale;

        RemoveCollider(rearWindow);
        ApplyGlassMaterial(rearWindow);
    }

    private void CreateSpoiler(GameObject parent)
    {
        GameObject spoiler = GameObject.CreatePrimitive(PrimitiveType.Cube);
        spoiler.name = "Spoiler";
        spoiler.transform.parent = parent.transform;
        spoiler.transform.localPosition = new Vector3(0, 0.5f, -2.3f) * carConfig.scale;
        spoiler.transform.localScale = new Vector3(2f, 0.8f, 0.3f) * carConfig.scale;

        RemoveCollider(spoiler);
        ApplyMaterial(spoiler, carConfig.bodyColor);
    }

    private void CreateBumpers(GameObject parent)
    {
        GameObject frontBumper = GameObject.CreatePrimitive(PrimitiveType.Cube);
        frontBumper.name = "BumperFront";
        frontBumper.transform.parent = parent.transform;
        frontBumper.transform.localPosition = new Vector3(0, 0.3f, 2.2f) * carConfig.scale;
        frontBumper.transform.localScale = new Vector3(2.4f, 0.3f, 0.2f) * carConfig.scale;

        RemoveCollider(frontBumper);
        ApplyMaterial(frontBumper, Color.gray);

        GameObject rearBumper = GameObject.CreatePrimitive(PrimitiveType.Cube);
        rearBumper.name = "BumperRear";
        rearBumper.transform.parent = parent.transform;
        rearBumper.transform.localPosition = new Vector3(0, 0.3f, -2.2f) * carConfig.scale;
        rearBumper.transform.localScale = new Vector3(2.4f, 0.3f, 0.2f) * carConfig.scale;

        RemoveCollider(rearBumper);
        ApplyMaterial(rearBumper, Color.gray);
    }

    private void RemoveCollider(GameObject obj)
    {
        Collider collider = obj.GetComponent<Collider>();
        if (collider != null)
            DestroyImmediate(collider);
    }

    private void ApplyMaterial(GameObject obj, Color color)
    {
        Material mat = new Material(Shader.Find("Standard"));
        mat.color = color;
        obj.GetComponent<Renderer>().material = mat;
    }

    private void ApplyGlassMaterial(GameObject obj)
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
        obj.GetComponent<Renderer>().material = mat;
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
}
