public class HealthUI : SliderUI
{
    public void SetMaxHealth(float maxHealth)
    {
        SetMaxValue(maxHealth);
    }

    public void SetCurrentHealth(float currentHealth)
    {
        SetValue(currentHealth);
    }
}
