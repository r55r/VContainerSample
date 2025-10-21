using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
