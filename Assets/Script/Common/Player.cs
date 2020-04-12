using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
	public int adjustX;
	public int adjustZ;
	public float speed;
	public Text moveCountText;
	
	

	Map stage;

	int mapsizeH;
	int mapsizeW;
	int[,] map;

	int posZ;     // vertical
	int posX;     // horizental
	int movedir;
	int count = 0;
	[SerializeField]
	bool isMoving = false;
	public bool Moving()
	{
		return isMoving;
	}
	[SerializeField]
	public bool isActive = false;
	Vector3 targetPos;


	// Start is called before the first frame update
	void Start()
    {
		isMoving = false;
		//isActive = false;

		stage = GameController.instance.map;
		mapsizeH = stage.mapsizeH;
		mapsizeW = stage.mapsizeW;
		map = stage.map;

		FindObjectOfType<TouchMove>().Move += PlayerControl;

		FindPlayer();
    }
	public void FindPlayer()
	{
		posX = (int)transform.position.x;
		posZ = (int)transform.position.z;

		Debug.Log(gameObject.name + "   Vertical : " + posZ + " Horizental : " + posX);
	}
	// Update is called once per frame
	void FixedUpdate()
    {
        if(isMoving)
        {
			transform.position = Vector3.MoveTowards(transform.position, targetPos, speed);
			if (transform.position == targetPos)
			{
				isMoving = false;
				transform.position = new Vector3(targetPos.x, targetPos.y, targetPos.z);
			}
		}
        
    }

    void PlayerControl(int direction)//direction 1 : u 2: r 3 : d 4 : l
    {
        if(!isMoving && isActive)
        {
			switch (direction)
			{
				case 1:
					Up();
					break;
				case 2:
					Right();
					break;
				case 3:
					Down();
					break;
				case 4:
					Left();
					break;
			}
		}
        
        
    }

    void Up()//z+1
    {
		//transform.rotation = Quaternion.Euler(-90, 0, 0);

		movedir = 0;
		count++;
		map[posZ,posX] = 0;

		while (map[posZ+1 , posX] == 0)
		{
			posZ++;
		}

		Debug.Log(posZ + "," + posX);
		targetPos = new Vector3(posX, transform.position.y, posZ);
		map[posZ,posX] = 2;
		isMoving = true;
	}
    void Down()//z-1
    {
		//transform.rotation = Quaternion.Euler(-90, 180, 0);

		movedir = 0;
		count++;
		map[posZ, posX] = 0;

		while (map[posZ - 1, posX] == 0)
		{
			posZ--;
		}

		Debug.Log(posZ + "," + posX);
		targetPos = new Vector3(posX, transform.position.y, posZ);
		map[posZ, posX] = 2;
		isMoving = true;
	}
    void Right()//x+1
    {
		//transform.rotation = Quaternion.Euler(-90, 90, 0);

		movedir = 0;
		count++;
		map[posZ, posX] = 0;

		while (map[posZ, posX+1] == 0)
		{
			posX++;
		}

		Debug.Log(posZ + "," + posX);
		targetPos = new Vector3(posX, transform.position.y, posZ);
		map[posZ, posX] = 2;
		isMoving = true;
	}
    void Left()//x-1
    {
		//transform.rotation = Quaternion.Euler(-90, -90, 0);

		movedir = 0;
		count++;
		map[posZ, posX] = 0;

		while (map[posZ, posX-1] == 0)
		{
			posX--;
		}

		Debug.Log(posZ + "," + posX);
		targetPos = new Vector3(posX, transform.position.y, posZ);
		map[posZ, posX] = 2;
		isMoving = true;
	}
}
