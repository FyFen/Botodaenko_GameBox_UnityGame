using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

public class SkillsSpauner : MonoBehaviour
{
    //рандомный спаунер рун, обновл€ет доску автоматически и по услови€м, считывает руны какого типа нужно спаунить

    [Header("ѕозици€ €чеек в которых нужно обновл€ть руны")]
    public GameObject[] position_cell;

    [Header("“аймеры и переменны св€занные с обновлением доски")]
    public static float _timer, _timer_limit, _timer_attack, _timer_attack_limit;
    public static bool update, attack_spaun;

    [Header("–уны под каждый тип")]
    public GameObject[] logic_image;
    public GameObject[] attack_hero_image;
    public GameObject[] attack_enemy_image;
    public GameObject[] peace_image;
    public GameObject[] muve_image;

    [Header("“ипы рун, от активного типа зависит набор рун")]
    public GameObject is_skill_active, is_peace_active, is_muve_active, outer;

    [Header("¬спомогательные элементы")]
    public Image selector_main;
    private int _rnd, _num_i;
    private bool _hierEmpty;
    public static float skill_type_switcher;

    private void Start()
    {
        Spaun_skill();

        attack_spaun = false;
        update = false;
        _timer_limit = 5;
        _timer_attack_limit = 5;
    }
    private void FixedUpdate()
    {
        Spaun_skill();
        Update_Spaun();

        if (is_skill_active.activeSelf)
        {
            SpaunAttack();
        }
    }
    private void Spaun_skill()
    {
        for (int i = 0; i < position_cell.Length; i++)
        {
            _hierEmpty = position_cell[i].transform.childCount == 0;

            if (_hierEmpty)
            {
                _num_i = i;
                WhatIsActive();
            }
        }
    }
    //обновление достки рандомными рунами по таймеру
    private void Update_Spaun()
    {
        _timer += Time.fixedDeltaTime;

        if (_timer >= _timer_limit || update == true)
        {
            update = false;
            
            for (int i = 0; i < position_cell.Length; i++)
            {
                while (position_cell[i].transform.childCount > 0)
                {
                    Transform child_obj = position_cell[i].transform.GetChild(0);
                    DestroyImmediate(child_obj.gameObject);
                }
            }

            _timer = 0;
        }
    }
    //спайн по таймеру атак врага
    private void SpaunAttack()
    {
        _timer_attack += Time.fixedDeltaTime;

        if (_timer_attack >= _timer_attack_limit)
        {
            Random rnd_cell = new Random();
            _rnd = rnd_cell.Next(0, attack_enemy_image.Length);

            Instantiate(attack_enemy_image[_rnd], outer.transform);

            BoxCollider2D _collider = attack_enemy_image[_rnd].GetComponent<BoxCollider2D>();
            _collider.enabled = true;

            _timer_attack = 0;
        }
    }
    //проверка, какой тип рун активен
    private void WhatIsActive()
    {
        if (is_skill_active.activeSelf)
        {
            if (skill_type_switcher == 1)
            {
                Random rnd_cell = new Random();
                _rnd = rnd_cell.Next(0, logic_image.Length);

                Instantiate(logic_image[_rnd], position_cell[_num_i].transform);
            }
            else if (skill_type_switcher == 2)
            {
                Random rnd_cell = new Random();
                _rnd = rnd_cell.Next(0, attack_hero_image.Length);

                Instantiate(attack_hero_image[_rnd], position_cell[_num_i].transform);
            }
        }
        if (is_peace_active.activeSelf)
        {
            Random rnd_cell = new Random();
            _rnd = rnd_cell.Next(0, peace_image.Length);

            Instantiate(peace_image[_rnd], position_cell[_num_i].transform);
        }
        if (is_muve_active.activeSelf)
        {
            Random rnd_cell = new Random();
            _rnd = rnd_cell.Next(0, muve_image.Length);

            Instantiate(muve_image[_rnd], position_cell[_num_i].transform);
        }
    }
}
