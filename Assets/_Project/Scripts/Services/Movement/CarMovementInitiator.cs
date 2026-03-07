using System;

public class CarMovementInitiator : IDisposable
{
    private readonly SlotReservator _slotReservator;
    private readonly CarMovementDirector _speedDirector;

    public CarMovementInitiator(SlotReservator swipeCoordinator, CarMovementDirector speedDirector)
    {
        _slotReservator = swipeCoordinator ?? throw new ArgumentNullException(nameof(swipeCoordinator));
        _speedDirector = speedDirector ?? throw new ArgumentNullException(nameof(speedDirector));

        _slotReservator.SlotReserved += OnSlotReserved;
    }

    public void Dispose() =>
        _slotReservator.SlotReserved -= OnSlotReserved;

    private void OnSlotReserved(Car car) =>
        _speedDirector.Register(car);
}