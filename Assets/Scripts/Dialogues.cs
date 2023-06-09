using System;
using UnityEngine;
using UnityEngine.UI;

public class Dialogues : MonoBehaviour
{
    //������������� ������� ������� (������� �� �������, �� � ����� ����� ������� �� ������ ������)
    
    [Header("������ ��������")]
    private String[] training =
    {
        "�����������, ���� ��������� ��� ����������! ������ ��� ��������� � ��� �����, ����� ����-���� ��������.",
        "��� ������. � ������ ����� ������ �� ������ ������������ ����. �������� ���������� - ��������� ��������.\r\n\r\n� ������ ������ �� ������� ��������� �� ��������:",
        "�� ������ �������, �� ����� ��������� ����������������� � ������� �������������:\r\n\r\n\r\n\r\n��������� ������� �� �������� ������������� ������ ������� �����.",
        "���� ����������, ������� ������ ����� �� ������������� � ������ � ����������� �������������� �� ��������:",
        "��������� �������������� ����� �������� � ���� ���������� ������������, � ����������� �� ���� ��������:\r\n\r\n\r\n\r\n\r\n�� ���, �����! ������ �� ������� ��� ������!"
    };
    public GameObject[] training_step;

    [Header("��������� ������ �����")]
    private String[] hero =
    {
        "��... �� ������ ����� ��� ���������� �������. ���� �� ���������. �������� ��������� ����� ����� �����!",
        "��� �� ���... �� ��! ������ ���������� ������ ���� �� ����� ��. ����� ��� ��������, ������ ������������������ �� ������ ����.",
        "������, ��� � ��������� ������������. ����������, ��� ����� ��� ����� ������������ ��������� ���� �� ����������.",
        "����� � �����? �������, �� �����-�� ���������. ����� ��� ����������, �� ������ ������������������ �� �����.",
    };

    [Header("��������������� �������� ��� ������� �������� � ��������")]
    public GameObject dialog_window, training_window;
    public static GameObject dialog_window_local;

    public Text bla_bla_bla, tutor_text;
    public static Text bla_bla_bla_local; 

    public static int switcher, step_tutor;
    public static bool next, stop;


    void Start()
    {
        step_tutor = 0;
        switcher = 0;
        next = false;
        stop = false;

        training_window.SetActive(true);

        dialog_window_local = dialog_window;
        bla_bla_bla_local = bla_bla_bla;

        OtherLogic.PauseManager_Puse();
    }
    private void Update()
    {
        if (next == true)
            FirstDialog();

        if (step_tutor >= training.Length)
        {
            training_window.SetActive(false);
            training_step[step_tutor-1].SetActive(false);

            OtherLogic.PauseManager_Play();
        }
        else
        {
            tutor_text.text = training[step_tutor];

            training_step[step_tutor].SetActive(true);
            if (step_tutor > 0 && training_step[step_tutor - 1].activeSelf == true)
                training_step[step_tutor - 1].SetActive(false);
        }
    }
    //������ ���������� ������� �����
    private void FirstDialog()
    {
        if (switcher >= hero.Length && stop != true)
        {
            dialog_window.SetActive(false);
            stop = true;
        }
        else
        {
            dialog_window.SetActive(true);

            bla_bla_bla.text = hero[switcher];
        }

        next = false;
    }
    //������ ������������ ������ � ��������
    public void NextStep_tutor()
    {
        step_tutor++;

        if (step_tutor >= training.Length)
            next = true;
    }
}
