using UnityEngine;

// Скрипт визуализации электростатического поля.
// Отображает поле как сетку стрелок (векторное поле) в Scene View.
public class FieldVisualizerLab7 : MonoBehaviour
{
    // Ссылка на систему зарядов, которая умеет считать поле
    public ElectricFieldSystem fieldSystem;

    // Размер области визуализации поля в единицах Unity
    public float gridSize = 5f;

    // Расстояние между точками сетки
    public float step = 1f;

    // Масштаб стрелок поля (чтобы они были видимыми, но не слишком большими)
    public float arrowScale = 0.5f;

    // Update вызывается каждый кадр
    // Используется для динамической визуализации поля
    void Update()
    {
        // Если ссылка на поле не установлена, ничего не делаем
        if (fieldSystem == null) return;

        // Проходим по сетке точек XY в пределах [-gridSize, gridSize]
        for (float x = -gridSize; x <= gridSize; x += step)
        {
            for (float y = -gridSize; y <= gridSize; y += step)
            {
                // Текущая точка сетки, на которой будем рисовать поле
                Vector3 point = new Vector3(x, y, 0);

                // Получаем вектор напряжённости электрического поля в этой точке
                Vector3 E = fieldSystem.GetField(point);

                // Если поле почти нулевое, стрелку рисовать не нужно
                if (E.magnitude > 0.001f)
                {
                    // Рисуем стрелку в Scene View
                    // point — начало стрелки
                    // E.normalized * arrowScale — направление и длина
                    // Color.green — цвет стрелки
                    Debug.DrawRay(
                        point,
                        E.normalized * arrowScale,
                        Color.green
                    );
                }
            }
        }
    }
}
