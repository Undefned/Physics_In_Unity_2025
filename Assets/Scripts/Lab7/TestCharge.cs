using UnityEngine;

// Скрипт пробного заряда.
// Моделирует движение заряженной частицы
// в электростатическом поле других зарядов.
public class TestCharge : MonoBehaviour
{
    // Величина пробного заряда q0
    // Определяет, с какой силой поле действует на частицу
    public float q0 = 1f;

    // Масса пробного заряда
    // Нужна для расчёта ускорения (второй закон Ньютона)
    public float mass = 1f;

    // Ссылка на систему электрического поля,
    // которая знает все источники (Charge)
    public ElectricFieldSystem fieldSystem;

    // Текущая скорость пробного заряда
    // Храним вручную, так как не используем Rigidbody
    private Vector3 velocity;

    // FixedUpdate вызывается с постоянным шагом времени
    // Используется для физического моделирования
    void FixedUpdate()
    {
        // Получаем напряжённость электрического поля
        // в точке, где сейчас находится пробный заряд
        Vector3 E = fieldSystem.GetField(transform.position);

        // Вычисляем силу, действующую на пробный заряд:
        // F = q0 * E
        Vector3 F = q0 * E;

        // Находим ускорение по второму закону Ньютона:
        // a = F / m
        Vector3 a = F / mass;

        // Обновляем скорость методом Эйлера:
        // v(t + dt) = v(t) + a * dt
        velocity += a * Time.fixedDeltaTime;

        // Обновляем положение пробного заряда:
        // r(t + dt) = r(t) + v * dt
        transform.position += velocity * Time.fixedDeltaTime;
    }
}
