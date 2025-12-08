#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ResourceBalanceChecker : EditorWindow
{
    private const string MenuPath = "Tools/";
    private const string Title = "Проверить баланс уровней";

    [MenuItem(MenuPath + Title)]
    public static void GenerateReport()
    {
        CustomLogger.ClearConsole();
        CustomLogger.Log("ПРОЦЕДУРА ПРОВЕРКИ СООТВЕТСТВИЯ ВРАГОВ И БОЕПРИПАСОВ");

        Level[] allLevels = Resources.LoadAll<Level>(string.Empty);

        if (allLevels == null || allLevels.Length == 0)
        {
            CustomLogger.LogRed($"Не найдено уровней в папке Resources/");

            return;
        }

        CustomLogger.LogBlue($"Найдено уровней: {allLevels.Length}");

        foreach (Level level in allLevels)
        {
            if (level == null)
            {
                CustomLogger.LogRed("Обнаружен null уровень в Resources");

                continue;
            }

            CustomLogger.LogBlue($"Уровень \"{level.name}\"");
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
            CustomLogger.LogRed($"Не удалось найти свойство {propertyName}");

            return;
        }

        Car[] cars = level.GetComponentsInChildren<Car>(true);
        SortedDictionary<int, int> enemiesByType = new();

        int totalCrowds = crowdsProperty.arraySize;
        int totalEnemies = 0;

        for (int i = 0; i < totalCrowds; i++)
        {
            SerializedProperty crowdProperty = crowdsProperty.GetArrayElementAtIndex(i);
            int id = crowdProperty.FindPropertyRelative("_id").intValue;
            int quantity = crowdProperty.FindPropertyRelative("_quantity").intValue;
            totalEnemies += quantity;

            if (enemiesByType.ContainsKey(id))
                enemiesByType[id] += quantity;
            else
                enemiesByType.Add(id, quantity);
        }

        Dictionary<int, CarAmmoInfo> ammoByCarType = new();
        int totalCars = cars.Length;

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
        int totalTypes = allIds.Count;

        bool hasBalanceIssues = false;

        foreach (int id in allIds)
        {
            bool hasEnemies = enemiesByType.TryGetValue(id, out int enemyCount);
            bool hasCar = ammoByCarType.TryGetValue(id, out CarAmmoInfo carInfo);

            if (hasEnemies == false && hasCar)
            {
                CustomLogger.LogRed($"ID [{id}]: есть авто ({carInfo.totalBulletCount} припасов), но нет враговов");
                hasBalanceIssues = true;

                continue;
            }


            if (hasEnemies && hasCar == false)
            {
                CustomLogger.LogRed($"ID [{id}]: есть враги ({enemyCount}), но нет авто");
                hasBalanceIssues = true;

                continue;
            }

            if (hasEnemies && hasCar)
            {
                if (enemyCount != carInfo.totalBulletCount)
                {
                    CustomLogger.LogRed($"ID [{id}]: Дисбаланс врагов ({enemyCount}) и припасов({carInfo.totalBulletCount})");
                    hasBalanceIssues = true;
                }
            }
        }

        CustomLogger.Log($"Всего толп: {totalCrowds}, врагов: {totalEnemies}, авто: {totalCars}, типов: {totalTypes}");

        if (hasBalanceIssues == false)
            CustomLogger.LogGreen($"Баланс проверен, проблем не обнаружено");
    }

    private class CarAmmoInfo
    {
        public int totalBulletCount;
        public List<Car> cars = new();
    }
}
#endif