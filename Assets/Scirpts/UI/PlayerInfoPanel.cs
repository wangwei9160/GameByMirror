using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfoPanel : MonoBehaviour 
{
    public Text Health;
    public Text Armor;
    public Text CurrentBullet;
    public Text MoreBullet;
    public Text FireMode;
    public Queue<Action> queue = new Queue<Action>();

    void Awake()
    {
        EventCenter.AddListener<PlayerInfo>(EventDefine.OnPlayerInfoChange, OnPlayerInfoChange);
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        EventCenter.RemoveListener<PlayerInfo>(EventDefine.OnPlayerInfoChange, OnPlayerInfoChange);
    }

    public void OnPlayerInfoChange(PlayerInfo info)
    {
        gameObject.SetActive(true);
        Debug.Log("OnPlayerInfoChange" + JsonUtility.ToJson(info));
        Health.text = info.Health.ToString();
        if(info.Armor > 0)
        {
            Armor.gameObject.SetActive(true);
            Armor.text = info.Armor.ToString();
        }else
        {
            Armor.gameObject.SetActive(false);
        }
        
        CurrentBullet.text = info.CurrentBullet.ToString();
        MoreBullet.text = info.MoreBullet.ToString();
        FireMode.text = string.Format("{0}·¢" , info.FireMode == 0 ? "µ¥" : "Á¬");
    }

}