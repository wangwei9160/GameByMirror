
using System;

[Serializable]
public class PlayerInfo
{
    public string Name;
    public int Health;
    public int Armor;
    public int CurrentBullet;
    public int MoreBullet;
    public int FireMode;

    public PlayerInfo()
    {
        Health = 100;
        Armor = 0;
        CurrentBullet = 100;
        MoreBullet = 100;
        FireMode = 0;
    }

    public PlayerInfo(string name, int health, int armor, int currentBullet, int moreBullet, int fireMode)
    {
        Name = name;
        Health = health;
        Armor = armor;
        CurrentBullet = currentBullet;
        MoreBullet = moreBullet;
        FireMode = fireMode;
    }
}