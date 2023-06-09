using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCLogic : MonoBehaviour
{
    //универсальная логика работы для диалогов, сражений и событий (попытка ее реализации)

    [Header("Укажите типы события для взаимодействия")]
    public string[] type_logic =
    {
        "dialog",
        "attack",
        "logic"
    };

    [Header("Вручную задайте порядковый номер существу, чтобы код понял, что с ним делать")]
    public int number_logic, next_logic;

    [Header("Для активации соответствующего событию элемента интерфейса")]
    public GameObject active_skill, dialog_window, attack_window, bones, attack_notify;

    [Header("Для корректной отработки всплывающей подсказки")]
    private GameObject _player;
    private float _distance_to_player, _radius_collider;

    [Header("Универсальная система диалогов")]
    public int number_chat;
    public string[] dialog_nps;
    public GameObject[] notif_logo;
    public Text bla_bla_bla;

    [Header("Вспомогательные элементы")]
    public bool _start_logic;

    [Header("Для события сражения")]
    public float _enemy_life_max;

    private void Start()
    {
        number_chat = -1;
        _distance_to_player = 0;

        _start_logic = false;

        _player = GameObject.FindGameObjectWithTag("Player");

        CircleCollider2D collider = GetComponent<CircleCollider2D>();
        _radius_collider = collider.radius;

        //воодные показатели здоровья врага
        _enemy_life_max = 5;
    }
    private void FixedUpdate()
    {
        if (_start_logic == true)
            _distance_to_player = Vector2.Distance(transform.position, _player.transform.position);

        ControlLogic();
    }
    //определяет, когда игрок вошел и вышел из зоны взаимодействия с объектом/существом
    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.CompareTag("Player") && Dialogues.dialog_window_local.activeSelf != true && attack_window.activeSelf != true)
        {
            _start_logic = true;
            notif_logo[number_logic].SetActive(true);
        }

        if (col.CompareTag("Player") && OtherLogic.kill_enemy == true)
        {
            KillNPS();
        }

        if (attack_window.activeSelf == true)
            notif_logo[number_logic].SetActive(false);
    }
    private void OnTriggerExit2D(Collider2D col)
    {
        if (_distance_to_player > _radius_collider + 2f)
        {
            notif_logo[number_logic].SetActive(false);

            _start_logic = false;
        }
    }
    //активация логики события диалога или сражения
    private void ControlLogic()
    {
        if (number_logic == 0 && active_skill.transform.childCount == 1 && _start_logic == true)
        {
            if (active_skill.transform.GetChild(0).tag == "chat")
            {
                DialogLogic();
            }
        }
        if (number_logic == 1 && active_skill.transform.childCount == 1 && _start_logic == true)
        {
            if (active_skill.transform.GetChild(0).tag == "attack")
            {
                AttackLogic();
            }
        }
    }
    private void DialogLogic()
    {
        if (Dialogues.dialog_window_local.activeSelf != true && number_chat == -1)
        {
            Dialogues.dialog_window_local.SetActive(true);
            notif_logo[number_logic].SetActive(false);
            number_chat++;
        }
        else if (Dialogues.dialog_window_local.activeSelf && number_chat != dialog_nps.Length)
        {
            number_chat++;

            if (number_chat == dialog_nps.Length)
            {
                Dialogues.dialog_window_local.SetActive(false);
                number_chat = dialog_nps.Length - 1;
                if (next_logic == 0)
                {
                    next_logic = 1;
                    PlayerMove.NextTarget();
                } 
            }
        }
        else if (Dialogues.dialog_window_local.activeSelf != true && number_chat < dialog_nps.Length)
        {
            Dialogues.dialog_window_local.SetActive(true);
        }

        if (number_chat < dialog_nps.Length)
            Dialogues.bla_bla_bla_local.text = dialog_nps[number_chat];
    }
    //логика сражения и победы в ней
    private void AttackLogic()
    {
        SkillsSpauner.skill_type_switcher = 2;
        OtherLogic.select_skill_panel = true;
        OtherLogic._enemy_life_max_local = _enemy_life_max;
        SkillsSpauner.update = true;
    }
    private void KillNPS()
    {
        bones.SetActive(true);
        attack_notify.SetActive(false);
        OtherLogic.kill_enemy = false;
        Destroy(gameObject);
    }
}
