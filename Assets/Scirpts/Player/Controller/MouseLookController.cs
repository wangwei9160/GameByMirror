using Mirror;
using UnityEngine;

/// <summary>
/// 鼠标控制镜头的上下，角色的左右转向
/// </summary>
public class MouseLookController : NetworkBehaviour
{
    // tooltip 鼠标停留的提示信息
    [Tooltip("鼠标灵敏度")]public float mouseSensitivity = 200f; // 鼠标灵敏度
    public Transform player;
    private float yRotation = 0f;
    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        player = transform.GetComponentInParent<MoveController>().transform;
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

        transform.localRotation = Quaternion.Euler(yRotation, 0, 0);
        player.Rotate(Vector3.up , mouseX);
    }

}