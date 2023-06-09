using UnityEngine;

public class ButtonSetActive : MonoBehaviour
{
    //��������� ������ ��� ������������ ����� ����� (����� ������)

    [Header("������ � �� �������, ������ ����� �������������� �� ������")]
    public GameObject self_on, self_off, other1_on, other1_off, other2_on, other2_off;

    //������ ������� ������, � ����� ��������� ���������� ��� �� ���� �� �������
    public void Switcher()
    {
        if (OtherLogic.pause_window.activeSelf != true)
        {
            self_on.SetActive(true);
            self_off.SetActive(false);

            other1_on.SetActive(false);
            other1_off.SetActive(true);

            other2_on.SetActive(false);
            other2_off.SetActive(true);

            SkillsSpauner.update = true;
        }
    }
    
}
