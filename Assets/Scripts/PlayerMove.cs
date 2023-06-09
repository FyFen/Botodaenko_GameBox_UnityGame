using UnityEngine;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviour
{
    //движение игрока и часть логики, которая с ним связана

    [Header("Основные параметры игрока")]
    public static Vector2 _direction;
    public static Rigidbody2D _player;
    private Animator _anim;

    [Header("Камера и дополнительные переменные для финального текста")]
    public GameObject main_camera, dialog;
    public Text dialog_txt;
    private string final_txt;

    [Header("Направляющая стрелка на точки интереса")]
    public GameObject arrow_pointer;
    public GameObject parent;
    public GameObject[] target;
    private Vector3 target_position;
    private float angle_to_target;
    public static int number_of_target;

    [Header("Здоровье игока для сражения")]
    public static float _hero_life_max, _hero_life_left, _max_target;

    private void Start()
    {
        _player = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();

        _hero_life_max = 10;
        _hero_life_left = _hero_life_max;

        _max_target = target.Length;

        final_txt = "Все прекрасное рано или поздно заканчивается. Чтобы подолжить игру, купите сезонный абонимент! #Упс! Недоступно в вашем регионе!";
    }
    private void Update()
    {
        movement();

        if (number_of_target < target.Length)
            LookAt_Target();
        else if (number_of_target >= target.Length)
        {
            dialog.SetActive(true);
            dialog_txt.text = final_txt;
            arrow_pointer.SetActive(false);
        }

        main_camera.transform.position = new Vector2(transform.position.x, transform.position.y - 1.27f);
    }
    //вспомогательная логика движения (непосредственно движение задают руны при активации в другом скрипте)
    private void movement()
    {
        if (_direction.x == 0 && _direction.y == 0)
        {
            _player.velocity = Vector2.zero;
            _player.constraints |= RigidbodyConstraints2D.FreezePositionX;
            _player.constraints |= RigidbodyConstraints2D.FreezePositionY;
            _player.constraints |= RigidbodyConstraints2D.FreezeRotation;
        }

        if (_direction.magnitude > 0)
        {
            _anim.SetBool("if_run", true);

            if (_direction.x == 1)
                transform.localScale = new Vector3(3, 3, 3);
            else if (_direction.x == -1)
                transform.localScale = new Vector3(-3, 3, 3);
        }
            
        else
            _anim.SetBool("if_run", false);
    }
    //логика направляющей стрелки, смотрит просто по заранее назначенному порядку точек
    private void LookAt_Target()
    {
        arrow_pointer.transform.position = parent.transform.position;
        
        target_position = target[number_of_target].transform.position - arrow_pointer.transform.position;
        angle_to_target = Mathf.Atan2(target_position.y-1.5f, target_position.x) * Mathf.Rad2Deg;
        arrow_pointer.transform.rotation = Quaternion.AngleAxis(angle_to_target, Vector3.forward);
    }
    public static void NextTarget()
    {
        if (number_of_target < _max_target)
            number_of_target++;
    }
}
