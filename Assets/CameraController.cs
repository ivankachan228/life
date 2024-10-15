using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float moveSpeed = 5f; // �������� ����������� ������
    public float zoomSpeed = 2f; // �������� ����
    public float minZoom = 5f;   // ����������� ������� ���� (�����)
    public float maxZoom = 20f;  // ������������ ������� ���� (������)

    void Update()
    {
        // ����������� ������ � ������� ������ WASD
        HandleMovement();

        // ��������������� ������ � ������� ������� ����
        HandleZoom();
    }

    void HandleMovement()
    {
        float moveX = 0f;
        float moveY = 0f;

        // W ��� ������� ����� - �������� �����
        if (Input.GetKey(KeyCode.W))
        {
            moveY = 1f;
        }
        // S ��� ������� ���� - �������� ����
        if (Input.GetKey(KeyCode.S))
        {
            moveY = -1f;
        }
        // A ��� ������� ����� - �������� �����
        if (Input.GetKey(KeyCode.A))
        {
            moveX = -1f;
        }
        // D ��� ������� ������ - �������� ������
        if (Input.GetKey(KeyCode.D))
        {
            moveX = 1f;
        }

        // ����������� ������ � ����������� �� ��������� ��������
        Vector3 moveDirection = new Vector3(moveX, moveY, 0f).normalized;
        transform.position += moveDirection * moveSpeed * Time.deltaTime;
    }

    void HandleZoom()
    {
        // �������� �������� ��������� ������� ����
        float scrollData = Input.GetAxis("Mouse ScrollWheel");

        // ������������ ������, ������� � ��������������� ������ (��� 2D ������)
        Camera.main.orthographicSize -= scrollData * zoomSpeed;
        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, minZoom, maxZoom); // ������������ ���
    }
}
