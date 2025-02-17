using Mirror;
using UnityEngine;

/// <summary>
/// �����ƾ�ͷ�����£���ɫ������ת��
/// </summary>
public class MouseLookController : NetworkBehaviour
{
    // tooltip ���ͣ������ʾ��Ϣ
    [Tooltip("���������")]public float mouseSensitivity = 200f; // ���������
    public Transform player;
    public Transform bullet;
    private float yRotation = 0f;
    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        player = transform.GetComponentInParent<MoveController>().transform;
        bullet = player.Find("MuzzleFlash/Bullet");
    }

    public override void OnStartLocalPlayer()
    {
        Camera.main.transform.SetParent(transform);
        Camera.main.transform.localPosition = new Vector3(0, 3f, -3f);
        //Camera.main.transform.Rotate(0, 90, 0);
    }


    private void Update()
    {
        if (!isLocalPlayer) return;

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        yRotation -= mouseY;
        yRotation = Mathf.Clamp(yRotation, -60f, 60f);

        transform.localRotation = Quaternion.Euler(yRotation, -10f, 0);
        Vector3 mousePosition = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            Vector3 target = hit.point;
            Vector3 direction = target - bullet.transform.position;
            direction.Normalize();

            //Debug.Log("��Ұ������������" + target);
            Quaternion rotation = Quaternion.LookRotation(direction , Vector3.up);
            bullet.rotation = rotation;
        }
        
        player.Rotate(Vector3.up , mouseX);
    }

}