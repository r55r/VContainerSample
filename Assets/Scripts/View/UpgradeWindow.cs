using Service;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace View
{
    public sealed class UpgradeWindow : MonoBehaviour
    {
        [SerializeField] Button closeButton;
        [SerializeField] Button upgradeButton;
        [SerializeField] TMP_Text upgradePrice;

        UpgradeService _upgradeService;

        void Awake()
        {
            upgradeButton.onClick.AddListener(Upgrade);
            closeButton.onClick.AddListener(Hide);
            Hide();
        }

        [Inject]
        void Init(UpgradeService upgradeService)
        {
            _upgradeService = upgradeService;
        }

        void Hide()
        {
            gameObject.SetActive(false);
        }

        public void Show()
        {
            upgradePrice.text = _upgradeService.UpgradePrice.ToString();
            upgradeButton.interactable = _upgradeService.CanUpgrade();
            gameObject.SetActive(true);
        }

        void Upgrade()
        {
            _upgradeService.Upgrade();
            Hide();
        }
    }
}