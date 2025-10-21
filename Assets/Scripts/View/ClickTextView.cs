using Service;
using TMPro;
using UnityEngine;
using VContainer;

namespace View
{
    [RequireComponent(typeof(TMP_Text))]
    public sealed class ClickTextView : MonoBehaviour
    {
        [SerializeField] TMP_Text text;

        ClickService _clickService;

        [Inject]
        public void Init(ClickService clickService)
        {
            _clickService = clickService;
            UpdateState();
        }

        void Reset() =>
            text = GetComponent<TMP_Text>();

        public void UpdateState() =>
            text.text = $"Clicks: {_clickService.ClickCount}";
    }
}