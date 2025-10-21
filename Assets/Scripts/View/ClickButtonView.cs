using System;
using Service;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace View
{
    [RequireComponent(typeof(Button))]
    public sealed class ClickButtonView : MonoBehaviour
    {
        [SerializeField] Button button;
        [SerializeField] ClickTextView text;

        ClickService _clickService;
        Func<int, ClickEffectView> _viewFactory;

        [Inject]
        public void Init(ClickService clickService, Func<int, ClickEffectView> viewFactory)
        {
            _clickService = clickService;
            _viewFactory = viewFactory;
        }

        void Awake()
        {
            button.onClick.AddListener(Click);
        }

        void Reset()
        {
            button = GetComponent<Button>();
            text = GetComponentInChildren<ClickTextView>();
        }

        void Update() =>
            button.interactable = _clickService.CanClick;

        void Click()
        {
            _clickService.Click();
            _viewFactory(_clickService.ClickCount);
            text.UpdateState();
        }
    }
}