using System;
using UnityEngine;
using UnityEngine.UI;

public class Dialogues : MonoBehaviour
{
    //универсальная система диалога (попытка ее сделать, но в итоге часть перешла на другой скрипт)
    
    [Header("Диалог обучения")]
    private String[] training =
    {
        "Приветствую, юный волшебник или волшебница! Прежде чем окунуться в мир магии, давай чуть-чуть поучимся.",
        "Все просто. В нижней части экрана ты видишь мыслительные руны. Соединяй одинаковые - активируй действие.\r\n\r\nВ нужный момент ты увидишь подсказку по действию:",
        "Ты можешь выбрать, на каких действиях концентрироваться с помощью переключателя:\r\n\r\n\r\n\r\nПовторное нажатие на активный переключатель просто обновит мысли.",
        "Будь внимателен, порядок мыслей долго не задерживаются в голове и обновляются самостоятельно со временем:",
        "Некоторые активированные мысли попадают в зону предельной концентрации, в зависимости от типа действий:\r\n\r\n\r\n\r\n\r\nНу все, удачи! Следуй по стрелке под ногами!"
    };
    public GameObject[] training_step;

    [Header("Стартовый диалог героя")]
    private String[] hero =
    {
        "Ох... Не стоило вчера так увлекаться зельями. Ноги не слушаются. Придется управлять телом силой магии!",
        "Как же это... Ах да! Просто перетащить нужную руну на такую же. Мысли еще путаются, сложно сконцентрироваться на нужной руне.",
        "Хорошо, что я установил концентратор. Показывает, как долго еще будут существовать мысленные руны до обновления.",
        "Зачем я здесь? Кажется, по чьему-то поручению. Стоит тут оглядеться, но прежде сконцентрироваться на ногах.",
    };

    [Header("Вспомогательные элементы для системы диалогов и обучения")]
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
    //логика стартового диалога героя
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
    //логика переключения текста в обучении
    public void NextStep_tutor()
    {
        step_tutor++;

        if (step_tutor >= training.Length)
            next = true;
    }
}
