using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrackedBlock : Block
{
    public int count;
    public int x;
    public int z;

    public Material material;
    public MeshRenderer meshRenderer;
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Player onCrackedPlayer = other.gameObject.GetComponent<Player>();
            count++;
            Debug.Log("through the cracked block :" + count);
            if(count == 3)
            {
                if(BlockNumber.cracked == num)
                {
                    num = BlockNumber.broken;
                    onCrackedPlayer.CrackedBlockisBroken(x, z, num);
                    
                }
                else if(BlockNumber.upperCracked == num)
                {
                    num = BlockNumber.upperBroken;
                    onCrackedPlayer.CrackedBlockisBroken(x, z, num);
                }

                meshRenderer.material = material;
            }
        }
    }

}
