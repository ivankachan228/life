using UnityEngine;

public class ClickableSquare : MonoBehaviour
{
    public int x; // Координата по оси X
    public int y; // Координата по оси Y
    private FieldGenerator fieldGenerator; // Ссылка на FieldGenerator

    // Метод для инициализации координат и ссылки на FieldGenerator
    public void Initialize(int x, int y, FieldGenerator fieldGenerator)
    {
        this.x = x;
        this.y = y;
        this.fieldGenerator = fieldGenerator;
    }

    void OnMouseDown()
    {
        // Вызываем метод OnClick в FieldGenerator при нажатии
        if (fieldGenerator != null)
        {
            fieldGenerator.OnClick(x, y); // Передаем координаты нажатия
        }
    }
}
