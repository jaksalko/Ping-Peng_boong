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

    public MeshRenderer[] crackerRenderer;
    public MeshFilter[] crackerMesh;
    public Mesh cracker2;
    public Mesh cracker3;

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Player onCrackedPlayer = other.gameObject.GetComponent<Player>();
            count++;



            Debug.Log("through the cracked block :" + count);
            if(count == 1)
            {
                for(int i = 0; i < crackerMesh.Length; i++)
                {
                    
                        crackerMesh[i].mesh = cracker2;
                }
                //crackerMesh[4].mesh = cracker2;
            }
            else if(count == 2)
            {
                /*for (int i = 0; i < crackerMesh.Length; i++)
                {
                    
                        crackerMesh[i].mesh = cracker3;
                }*/
                crackerMesh[4].mesh = cracker3;
            }
            else if(count == 3)
            {
                Debug.Log(num);
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

                //meshRenderer.material = material;

                for(int i = 0; i < crackerRenderer.Length; i++)
                {
                    crackerRenderer[i].material = material;
                }
            }
            
        }
    }

}
