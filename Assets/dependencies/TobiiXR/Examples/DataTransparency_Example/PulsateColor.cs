using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PulsateColor : MonoBehaviour
{
#pragma warning disable 649 // Field is never assigned
    [SerializeField] private Color _normalColor;
    [SerializeField] private Color _pulsateColor;
    [SerializeField] private Image _image;
    [SerializeField] private float _duration;
#pragma warning restore 649

    private Color _fromColor;
    private Color _toColor;
    private float _time;

    void Start()
    {
        _fromColor = _normalColor;
        _toColor = _pulsateColor;
        _time = 0;
    }

    void Update()
    {
        _time += Time.deltaTime;
        float elapsedAmount = _time / _duration;
        float r = Mathf.SmoothStep(_fromColor.r, _toColor.r, elapsedAmount);
        float g = Mathf.SmoothStep(_fromColor.g, _toColor.b, elapsedAmount);
        float b = Mathf.SmoothStep(_fromColor.b, _toColor.b, elapsedAmount);
        _image.color = new Color(r, g, b);

        if (_time > _duration)
        {
            _time = 0;
            Color _previousFromColor = _fromColor;
            _fromColor = _toColor;
            _toColor = _previousFromColor;
        }
    }
}
