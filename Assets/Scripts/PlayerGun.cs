using UnityEngine;

public class ArmFollowMouse : MonoBehaviour
{
    public Transform shoulder; // Точка вращения руки
    public Transform arm; // Объект руки
    public Transform player; // Объект игрока
    public float rotationSpeed = 10f; // Скорость поворота

    void Update()
    {
        // Получение позиции курсора в мировых координатах
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0; // Устанавливаем Z в 0 для 2D

        // Вычисление направления от плеча к курсору
        Vector3 direction = mousePosition - shoulder.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Проверка, нужно ли развернуть игрока
        if ((mousePosition.x < player.position.x && player.localScale.x > 0) || (mousePosition.x > player.position.x && player.localScale.x < 0))
        {
            Flip();
        }

        // Корректируем угол, если игрок развернут
        if (player.localScale.x < 0)
        {
            angle += 180f;
        }

        // Поворот руки вокруг плеча
        Quaternion rotation = Quaternion.Euler(0, 0, angle);
        arm.rotation = Quaternion.Slerp(arm.rotation, rotation, rotationSpeed * Time.deltaTime);
    }

    private void Flip()
    {
        Vector3 scale = player.localScale;
        scale.x *= -1;
        player.localScale = scale;

        // Коррекция позиции игрока при отражении
        Vector3 position = player.position;
        position.x += scale.x * 0.1f; // Настроить смещение при повороте
        player.position = position;
    }
}