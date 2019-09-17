using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //Singleton instance of this class
    public Player player;

    private readonly string[] AcceptableStates = {"idle","sitting","laying","prone","walking","running","swimming","mounted","unconcious","revival","dead" };


    void Awake()
    {
        player = this;
        MaxHP = 100;
        CurrentHP = 100;
        MaxMP = 100;
        CurrentMP = 100;
        MaxStamina = 100;
        CurrentStamina = 100;
        Hunger = 100;
        Thirst = 100;
        State = "idle";
    }

    #region Properties
    int CurrentHP
    {
        get
        { return CurrentHP; }

        set
        { CurrentHP = value; }
    }

    public int MaxHP
    {
        get
        { return MaxHP; }
        set
        { MaxHP = value; }
    }

    int CurrentMP
    {
        get
        { return CurrentMP; }
        set
        { CurrentMP = value; }
    }

    int MaxMP
    {
        get
        { return MaxMP; }
        set
        { MaxMP = value; }
    }
    
    int CurrentStamina
    {
        get
        { return CurrentStamina; }
        set
        { CurrentStamina = value; }
    }

    int MaxStamina
    {
        get
        { return MaxStamina; }
        set
        { MaxStamina = value; }
    }

    int Hunger
    {
        get
        { return Hunger; }
        set
        { Hunger = value; }
    }

    int Thirst
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

    void Update()
    {
        if (CurrentHP <= 0 && (State != "unconcious" || State != "dead"))
        {
            //Current hp is 0 or lower and player is not already dead/unconcious
            player.State = "unconcious";
        } else if (CurrentHP <= -MaxHP && State != "dead")
        {
            //Current hp is at the death threshold and player is not already dead
            player.State = "dead";
        }

    }

    

}
