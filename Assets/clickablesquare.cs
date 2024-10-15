using UnityEngine;

public class ClickableSquare : MonoBehaviour
{
    public int x; // ���������� �� ��� X
    public int y; // ���������� �� ��� Y
    private FieldGenerator fieldGenerator; // ������ �� FieldGenerator

    // ����� ��� ������������� ��������� � ������ �� FieldGenerator
    public void Initialize(int x, int y, FieldGenerator fieldGenerator)
    {
        this.x = x;
        this.y = y;
        this.fieldGenerator = fieldGenerator;
    }

    void OnMouseDown()
    {
        // �������� ����� OnClick � FieldGenerator ��� �������
        if (fieldGenerator != null)
        {
            fieldGenerator.OnClick(x, y); // �������� ���������� �������
        }
    }
}
