using UnityEngine;
using UnityEngine.EventSystems;

public class SkillsLogic : MonoBehaviour, IPointerEnterHandler, IPointerUpHandler, IPointerDownHandler, IPointerExitHandler
{
    //Данный скрипт применяется отдельно к каждой спосоности (мердж-рунам)

    [Header("Укажите элементы, которые будут реагировать на выделение мышкой")]
    public GameObject off, on;
    [Header("Для сортировки изображения взятых в руку рун поверх всех остальных")]
    public GameObject img;
    [Header("Эффект взрыва от мерджа")]
    public GameObject particle;

    [Header("Массивы для расположения рун и сортировки активных")]
    private GameObject[] _position_active_cell, _aktive_skill, _match_skills;

    [Header("Вход и выход для рун в режиме сражения")]
    private GameObject _inner, _outer;

    [Header("Вспомогательные параметры")]
    private bool _is_down, _all_is_active;
    private SpriteRenderer _spriteRend, _spriteRend_img;
    private int _spriteRend_orig, _spriteRend_orig_img, _num_active;
    private string _tag;

    private void Start()
    {
        off.SetActive(true);
        _is_down = false;
        _all_is_active = false;

        _spriteRend = GetComponent<SpriteRenderer>();
        _spriteRend_orig = _spriteRend.sortingOrder;
        _spriteRend_img = img.GetComponent<SpriteRenderer>();
        _spriteRend_orig_img = _spriteRend_img.sortingOrder;

        _position_active_cell = GameObject.FindGameObjectsWithTag("activeskill");
        _inner = GameObject.FindGameObjectWithTag("destroer");
        _outer = GameObject.FindGameObjectWithTag("outer");
    }
    private void Update()
    {
        IsActiveSkill();
    }
    private void FixedUpdate()
    {
        if (_is_down == true && OtherLogic.pause_window.activeSelf != true)
            GoToMouse_Logic();
        else if (_is_down != true)
            Move_AktiveSkill();
    }
    //нажатие кнопки запускает методы
    public void OnPointerDown(PointerEventData eventData)
    {
        _is_down = true;
        ActivateCollider();
    }
    //определяет, когда мышка наведена на кнопку
    public void OnPointerEnter(PointerEventData eventData)
    {
        off.SetActive(false);
        on.SetActive(true);
    }
    //определяет, когда мышка вышла за пределы кнопки
    public void OnPointerExit(PointerEventData eventData)
    {
        off.SetActive(true);
        on.SetActive(false);
    }
    //определяет, когда отжата кнопка
    public void OnPointerUp(PointerEventData eventData)
    {
        _is_down = false;

        EventSystem.current.SetSelectedGameObject(null);

        off.SetActive(true);
        on.SetActive(false);

        transform.localPosition = Vector2.zero;

        _spriteRend.sortingOrder = _spriteRend_orig;
        _spriteRend_img.sortingOrder = _spriteRend_orig_img;
    }
    //заставляет кнопку двигаться за курсором
    public void GoToMouse_Logic()
    {
        Vector2 _cursor_pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector2(_cursor_pos.x, _cursor_pos.y);

        _spriteRend.sortingOrder = 15;
        _spriteRend_img.sortingOrder = 16;
    }
    //включает коллайдеры всех одинаковых кнопок
    public void ActivateCollider()
    {
        _tag = transform.gameObject.tag;
        _match_skills = GameObject.FindGameObjectsWithTag(_tag);

        foreach (GameObject obj in _match_skills)
        {
            BoxCollider2D _collider = obj.GetComponent<BoxCollider2D>();
            _collider.enabled = true;
        }
    }
    //считывает столкновение одинаковых кнопок (когда они мерджатся)
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == gameObject.tag && OtherLogic.attack_panel.activeSelf == true)
        {
            if (_outer.transform.childCount != 0)
            {
                GameObject new_particle = Instantiate(particle, col.transform.position, Quaternion.identity);
                new_particle.transform.SetParent(col.transform.parent.parent, false);

                if (_is_down == false)
                {
                    if (_outer.transform.GetChild(0).tag == col.gameObject.tag)
                    {
                        GameObject attack_obj = _outer.transform.GetChild(0).gameObject;
                        Destroy(attack_obj);
                        OtherLogic.AttackDamage_toEnemy();
                    }
                    Destroy(gameObject);
                    Destroy(col.gameObject);
                }

                if (_outer.transform.childCount != 0)
                    if (_outer.transform.GetChild(0).tag == col.gameObject.tag)
                        AktivateAttack_Skill();
            }
            else if (_outer.transform.childCount != 0)
            {
                if (_outer.transform.GetChild(0).tag == col.gameObject.tag)
                {
                    OtherLogic.AttackDamage_toEnemy();
                    Destroy(col.gameObject);
                }
            }
        }
        else if (col.gameObject.CompareTag("destroer"))
        {
            Destroy(gameObject);
            OtherLogic.AttackDamage_toHero();
        }
        else if (col.gameObject.tag == gameObject.tag && _all_is_active != true && OtherLogic.attack_panel.activeSelf != true)
        {
            GameObject new_particle = Instantiate(particle, col.transform.position, Quaternion.identity);
            new_particle.transform.SetParent(col.transform.parent.parent, false);

            if (_is_down == false)
            {
                Destroy(col.gameObject);
            }  

            AktivateSkill();
        }
    }
    //логика режима сражения
    private void AktivateAttack_Skill()
    {
        transform.SetParent(_inner.transform);
        transform.localPosition = Vector2.zero;
        transform.localScale = new Vector2(100, 100);

        BoxCollider2D _collider = transform.GetComponent<BoxCollider2D>();
        _collider.offset = new Vector2(0, 130); 
        _collider.enabled = true;

        off.SetActive(false);
    }
    private void AktivateSkill()
    {
        if (_aktive_skill.Length - 3 != 0)
            _num_active = _aktive_skill.Length - 3;
        else
            _num_active = 0;

        transform.SetParent(_position_active_cell[_num_active].transform);
        transform.localPosition = Vector2.zero;
        transform.GetChild(0).gameObject.tag = "activeskill";

        off.SetActive(false);
    }
    //логика перемещения смердженных рун
    private void IsActiveSkill()
    {
        _aktive_skill = GameObject.FindGameObjectsWithTag("activeskill");

        if (_aktive_skill.Length - 3 == 2)
            _all_is_active = true;
        else if (_aktive_skill.Length - 3 <= 1)
            _all_is_active = false;
    }
    private void Move_AktiveSkill()
    {
        if (_num_active == 1 && _position_active_cell[0].transform.childCount == 0 && transform.GetChild(0).tag == "activeskill")
        {
            transform.SetParent(_position_active_cell[_num_active - 1].transform);
            transform.localPosition = Vector2.zero;
            _num_active = 0;
        }
    }
}
