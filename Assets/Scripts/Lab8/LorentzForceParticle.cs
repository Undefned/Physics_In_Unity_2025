using UnityEngine;
using TMPro;

public class LorentzForceParticle : MonoBehaviour
{
    [Header("Физические параметры")]
    public float charge = 1.0f;  // Заряд (может быть отрицательным)
    public float mass = 1.0f;     // Масса
    public Vector3 initialVelocity = new Vector3(5f, 0f, 0f); // Начальная скорость
    
    [Header("Магнитное поле")]
    public Vector3 magneticField = new Vector3(0f, 0f, 1f); // B = (0,0,Bz)
    public float fieldStrength = 1.0f; // Сила поля B_z

    [Header("Электрическое поле (для продвинутого уровня)")]
    public Vector3 electricField = Vector3.zero;
    public bool useElectricField = false;
    
    [Header("Визуализация")]
    public TrailRenderer trail;
    public Color positiveChargeColor = Color.blue;
    public Color negativeChargeColor = Color.red;
    
    [Header("UI Элементы")]
    public TMP_Text speedText;
    public TMP_Text energyText;
    public TMP_Text radiusText;
    public TMP_Text periodText;
    public TMP_Text chargeText;

    // Приватные переменные
    private Vector3 currentVelocity;
    private Vector3 acceleration;
    private float kineticEnergy;
    private float orbitRadius;
    private float orbitPeriod;
    
    void Start()
    {
        // Инициализация
        currentVelocity = initialVelocity;
        magneticField = new Vector3(0f, 0f, fieldStrength);
        
        // Настройка следа
        if (trail != null)
        {
            trail.startColor = (charge > 0) ? positiveChargeColor : negativeChargeColor;
            trail.endColor = (charge > 0) ? positiveChargeColor : negativeChargeColor;
        }
        
        UpdateUI();
    }
    
    void FixedUpdate()
    {

        Vector3 lorentzForce;
        if (useElectricField)
        {
            // Полная сила Лоренца: F = q(E + v × B)
            lorentzForce = charge * (electricField + Vector3.Cross(currentVelocity, magneticField));
        }
        else
        {
            lorentzForce = charge * Vector3.Cross(currentVelocity, magneticField);
        }
        // Сила Лоренца: F = q(v × B)
        // Vector3 lorentzForce = charge * Vector3.Cross(currentVelocity, magneticField);
        
        // Ускорение: a = F/m
        acceleration = lorentzForce / mass;
        
        // Интегрирование скорости (метод Эйлера)
        currentVelocity += acceleration * Time.fixedDeltaTime;
        
        // Обновление позиции
        transform.position += currentVelocity * Time.fixedDeltaTime;
        
        // Расчет физических величин
        CalculatePhysics();
        UpdateUI();
        
        // Визуализация силы (опционально)
        Debug.DrawRay(transform.position, lorentzForce.normalized, Color.green);
    }
    
    void CalculatePhysics()
    {
        // Кинетическая энергия: K = 1/2 mv²
        kineticEnergy = 0.5f * mass * currentVelocity.sqrMagnitude;
        
        // Радиус орбиты: R = mv/(|q|B)
        float speed = currentVelocity.magnitude;
        float bMagnitude = magneticField.magnitude;
        
        if (Mathf.Abs(charge) > 0.0001f && bMagnitude > 0.0001f)
        {
            orbitRadius = (mass * speed) / (Mathf.Abs(charge) * bMagnitude);
            orbitPeriod = (2f * Mathf.PI * mass) / (Mathf.Abs(charge) * bMagnitude);
        }
    }
    
    void UpdateUI()
    {
        if (speedText != null)
            speedText.text = $"Скорость: {currentVelocity.magnitude:F2} м/с";
        
        if (energyText != null)
            energyText.text = $"Энергия: {kineticEnergy:F2} Дж";
        
        if (radiusText != null)
            radiusText.text = $"Радиус: {orbitRadius:F2} м";
        
        if (periodText != null)
            periodText.text = $"Период: {orbitPeriod:F2} с";
        
        if (chargeText != null)
            chargeText.text = $"Заряд: {charge:F2} Кл";
    }
    
    // Методы для изменения параметров
    public void SetCharge(float newCharge)
    {
        charge = newCharge;
        
        // Меняем цвет следа при смене заряда
        if (trail != null)
        {
            trail.startColor = (charge > 0) ? positiveChargeColor : negativeChargeColor;
            trail.endColor = (charge > 0) ? positiveChargeColor : negativeChargeColor;
        }
    }
    
    public void SetMagneticFieldStrength(float strength)
    {
        fieldStrength = strength;
        magneticField = new Vector3(0f, 0f, fieldStrength);
    }
    
    // Перезапуск частицы
    public void ResetParticle()
    {
        transform.position = Vector3.zero;
        currentVelocity = initialVelocity;
        trail.Clear();
    }

    void OnGUI()
    {
        GUIStyle style = new GUIStyle();
        style.fontSize = 20;
        style.normal.textColor = Color.white;
        
        GUI.Label(new Rect(10, 100, 300, 50), 
            $"dK/dt ≈ {CalculateEnergyDerivative():E2} Дж/с", style);
        
        if (Mathf.Abs(CalculateEnergyDerivative()) < 0.01f)
        {
            GUI.Label(new Rect(10, 130, 300, 50), 
                "✓ Энергия сохраняется!", style);
        }
    }

    private float previousEnergy = 0f;
    private float energyDerivative = 0f;

    float CalculateEnergyDerivative()
    {
        float currentEnergy = kineticEnergy;
        energyDerivative = (currentEnergy - previousEnergy) / Time.fixedDeltaTime;
        previousEnergy = currentEnergy;
        return energyDerivative;
    }
}