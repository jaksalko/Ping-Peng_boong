using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class Block : MonoBehaviour , IBlock
{
    public enum Type
    {
        Ground,
        SecondGround,
        Slope,
        Obstacle,
        Parfait,
        Cracked,
        broken,
        Cloud,
        Outline
    };


    public int Data { get; set; }
    public bool Snow { get; set; }
    public virtual void Init(int block_num , bool snow) { Data = block_num; Snow = snow; }

    public bool IsSnow()
    {
        if(Snow)
        {
            Snow = false;
            return true;
        }
        else
        {
            return false;
        }
        //throw new System.NotImplementedException();
    }
}
