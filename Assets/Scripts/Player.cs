using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //Singleton instance of this class
    public Player player;

    private readonly string[] AcceptableStates = {"idle","sitting","walking","running","swimming","mounted","unconcious" };

    void Awake()
    {
        player = this;
    }

    int CurrentHP
    {
        get
        {
            return CurrentHP;
        }

        set
        {
            CurrentHP = value;
        }
    }

    int MaxHP
    {
        get
        {
            return MaxHP;
        }
        set
        {
            MaxHP = value;
        }
    }

    int CurrentMP
    {
        get
        {
            return CurrentMP;
        }
        set
        {
            CurrentMP = value;
        }
    }

    int MaxMP
    {
        get
        {
            return MaxMP;
        }
        set
        {
            MaxMP = value;
        }
    }
    
    int CurrentStamina
    {
        get
        {
            return CurrentStamina;
        }
        set
        {
            CurrentStamina = value;
        }
    }

    int MaxStamina
    {
        get
        {
            return MaxStamina;
        }
        set
        {
            MaxStamina = value;
        }
    }

    int Hunger
    {
        get
        {
            return Hunger;
        }
        set
        {
            Hunger = value;
        }
    }

    int Thirst
    {
        get
        {
            return Thirst;
        }
        set
        {
            Thirst = value;
        }
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
                if (value == AcceptableStates[i])
                {
                    State = value;
                }
            }
        }
    }
}
