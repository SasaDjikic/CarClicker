using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using System.IO;
#endif

public class UpgradeDataInitializer : MonoBehaviour
{
#if UNITY_EDITOR
    [MenuItem("CarClicker/Create UpgradeData Asset")]
    public static void CreateUpgradeDataAsset()
    {
        string resourcesPath = "Assets/Resources/Config";

        if (!Directory.Exists(resourcesPath))
        {
            Directory.CreateDirectory(resourcesPath);
            AssetDatabase.Refresh();
        }

        string assetPath = $"{resourcesPath}/UpgradeData.asset";

        UpgradeData upgradeData = ScriptableObject.CreateInstance<UpgradeData>();
        upgradeData.Initialize();

        AssetDatabase.CreateAsset(upgradeData, assetPath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log($"UpgradeData created at {assetPath}");
        EditorGUIUtility.PingObject(upgradeData);
    }
#endif
}
