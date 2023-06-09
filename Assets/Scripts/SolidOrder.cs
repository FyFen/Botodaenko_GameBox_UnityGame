using UnityEngine;

public class SolidOrder : MonoBehaviour
{
    //������� ������� �����, �������� �� ������ �������������� � ��������� �� �-����������, ������ ������� �����

    [Header("��������������� ��������")]
    public bool isStatic = false;
    private Renderer render_solid;
    public SpriteRenderer _set_order;

    [Header("���������� ��� ������ ����������, ���� ������� ���� �������� ��������� �����������")]
    public float offset = 0;
    public int sorting_solid = 0;

    [Header("��������� ��� ���������� ������")]
    private float _offset_y_col, _size_y_col, _obj_height;

    //����� ������� ������� ������� � ��������� ���������� ��� ������������ ����������
    private void Awake()
    {
        render_solid = GetComponent<Renderer>();

        BoxCollider2D _collider = GetComponent<BoxCollider2D>();
        _offset_y_col = _collider.offset.y * -1;
        _size_y_col = _collider.size.y / 2f;

        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        _obj_height = spriteRenderer.bounds.size.y / 2f;
    }
    //������ �� ���������� ����, ����������� ������� ���� �������
    private void LateUpdate()
    {
        render_solid.sortingOrder = (int)(sorting_solid - (_obj_height + _offset_y_col - _size_y_col + transform.position.y - _offset_y_col - _size_y_col + offset));
        _set_order.sortingOrder = render_solid.sortingOrder - 10;

        if (isStatic)
        {
            Destroy(this);
        }
    }
}
