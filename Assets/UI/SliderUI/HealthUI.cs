using UnityEngine;

public class HealthUI : SliderUI
{
    [Header("GRADIENTS")]
    [SerializeField] private Gradient _alliedGradient;
    [SerializeField] private Gradient _hostileGradient;

    public void SetMaxHealth(float maxHealth, LayerMask layer)
    {
        if (layer == LayerMask.NameToLayer("Allied"))
        {
            SetGradient(_alliedGradient);
        }
        else
        {
            SetGradient(_hostileGradient);
        }

        SetMaxValue(maxHealth);
    }

    public void SetCurrentHealth(float currentHealth)
    {
        SetValue(currentHealth);
    }
}
