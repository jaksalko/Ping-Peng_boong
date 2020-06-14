using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Indexer : MonoBehaviour
{
    [SerializeField]int x; public int X { get { return x; } set { x = value; } }
    [SerializeField]int z; public int Z { get { return z; } set { z = value; } }
    public int floor;
    public bool isFull;
    public void SetXZ(int x, int z)
    {
        floor = 0;
        isFull = false;
        this.x = x;
        this.z = z;
    }


}
