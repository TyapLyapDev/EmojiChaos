using UnityEngine;

public class CutSceneCameraAnimator : MonoBehaviour
{
    [SerializeField] private CutSceneBootstrap _sceneBootstrap;
    [SerializeField] private Car _firstCar;
    [SerializeField] private Car _secondCar;
    [SerializeField] private Car _thirdCar;

    public void StartEnemySpawn()
    {
        _sceneBootstrap.StartEnemySpawn();
    }

    public void StartFirstCar()
    {
        _sceneBootstrap.SlotReservator.ReserveSlot(_firstCar, -1);
    }

    public void StartSecondCar()
    {
        _sceneBootstrap.SlotReservator.ReserveSlot(_secondCar, -1);
    }

    public void StartThirdCar()
    {
        _sceneBootstrap.SlotReservator.ReserveSlot(_thirdCar, -1);
    }
}