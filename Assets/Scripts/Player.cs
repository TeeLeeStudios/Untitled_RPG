using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //Singleton instance of this class
    public Player player;

    //Adjustable settings for survival
    public float SecondsPerHunger = 6;
    public float SecondsPerThirst = 3;
    public float DamagePerTick = 2;
    
    //Possible states that our player may be in.
    private readonly string[] AcceptableStates = {"idle","sitting","laying","prone","walking","running","swimming","mounted","unconcious","revival","dead" };

    //Timers to adjust thirst and hunger
    private float ThirstTimer = 0;
    private float HungerTimer = 0;

    //Called before start and should be used only for initialization
    void Awake()
    {
        MaxHP = 100;
        CurrentHP = 100;
        MaxMP = 100;
        CurrentMP = 100;
        MaxStamina = 100;
        CurrentStamina = 100;
        Hunger = 100;
        Thirst = 100;
        State = "idle";
        player = this;
    }

    //Called once per frame
    void Update()
    {
        //If we are able to set the next timer then take some thirst and hunger
        if (SetTimer(ThirstTimer, SecondsPerThirst))
        {
            TakeThirst(player);
        }
        if (SetTimer(HungerTimer, SecondsPerHunger))
        {
            TakeHunger(player);
        }

    }

    #region Properties
    float CurrentHP
    {
        get
        { return CurrentHP; }

        set
        { CurrentHP = value; }
    }

    float MaxHP
    {
        get
        { return MaxHP; }
        set
        { MaxHP = value; }
    }

    float CurrentMP
    {
        get
        { return CurrentMP; }
        set
        { CurrentMP = value; }
    }

    float MaxMP
    {
        get
        { return MaxMP; }
        set
        { MaxMP = value; }
    }

    float CurrentStamina
    {
        get
        { return CurrentStamina; }
        set
        { CurrentStamina = value; }
    }

    float MaxStamina
    {
        get
        { return MaxStamina; }
        set
        { MaxStamina = value; }
    }

    float Hunger
    {
        get
        { return Hunger; }
        set
        { Hunger = value; }
    }

    float Thirst
    {
        get
        { return Thirst; }
        set
        { Thirst = value; }
    }

    string State
    {
        get
        {
            return State;
        }
        set
        {
            for(int i = 0; i < AcceptableStates.Length; i++ )
            {
                if (value == AcceptableStates[i] && (State != "unconcious" || State != "dead") && value != "revival")
                {
                    //Value is anything that is not revival
                    State = value;
                    return;
                }else if((State == "unconcious" || State == "dead") && value == "revival")
                {
                    //Value is revival and player is either dead or unconcious
                    State = value;
                    return;
                }
                //Either an invalid state was given or player is concious but revival was used
            }
        }
    }
    #endregion


    #region Methods
    //Check to see if player should be unconscious/dead and adjust state accordingly.
    float HPStateUpdate(Player target)
    {
        if (target.CurrentHP <= 0 && (target.State != "unconcious" || target.State != "dead"))
        {
            //Current hp is 0 or lower and player is not already dead/unconcious
            target.State = "unconcious";
            return 1;
        }
        else if (target.CurrentHP <= -target.MaxHP && target.State != "dead")
        {
            //Current hp is at the death threshold and player is not already dead
            target.State = "dead";
            return 2;
        }
        return 0;
    }

    //Simply check to see if we can take damage and return true after applying damage
    //Also checks to see if we should be dead/unconcious
    bool TakeDamage(Player target,float dmg)
    {
        switch (HPStateUpdate(target))
        {
            case 0:
                target.CurrentHP -= dmg;
                HPStateUpdate(target);
                return true;
            case 1:
                target.CurrentHP -= dmg;
                HPStateUpdate(target);
                return true;
            case 2:
                return false;
        }
        //IDK WHEN WE WOULD EVER GET HERE BUT WE DUN GOOFED IF WE DO
        return false;
    }

    //pretty self explanitory
    bool TakeHunger(Player target)
    {
        if (target.Hunger > 0)
        {
            target.Hunger--;
            return true;
        } else {
            TakeDamage(target,DamagePerTick);
            return false;
        }
    }

    bool TakeThirst(Player target)
    {
        if (target.Thirst > 0)
        {
            target.Thirst--;
            return true;
        } else {
            TakeDamage(target, DamagePerTick);
            return false;
        }
    }

    //Timer to make sure we only trigger methods when we intend to
    private bool SetTimer(float timer, float offset)
    {
        if (Time.time >= timer)
        {
            timer = Time.time + offset;
            return true;
        }
        else
        {
            return false;
        }
    }
    #endregion
}
