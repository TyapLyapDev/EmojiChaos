using UnityEngine;
using UnityEngine.UI;

namespace UI.LevelCards
{
    public class CrowdSequenceIndicator : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private Sprite _randomSequence;
        [SerializeField] private Sprite _deterministicSequence;

        public void SetSequenceType(CrowdSequenceType crowdSequenceType) =>
            _icon.sprite = crowdSequenceType ==
                        CrowdSequenceType.Deterministic ?
                        _deterministicSequence :
                        _randomSequence;
    }
}