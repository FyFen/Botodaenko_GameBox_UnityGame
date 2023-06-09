using UnityEngine;
using UnityEngine.UI;

public class OtherLogic : MonoBehaviour
{
    //�� ������� ����� ��������� ������� ������, � ������� �������� ������ ������ �� ��������� ���� � ������

    [Header("���� � �� ����������� ����������� ����������, ��� ������ �������� �� ������ ��������")]
    public GameObject particle_click, pause_window_local;
    public static GameObject pause_window, attack_panel;

    public Image timer_to_update;

    [Header("���������� ��������� � ������������� ����� ��� (�������)")]
    public static bool select_skill_panel, kill_enemy;
    public GameObject skill_on, skill_off, peace_on, peace_off, muve_on, muve_off, notify;
    public Button skill_interactecle, peace_interactecle, muve_interactecle;
    public GameObject skill_lock, peace_lock, muve_lock, peace_eye, muve_eye;

    [Header("���������� ��� ���������� ��������� �������� � ��������� ��������")]
    public Image _hero_life_img, _enemy_life_img, timer_img;
    public GameObject _attack_fone, _attack;
    public static float _enemy_life_max_local, _enemy_life_left;

    void Start()
    {
        //��� ����� �������� �� ����� ���� ������ � ��������� �� �������
        Button[] all_buttons = FindObjectsOfType<Button>();

        foreach (Button button in all_buttons)
            button.onClick.AddListener(() => CreateParticle(button));

        //����� ����� ��������������� ���������
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
    //���������, ��� ���������� ��� ������� ����� ������ (������ �������� ����� �� ��� �� �����, �� ����� ����������� �� �����)
    private void CreateParticle(Button button)
    {
        GameObject particle_new = Instantiate(particle_click, button.transform.position, Quaternion.identity);
        particle_new.transform.SetParent(button.transform, false);
    }
    //�������� � ��������� ��� ��� �������� (������ �� ����� �� ���, �� ������� ��� ����, ����� ����������� � ������ �����)
    public static void PauseManager_Puse()
    {
        pause_window.SetActive(true);
    }
    public static void PauseManager_Play()
    {
        pause_window.SetActive(false);
    }
    //���������� ����������� �������
    private void Uptade_img()
    {
        timer_to_update.fillAmount = 1 - (SkillsSpauner._timer / SkillsSpauner._timer_limit);
    }
    //����� ��� ������� ������� �� ��������� � ����������� ������ ������� (������ ��� �������, ����� ��� �������� � ������ ���� ���� (����������� ������ ��������)
    //���� ������ ����������� �����, ���������, ����� ������� �����
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
    //��������� �������� �����, ����� � ������� ����� �����
    private void AttackSpauner()
    {
        _hero_life_img.fillAmount = PlayerMove._hero_life_left / PlayerMove._hero_life_max;
        _enemy_life_img.fillAmount = _enemy_life_left / _enemy_life_max_local;

        timer_img.fillAmount = 1 - (SkillsSpauner._timer_attack / SkillsSpauner._timer_attack_limit);
    }
    //������ ���������� �� ���� �� ����� � �����
    public static void AttackDamage_toHero()
    {
        PlayerMove._hero_life_left--;
    }
    public static void AttackDamage_toEnemy()
    {
        _enemy_life_left--;
    }
    //������� ��� ���������� ��� (���� �������� ��� �������)
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
