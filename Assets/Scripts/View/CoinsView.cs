using Service;
using TMPro;
using UnityEngine;
using VContainer;

namespace View
{
    [RequireComponent(typeof(TMP_Text))]
    public sealed class CoinsView : MonoBehaviour
    {
        [SerializeField] TMP_Text text;

        CoinService _service;

        void Reset()
        {
            text = GetComponent<TMP_Text>();
        }

        [Inject]
        public void Init(CoinService service)
        {
            _service = service;
        }

        void Update() =>
            text.text = _service.Coins.ToString();
    }
}