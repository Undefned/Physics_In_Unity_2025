using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
    [Header("Ссылки")]
    public LorentzForceParticle particle;
    public Slider chargeSlider;
    public Slider fieldSlider;
    public Slider massSlider;
    public TMP_Text chargeValueText;
    public TMP_Text fieldValueText;
    public TMP_Text massValueText;
    
    void Start()
    {
        // Инициализация слайдеров
        if (chargeSlider != null)
        {
            chargeSlider.minValue = -2f;
            chargeSlider.maxValue = 2f;
            chargeSlider.value = particle.charge;
            chargeSlider.onValueChanged.AddListener(OnChargeChanged);
        }
        
        if (fieldSlider != null)
        {
            fieldSlider.minValue = 0.1f;
            fieldSlider.maxValue = 5f;
            fieldSlider.value = particle.fieldStrength;
            fieldSlider.onValueChanged.AddListener(OnFieldChanged);
        }
        
        if (massSlider != null)
        {
            massSlider.minValue = 0.1f;
            massSlider.maxValue = 3f;
            massSlider.value = particle.mass;
            massSlider.onValueChanged.AddListener(OnMassChanged);
        }
        
        UpdateSliderTexts();
    }
    
    void OnChargeChanged(float value)
    {
        particle.SetCharge(value);
        if (chargeValueText != null)
            chargeValueText.text = value.ToString("F2");
    }
    
    void OnFieldChanged(float value)
    {
        particle.SetMagneticFieldStrength(value);
        if (fieldValueText != null)
            fieldValueText.text = value.ToString("F2");
    }
    
    void OnMassChanged(float value)
    {
        particle.mass = value;
        if (massValueText != null)
            massValueText.text = value.ToString("F2");
    }
    
    void UpdateSliderTexts()
    {
        if (chargeValueText != null)
            chargeValueText.text = chargeSlider.value.ToString("F2");
        
        if (fieldValueText != null)
            fieldValueText.text = fieldSlider.value.ToString("F2");
        
        if (massValueText != null)
            massValueText.text = massSlider.value.ToString("F2");
    }
    
    // Кнопки
    public void ResetParticle()
    {
        particle.ResetParticle();
    }
}