using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Indexer : MonoBehaviour
{
    public int data;
    [SerializeField]int x; public int X { get { return x; } set { x = value; } }
    [SerializeField]int z; public int Z { get { return z; } set { z = value; } }
    [SerializeField]int floor; public int Floor { get { return floor; }
        set { floor = value;
            transform.position = new Vector3(transform.position.x, floor, transform.position.z); } }
    public bool isFull;
    public void SetXZ(int x, int z)
    {
        data = 1;
        floor = 0;
        isFull = false;
        this.x = x;
        this.z = z;
    }


}
