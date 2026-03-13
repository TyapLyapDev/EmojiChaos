#if UNITY_EDITOR
using System.Collections.Generic;
using EmojiChaos.Data;
using EmojiChaos.Entities.Cars;
using UnityEditor;
using UnityEngine;

namespace EmojiChaos.UtilsSpace.Editor
{
    public class ResourceBalanceChecker : EditorWindow
    {
        private const string MenuPath = "Tools/";
        private const string Title = "Check the level balance";

        [MenuItem(MenuPath + Title)]
        public static void GenerateReport()
        {
            CustomLogger.ClearConsole();
            CustomLogger.Log("Checking the level balance");

            Level[] allLevels = Resources.LoadAll<Level>(string.Empty);

            if (allLevels == null || allLevels.Length == 0)
            {
                CustomLogger.LogRed($"No levels found along the path in Resources/");

                return;
            }

            CustomLogger.LogBlue($"Total levels: {allLevels.Length}");

            foreach (Level level in allLevels)
            {
                if (level == null)
                {
                    CustomLogger.LogRed("Null level detected in Resources");

                    continue;
                }

                CustomLogger.LogBlue($"Level \"{level.name}\"");
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
                CustomLogger.LogRed($"Missing property {propertyName}");

                return;
            }

            Car[] cars = level.GetComponentsInChildren<Car>(true);
            SortedDictionary<int, int> enemiesByType = new ();

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

            Dictionary<int, CarAmmoInfo> ammoByCarType = new ();
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
                    CustomLogger.LogRed($"ID [{id}]: there is a car ({carInfo.totalBulletCount}), but there are no enemies");
                    hasBalanceIssues = true;

                    continue;
                }


                if (hasEnemies && hasCar == false)
                {
                    CustomLogger.LogRed($"ID [{id}]: there are enemies ({enemyCount}), but there is no car");
                    hasBalanceIssues = true;

                    continue;
                }

                if (hasEnemies && hasCar)
                {
                    if (enemyCount != carInfo.totalBulletCount)
                    {
                        CustomLogger.LogRed($"ID [{id}]: number of enemies ({enemyCount}), number of shells({carInfo.totalBulletCount})");
                        hasBalanceIssues = true;
                    }
                }
            }

            CustomLogger.Log($"Total crowds: {totalCrowds}, enemies: {totalEnemies}, cars: {totalCars}, types: {totalTypes}");

            if (hasBalanceIssues == false)
                CustomLogger.LogGreen($"Level checked, no errors found");
        }

        private class CarAmmoInfo
        {
            public int totalBulletCount;
            public List<Car> cars = new ();
        }
    }
}
#endif