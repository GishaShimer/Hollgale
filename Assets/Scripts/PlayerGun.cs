using UnityEngine;

public class ArmFollowMouse : MonoBehaviour
{
    public Transform shoulder; // ����� �������� ����
    public Transform arm; // ������ ����
    public Transform player; // ������ ������
    public float rotationSpeed = 10f; // �������� ��������

    void Update()
    {
        // ��������� ������� ������� � ������� �����������
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0; // ������������� Z � 0 ��� 2D

        // ���������� ����������� �� ����� � �������
        Vector3 direction = mousePosition - shoulder.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // ��������, ����� �� ���������� ������
        if ((mousePosition.x < player.position.x && player.localScale.x > 0) || (mousePosition.x > player.position.x && player.localScale.x < 0))
        {
            Flip();
        }

        // ������������ ����, ���� ����� ���������
        if (player.localScale.x < 0)
        {
            angle += 180f;
        }

        // ������� ���� ������ �����
        Quaternion rotation = Quaternion.Euler(0, 0, angle);
        arm.rotation = Quaternion.Slerp(arm.rotation, rotation, rotationSpeed * Time.deltaTime);
    }

    private void Flip()
    {
        Vector3 scale = player.localScale;
        scale.x *= -1;
        player.localScale = scale;

        // ��������� ������� ������ ��� ���������
        Vector3 position = player.position;
        position.x += scale.x * 0.1f; // ��������� �������� ��� ��������
        player.position = position;
    }
}