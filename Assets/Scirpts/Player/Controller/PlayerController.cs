using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class PlayerController : NetworkBehaviour 
{
    public GameObject[] weapons;
    /// <summary>
    /// SyncVar : ���ڱ����Ҫͬ�������пͻ��˵ı���
    /// ͬ�����ƣ���һ��syncvarֵ�ڷ������ϸ���ʱ��mirror���Զ����������ͬ�������еĿͻ���
    /// hook����syncvar�ڿͻ�������Ҫ����ʱ��Ҫ���õķ�������ͬ������ʱִ���ض����߼�������ui���ߴ�������
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
    /// Command �� �����Ҫ�ڷ�������ִ�еķ������ɿͻ��˵��ã�ʵ��ִ���߼��ڷ������ϡ�
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