using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float moveSpeed = 5f; // Скорость перемещения камеры
    public float zoomSpeed = 2f; // Скорость зума
    public float minZoom = 5f;   // Минимальный уровень зума (ближе)
    public float maxZoom = 20f;  // Максимальный уровень зума (дальше)

    void Update()
    {
        // Перемещение камеры с помощью клавиш WASD
        HandleMovement();

        // Масштабирование камеры с помощью колёсика мыши
        HandleZoom();
    }

    void HandleMovement()
    {
        float moveX = 0f;
        float moveY = 0f;

        // W или стрелка вверх - движение вверх
        if (Input.GetKey(KeyCode.W))
        {
            moveY = 1f;
        }
        // S или стрелка вниз - движение вниз
        if (Input.GetKey(KeyCode.S))
        {
            moveY = -1f;
        }
        // A или стрелка влево - движение влево
        if (Input.GetKey(KeyCode.A))
        {
            moveX = -1f;
        }
        // D или стрелка вправо - движение вправо
        if (Input.GetKey(KeyCode.D))
        {
            moveX = 1f;
        }

        // Перемещение камеры в зависимости от введенных значений
        Vector3 moveDirection = new Vector3(moveX, moveY, 0f).normalized;
        transform.position += moveDirection * moveSpeed * Time.deltaTime;
    }

    void HandleZoom()
    {
        // Получаем значение прокрутки колёсика мыши
        float scrollData = Input.GetAxis("Mouse ScrollWheel");

        // Масштабируем камеру, изменяя её ортографический размер (для 2D камеры)
        Camera.main.orthographicSize -= scrollData * zoomSpeed;
        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, minZoom, maxZoom); // Ограничиваем зум
    }
}
