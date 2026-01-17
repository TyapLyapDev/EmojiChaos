using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Tutorial : InitializingWithConfigBehaviour<TutorialConfig>
{
    [SerializeField] TutorialCircle _circle;
    [SerializeField] List<TutorialItem> _items;

    public void Complete()
    {
        List<TutorialItem> items = new(_items);

        foreach (TutorialItem item in items)
            item.Deactivate();

        _items.Clear();
        Destroy(gameObject);
    }

    protected override void OnInitialize(TutorialConfig config)
    {
        foreach (TutorialItem item in _items)
            item.Initilize(config);

        _circle.Initialize();
        ActivateNextItem();
    }

    private void ActivateNextItem()
    {
        if(_items.Count == 0)
        {
            Complete();

            return;
        }

        TutorialItem item = _items.First();
        item.Activate();
        item.Deactivated += OnItemDeactivated;
    }

    private void OnItemDeactivated(TutorialItem item)
    {
        item.Deactivated -= OnItemDeactivated;
        _items.Remove(item);
        Destroy(item.gameObject);
        ActivateNextItem();
    }
}