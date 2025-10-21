using System;
using UnityEngine;
using UnityEngine.UI;

namespace tutorial
{
    public class SayHelloButton : MonoBehaviour
    {
        private Button _button;

        public event Action OnClick;

        private void Awake()
        {
            _button = GetComponent<Button>();

            _button.onClick.AddListener(() =>
            {
                OnClick?.Invoke();
            });
        }
    }
}
