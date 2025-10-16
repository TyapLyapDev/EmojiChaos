#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ResourceBalanceChecker : EditorWindow
{
    private const string MenuPath = "Tools/";
    private const string Title = "��������� ������ �������";

    [MenuItem(MenuPath + Title)]
    public static void GenerateReport()
    {
        CustomLogger.ClearConsole();
        CustomLogger.LogWhite("��������� �������� ������������ ������ � �����������");

        if (TryFindSingleObjectByType(out LevelBootstrap levelBootstrap) == false)
        {
            CustomLogger.LogRed($"{nameof(levelBootstrap)} �� ������ ��� �� �������� ������������ �� �����!");

            return;
        }

        string propertyName = "_levels";
        SerializedObject serializedBootstrap = new(levelBootstrap);
        SerializedProperty levelsProperty = serializedBootstrap.FindProperty(propertyName);

        if (levelsProperty == null)
        {
            CustomLogger.LogRed($"�� ������� ����� �������� \"{propertyName}\"");

            return;
        }

        CustomLogger.LogBlue($"������� �������: {levelsProperty.arraySize}");

        for (int i = 0; i < levelsProperty.arraySize; i++)
        {
            SerializedProperty levelProperty = levelsProperty.GetArrayElementAtIndex(i);
            Level level = levelProperty.objectReferenceValue as Level;

            if (level == null)
            {
                CustomLogger.LogRed($"����������� ������ �� �������, ������: [{i}]");

                continue;
            }

            CustomLogger.LogBlue($"������� [{i}] \"{level.name}\"");
            AnalyzeLevelBalance(level);
        }
    }

    private static void AnalyzeLevelBalance(Level level)
    {
        string propertyName = "_crowds";
        SerializedObject serializedLevel = new(level);
        SerializedProperty crowdsProperty = serializedLevel.FindProperty(propertyName);

        if (crowdsProperty == null)
        {
            CustomLogger.LogRed($"�� ������� ����� �������� {propertyName}");

            return;
        }

        Car[] cars = level.GetComponentsInChildren<Car>(true);
        SortedDictionary<int, int> enemiesByType = new();

        for (int i = 0; i < crowdsProperty.arraySize; i++)
        {
            SerializedProperty crowdProperty = crowdsProperty.GetArrayElementAtIndex(i);
            int id = crowdProperty.FindPropertyRelative("_id").intValue;
            int quantity = crowdProperty.FindPropertyRelative("_quantity").intValue;
            int totalEnemies = quantity;

            if (enemiesByType.ContainsKey(id))
                enemiesByType[id] += totalEnemies;
            else
                enemiesByType.Add(id, totalEnemies);
        }

        Dictionary<int, CarAmmoInfo> ammoByCarType = new();

        foreach (Car car in cars)
        {
            if (car != null)
            {
                int id = car.Id;
                int bulletCount = car.BulletCount;

                if (ammoByCarType.ContainsKey(id))
                {
                    ammoByCarType[id].totalBulletCount += bulletCount;
                    ammoByCarType[id].cars.Add(car);
                }
                else
                {
                    ammoByCarType.Add(id, new CarAmmoInfo
                    {
                        totalBulletCount = bulletCount,
                        cars = new List<Car> { car }
                    });
                }
            }
        }

        HashSet<int> allIds = new(enemiesByType.Keys);
        allIds.UnionWith(ammoByCarType.Keys);

        bool hasBalanceIssues = false;

        foreach (int id in allIds)
        {
            bool hasEnemies = enemiesByType.TryGetValue(id, out int enemyCount);
            bool hasCar = ammoByCarType.TryGetValue(id, out CarAmmoInfo carInfo);

            if (hasEnemies == false && hasCar)
            {
                CustomLogger.LogRed($"ID [{id}]: ���� ���� ({carInfo.totalBulletCount} ��������), �� ��� ��������");
                hasBalanceIssues = true;

                continue;
            }


            if (hasEnemies && hasCar == false)
            {
                CustomLogger.LogRed($"ID [{id}]: ���� ����� ({enemyCount}), �� ��� ����");
                hasBalanceIssues = true;

                continue;
            }

            if (hasEnemies && hasCar)
            {
                if (enemyCount != carInfo.totalBulletCount)
                {
                    CustomLogger.LogRed($"ID [{id}]: ��������� ������ ({enemyCount}) � ��������({carInfo.totalBulletCount})");
                    hasBalanceIssues = true;
                }
            }
        }

        if (hasBalanceIssues == false)
            CustomLogger.LogGreen($"������ ��������, ������� �� ����������");
    }

    private static bool TryFindSingleObjectByType<T>(out T component) where T : Object
    {
        component = null;

        T[] list = FindObjectsByType<T>(FindObjectsSortMode.None);

        if (list.Length == 0 || list.Length > 1)
            return false;

        component = list[0];
        return true;
    }

    private class CarAmmoInfo
    {
        public int totalBulletCount;
        public List<Car> cars = new();
    }
}
#endif