using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace View
{
    [RequireComponent(typeof(Button))]
    public sealed class UpgradeButtonView : MonoBehaviour
    {
        [SerializeField] Button button;

        UpgradeWindow _window;

        [Inject]
        public void Init(UpgradeWindow window)
        {
            _window = window;
        }

        void Awake()
        {
            button.onClick.AddListener(Click);
        }

        void Reset()
        {
            button = GetComponent<Button>();
        }

        void Click()
        {
            _window.Show();
        }
    }
}