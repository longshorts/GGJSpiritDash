using UnityEngine;
using System.Collections;

public class SpiritGenerator : MonoBehaviour
{
	public GameObject SpiritObj;
	public float Count = 10;

	public float MinX = -200;
	public float MaxX = 200;
	public float MinY = -200;
	public float MaxY = 200;

	// Use this for initialization
	void Start ()
	{
		GenerateSpirit();
	}

	private void GenerateSpirit()
	{
		float x;
		float z;

		for(int i = 0; i < Count; i++)
		{
			bool Find = true;
			while(Find)
			{
				x = Random.Range(MinX, MaxX);
				z = Random.Range(MinY, MaxY);

				Collider[] col = Physics.OverlapSphere(new Vector3(x,1,z), 3, 1 << 9);
				if(col.Length == 0)
				{
					GameObject obj = (GameObject)Instantiate(SpiritObj, new Vector3(0,0,0), new Quaternion());
					obj.name = "Spirit";
					obj.transform.parent = transform;
					obj.transform.localPosition = new Vector3(x,1,z);
					Find = false;
				}
			}
		}
	}
}
