using UnityEngine;
using UnityEngine.UI;

public abstract class SliderUI : MonoBehaviour
{
    [Header("SLIDER REFERENCES")]
    [SerializeField] private Slider _slider;
    [SerializeField] private Image _fill;
    private Gradient _gradient;

    protected void SetGradient(Gradient gradient)
    {
        _gradient = gradient;
    }

    protected void SetMaxValue(float maxValue)
    {
        _slider.maxValue = maxValue;
        _slider.value = _slider.maxValue;
        _fill.color = _gradient.Evaluate(_slider.normalizedValue);
    }

    protected void SetValue(float value)
    {
        _slider.value = value;
        _fill.color = _gradient.Evaluate(_slider.normalizedValue);
    }
}
