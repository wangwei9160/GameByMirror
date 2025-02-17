using UnityEngine;

public class SingleShootState : IState
{
    public override int ID => 5;
    public float ShootBufferTime = 0.2f;
    public float Timer = 0;
    public bool isShoot = false;
    public SingleShootState(MoveController moveController, string[] _animation, float _crossfade):base(moveController , _animation , _crossfade) { }

    public override void OnEnter()
    {
        base.OnEnter();
        isShoot = true;
        moveController.isShoot = true;
        if (animationSize == 1)
        {
            animator.CrossFade(animation[0], crossfade);
        }
        moveController.PlayerInfo.CurrentBullet -= 1;
        EventCenter.Broadcast(EventDefine.OnPlayerInfoChange , moveController.PlayerInfo);
        foreach (ParticleSystem item in moveController.VfXs)
        {
            item.Play();
        }
        Timer = ShootBufferTime;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if(isShoot)
        {
            Timer -= Time.deltaTime;
            if (Timer <= 0f)
            {
                isShoot = false;
            }
        }
    }

    public override void OnExit()
    {
        base.OnExit();
    }

    public override bool OnChange(IState other)
    {
        if(isShoot)
        {
            return false;
        }
        if(other.ID == 0)
        {
            return true;
        }
        return false;
    }
}