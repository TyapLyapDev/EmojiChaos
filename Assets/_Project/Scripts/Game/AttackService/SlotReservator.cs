using System;
using System.Collections.Generic;

public class SlotReservator : IDisposable
{
    private readonly IReadOnlyList<AttackSlot> _slots;
    private readonly ISwipeStrategy _carSwipeStrategy;

    public SlotReservator(IReadOnlyList<AttackSlot> slots, ISwipeStrategy carSwipeStrategy)
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
        if (swipeableObject is Car car)
            if (TryGetAvailableSlot(out AttackSlot attackSlot))
                if (car.TryReservationSlot(attackSlot, direction))
                    SlotReserved?.Invoke(car);
    }

    private bool TryGetAvailableSlot(out AttackSlot attackSlot)
    {
        foreach (AttackSlot slot in _slots)
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