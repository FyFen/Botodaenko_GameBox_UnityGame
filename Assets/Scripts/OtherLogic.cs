using UnityEngine;
using UnityEngine.UI;

public class OtherLogic : MonoBehaviour
{
    //по большей части получился пдлохой скрипт, в котором намешаны разные логики не связанные друг с другом

    [Header("Окна и их дублирующие статические переменные, для вызова статусов из других скриптов")]
    public GameObject particle_click, pause_window_local;
    public static GameObject pause_window, attack_panel;

    public Image timer_to_update;

    [Header("Переменные связанные с переключением типов рун (скиллов)")]
    public static bool select_skill_panel, kill_enemy;
    public GameObject skill_on, skill_off, peace_on, peace_off, muve_on, muve_off, notify;
    public Button skill_interactecle, peace_interactecle, muve_interactecle;
    public GameObject skill_lock, peace_lock, muve_lock, peace_eye, muve_eye;

    [Header("Переменные для корректной отрисовки таймеров и изменения здоровья")]
    public Image _hero_life_img, _enemy_life_img, timer_img;
    public GameObject _attack_fone, _attack;
    public static float _enemy_life_max_local, _enemy_life_left;

    void Start()
    {
        //эта часть отвечает за поиск всех кнопок и считывает их нажатие
        Button[] all_buttons = FindObjectsOfType<Button>();

        foreach (Button button in all_buttons)
            button.onClick.AddListener(() => CreateParticle(button));

        //далее вызов вспомогательных элементов
        pause_window = pause_window_local;
        attack_panel = _attack;

        skill_interactecle = skill_interactecle.GetComponent<Button>();
        peace_interactecle = peace_interactecle.GetComponent<Button>();
        muve_interactecle = muve_interactecle.GetComponent<Button>();

        _hero_life_img = _hero_life_img.GetComponent<Image>();
        _enemy_life_img = _enemy_life_img.GetComponent<Image>();
        timer_img = timer_img.GetComponent<Image>();

        select_skill_panel = false;
        kill_enemy = false;

        _enemy_life_max_local = 5;
        _enemy_life_left = _enemy_life_max_local;
    }
    private void FixedUpdate()
    {
        Uptade_img();

        if (select_skill_panel)
            ActivateSkillPanel();

        if (_attack.activeSelf)
        {
            AttackSpauner();
            EndAttack();
        }
    }
    //описывает, что происходит при нажатии любой кнопки (правда работает вроде ка кне со всеми, не успел разобарться до конца)
    private void CreateParticle(Button button)
    {
        GameObject particle_new = Instantiate(particle_click, button.transform.position, Quaternion.identity);
        particle_new.transform.SetParent(button.transform, false);
    }
    //включает и выключает фон при обучении (логику не довел до ума, но оставил как есть, пауза реализована в другом месте)
    public static void PauseManager_Puse()
    {
        pause_window.SetActive(true);
    }
    public static void PauseManager_Play()
    {
        pause_window.SetActive(false);
    }
    //визуальное отображение таймера
    private void Uptade_img()
    {
        timer_to_update.fillAmount = 1 - (SkillsSpauner._timer / SkillsSpauner._timer_limit);
    }
    //далее два события отечают за активацию и даективацию панели скиллов (служит для событий, таких как сражение и всякие мини игры (реализовано только сражение)
    //сами методы реализоваты плохо, торопился, пошел простым путем
    private void ActivateSkillPanel()
    {
        _attack_fone.SetActive(true);
        _attack.SetActive(true);

        skill_on.SetActive(true);
        skill_off.SetActive(false);
        skill_interactecle.interactable = true;

        peace_on.SetActive(false);
        peace_off.SetActive(true);
        peace_interactecle.interactable = false;
        peace_lock.SetActive(true);
        peace_eye.SetActive(false);

        muve_on.SetActive(false);
        muve_off.SetActive(true);
        muve_interactecle.interactable = false;
        muve_lock.SetActive(true);
        muve_eye.SetActive(false);

        select_skill_panel = false;
    }
    private void OffSkillPanel()
    {
        _attack_fone.SetActive(false);
        _attack.SetActive(false);

        skill_on.SetActive(false);
        skill_off.SetActive(true);
        skill_interactecle.interactable = false;

        peace_on.SetActive(false);
        peace_off.SetActive(true);
        peace_interactecle.interactable = true;
        peace_lock.SetActive(false);
        peace_eye.SetActive(true);

        muve_on.SetActive(true);
        muve_off.SetActive(false);
        muve_interactecle.interactable = true;
        muve_lock.SetActive(false);
        muve_eye.SetActive(false);

        select_skill_panel = false;

        PlayerMove._hero_life_left = PlayerMove._hero_life_max;
        _enemy_life_left = _enemy_life_max_local;
    }
    //отрисовка здоровья врага, героя и таймера новой атаки
    private void AttackSpauner()
    {
        _hero_life_img.fillAmount = PlayerMove._hero_life_left / PlayerMove._hero_life_max;
        _enemy_life_img.fillAmount = _enemy_life_left / _enemy_life_max_local;

        timer_img.fillAmount = 1 - (SkillsSpauner._timer_attack / SkillsSpauner._timer_attack_limit);
    }
    //методы отвечающие за урон по герои и врагу
    public static void AttackDamage_toHero()
    {
        PlayerMove._hero_life_left--;
    }
    public static void AttackDamage_toEnemy()
    {
        _enemy_life_left--;
    }
    //событие при завершении боя (если проиграл или победил)
    private void EndAttack()
    {
        if (PlayerMove._hero_life_left == 0)
        {
            OffSkillPanel();
        }
        if (_enemy_life_left == 0)
        {
            OffSkillPanel();
            kill_enemy = true;
            PlayerMove.NextTarget();
        }
    }
}
