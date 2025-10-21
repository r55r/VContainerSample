using Service;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace View
{
    [RequireComponent(typeof(Button))]
    public sealed class OverdraftButtonView : MonoBehaviour
    {
        [SerializeField] Button button;
        [SerializeField] TMP_Text text;

        DebtService _service;

        void Reset()
        {
            button = GetComponent<Button>();
            text = GetComponentInChildren<TMP_Text>();
        }

        void Awake()
        {
            button.onClick.AddListener(ChangeState);
        }

        [Inject]
        public void Init(DebtService service)
        {
            _service = service;
            UpdateState();
        }

        void UpdateState()
        {
            text.text = _service.AllowOverdraft ? "Disable overdraft" : "Enable overdraft";
        }

        void ChangeState()
        {
            _service.AllowOverdraft = !_service.AllowOverdraft;
            UpdateState();
        }
    }
}