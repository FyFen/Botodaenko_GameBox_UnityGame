using UnityEngine;
using UnityEngine.UI;

public class ActiveSkill : MonoBehaviour
{
    //универсальный скрипт для отработки всех типов рун (также не идеально реализовано, чать логики ушла в другие скрипты)

    [Header("Список всех тегов для обнаружения всех типов рун")]
    public string[] tags;

    [Header("Вспомогательные переменные для отчета активного состояния конкретных рун")]
    private bool _is_active_in;
    private float _timer, _time_left, _speed;
    public GameObject timer_main_obj, timer_skill_obj;
    private Image timer_img_main;

    void Start()
    {
        _time_left = 1.2f;
        _speed = 2.5f;

        timer_img_main = timer_main_obj.GetComponent<Image>();

        timer_main_obj.SetActive(false);
    }
    void FixedUpdate()
    {
        IsActiveSkill();
    }
    //обработка столкновений рун и выбор действия исходя из столкновения
    private void IsActiveSkill()
    {
        _is_active_in = transform.childCount == 1;

        if (_is_active_in)
        {
            //условия для мирных действий
            if (transform.GetChild(0).tag == "chat")
                chat_skill();
            if (transform.GetChild(0).tag == "attack")
                Destroy_Skill();

            //условия для действий движения
            if (transform.GetChild(0).tag == "up")
                up_skill();
            if (transform.GetChild(0).tag == "down")
                down_skill();
            if (transform.GetChild(0).tag == "right")
                right_skill();
            if (transform.GetChild(0).tag == "left")
                left_skill();

            timer_img_main.fillAmount = 1 - (_timer / _time_left);
        }
        else
        {
            PlayerMove._direction = new Vector2(0, 0);
        }
    }

    //мирные действия
    private void chat_skill()
    {
        if (Dialogues.stop != true)
        {
            Dialogues.switcher++;
            Dialogues.next = true;
        }

        Destroy_Skill();
    }

    //базовые действия передвижения рунами
    private void up_skill()
    {
        _timer += Time.fixedDeltaTime;
        timer_main_obj.SetActive(true);

        if (_timer >= _time_left)
        {
            Destroy_Skill();
        }

        PlayerMove._player.constraints &= RigidbodyConstraints2D.FreezePositionX;
        PlayerMove._player.constraints &= RigidbodyConstraints2D.FreezePositionY;
        PlayerMove._player.constraints |= RigidbodyConstraints2D.FreezeRotation;

        PlayerMove._direction = new Vector2(0, _speed);
        PlayerMove._player.velocity = PlayerMove._direction;
    }
    private void down_skill()
    {
        _timer += Time.fixedDeltaTime;
        timer_main_obj.SetActive(true);

        if (_timer >= _time_left)
        {
            Destroy_Skill();
        }

        PlayerMove._player.constraints &= RigidbodyConstraints2D.FreezePositionX;
        PlayerMove._player.constraints &= RigidbodyConstraints2D.FreezePositionY;
        PlayerMove._player.constraints |= RigidbodyConstraints2D.FreezeRotation;

        PlayerMove._direction = new Vector2(0, -_speed);
        PlayerMove._player.velocity = PlayerMove._direction;
    }
    private void right_skill()
    {
        _timer += Time.fixedDeltaTime;
        timer_main_obj.SetActive(true);

        if (_timer >= _time_left)
        {
            Destroy_Skill();
        }

        PlayerMove._player.transform.localScale = new Vector3(3, 3, 3);
        PlayerMove._player.constraints &= RigidbodyConstraints2D.FreezePositionX;
        PlayerMove._player.constraints &= RigidbodyConstraints2D.FreezePositionY;
        PlayerMove._player.constraints |= RigidbodyConstraints2D.FreezeRotation;

        PlayerMove._direction = new Vector2(_speed, 0);
        PlayerMove._player.velocity = PlayerMove._direction;
    }
    private void left_skill()
    {
        _timer += Time.fixedDeltaTime;
        timer_main_obj.SetActive(true);

        if (_timer >= _time_left)
        {
            Destroy_Skill();
        }

        PlayerMove._player.transform.localScale = new Vector3(-3, 3, 3);
        PlayerMove._player.constraints &= RigidbodyConstraints2D.FreezePositionX;
        PlayerMove._player.constraints &= RigidbodyConstraints2D.FreezePositionY;
        PlayerMove._player.constraints |= RigidbodyConstraints2D.FreezeRotation;

        PlayerMove._direction = new Vector2(-_speed, 0);
        PlayerMove._player.velocity = PlayerMove._direction;
    }

    //таймер уничтожения скиллов
    private void Destroy_Skill()
    {
        timer_main_obj.SetActive(false);

        Transform child_obj = transform.GetChild(0);
        Destroy(child_obj.gameObject);
        _timer = 0;
    }
}
