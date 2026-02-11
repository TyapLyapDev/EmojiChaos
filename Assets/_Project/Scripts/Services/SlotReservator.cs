using System;
using System.Collections.Generic;

public class SlotReservator : IDisposable
{
    private readonly IReadOnlyList<Rack> _slots;
    private readonly ISwipeStrategy _carSwipeStrategy;

    public SlotReservator(IReadOnlyList<Rack> slots, ISwipeStrategy carSwipeStrategy)
    {
        _slots = slots ?? throw new ArgumentNullException(nameof(slots));
        _carSwipeStrategy = carSwipeStrategy ?? throw new ArgumentNullException(nameof(carSwipeStrategy));

        _carSwipeStrategy.HasSwipe += OnSwipe;
    }

    public event Action<Car> SlotReserved;

    public void Dispose()
    {
        _carSwipeStrategy.HasSwipe -= OnSwipe;
    }

    private void OnSwipe(ISwipeable swipeableObject, int direction)
    {
        if (swipeableObject is Car car == false)
            return;

        if (TryGetAvailableSlot(out Rack attackSlot))
        {
            if (car.TryReservationSlot(attackSlot, direction))
                SlotReserved?.Invoke(car);
        }
        else
        {
            car.HandleUnavailableStatus();
            Audio.Sfx.PlayCarCantDrive();
        }
    }

    private bool TryGetAvailableSlot(out Rack attackSlot)
    {
        foreach (Rack slot in _slots)
        {
            if (slot.IsAvailable)
            {
                attackSlot = slot;

                return true;
            }
        }

        attackSlot = null;

        return false;
    }
}