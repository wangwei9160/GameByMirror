using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class PlayerController : NetworkBehaviour 
{
    public GameObject[] weapons;
    /// <summary>
    /// SyncVar : 用于标记需要同步给所有客户端的变量
    /// 同步机制：当一个syncvar值在服务器上更改时，mirror会自动将这个更改同步给所有的客户端
    /// hook：当syncvar在客户端上需要更改时需要调用的方法，在同步变量时执行特定的逻辑，更新ui或者触发动画
    /// </summary>
    [SyncVar(hook = nameof(OnWeaponChanged))]
    public int activeWeaponSynced = 0;

    void OnWeaponChanged(int _old, int _new)
    {
        if (0 <= _old && _old < weapons.Length)
        {
            weapons[_old].SetActive(false);
        }

        if (0 <= _new && _new < weapons.Length)
        {
            weapons[_new].SetActive(true);
            activeWeaponSynced = _new;
        }
    }

    /// <summary>
    /// Command ： 标记需要在服务器上执行的方法，由客户端调用，实际执行逻辑在服务器上。
    /// </summary>
    [Command]
    public void CmdChangeActiveWeapon(int idx)
    {
        activeWeaponSynced = idx;
    }

    private void Awake()
    {
        for (int i = 0; i < weapons.Length; i++)
        {
            weapons[i].SetActive(i == activeWeaponSynced);
        }
    }

    

    void Update()
    {
        if (!isLocalPlayer) { return; }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            CmdChangeActiveWeapon(0);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            CmdChangeActiveWeapon(1);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            CmdChangeActiveWeapon(2);
        }
    }

    


}