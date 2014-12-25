using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnbreakableCollider : MonoBehaviour 
{
	public GameObject MoveCube;

	public Transform DotPrefab;
	Vector3 lastDotPosition = Vector3.zero;
	bool lastPointExists;

	private List<Transform> waypointlist = new List<Transform>();
	float percentsPerScenond = 0.05f;
	float currentPathPercent = 0.0f;

	private List<Vector3> pathposlist = new List<Vector3>();

	// Use this for initialization
	void Start () 
	{
		lastPointExists = false;
	}
	
	// Update is called once per frame
	void Update () 
	{
		GetMousePath();

		if(Input.GetMouseButtonDown(1))
		{
			iTween.Pause();
		}

		if(Input.GetMouseButtonUp(1))
		{
			iTween.Resume();
		}
	}

	void GetMousePath()
	{
		if(Input.GetMouseButtonUp(0))
		{
			//StartCoroutine(UpdatePath());\
			MovePath();
		}
		else if(Input.GetMouseButtonDown(0))
		{
			pathposlist = new List<Vector3>();
			Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
			lastDotPosition = mouseRay.origin - mouseRay.direction / mouseRay.direction.z * mouseRay.origin.z;
			waypointlist.ForEach(e => {
				GameObject.Destroy(e.gameObject);
			});
			waypointlist = new List<Transform>();
			//StopCoroutine(UpdatePath());
		}

		if(Input.GetMouseButton(0))
		{
			Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
			Vector3 newDotPosition = mouseRay.origin - mouseRay.direction / mouseRay.direction.z * mouseRay.origin.z;
			if(newDotPosition != lastDotPosition)
			{
				MakeADot(newDotPosition);
			}
		}
	}

	void MovePath()
	{
		Vector3[] movepath = ConstantPath(pathposlist).ToArray();//pathposlist.ToArray();

		iTween.MoveTo(MoveCube,iTween.Hash("path",movepath,"speed", 5f, "easetype",iTween.EaseType.linear));
	}

	private List<Vector3> ConstantPath(List<Vector3> pathlist)
	{
		List<Vector3> FinalPath = new List<Vector3>();
		Vector3[] position = pathlist.ToArray();
		FinalPath.Add(position[0]);
		int amount = 1000;
		float distance = 0.5f;
		int atual = 0;
		for( int i = 0; i < amount; i++ )
		{
			if( Vector3.Distance(FinalPath[atual],iTween.PointOnPath(position,(float)i/amount)) > distance )
			{
				FinalPath.Add( iTween.PointOnPath(position,(float)i/amount) );
				atual++;
			}
		}
		FinalPath.Add(position[position.Length-1]);
		return FinalPath;
	}

	IEnumerator UpdatePath()
	{
		while(true)
		{
			yield return new WaitForFixedUpdate();
			currentPathPercent += percentsPerScenond * Time.deltaTime;
			Transform[] waypointArray = waypointlist.ToArray();
			iTween.PutOnPath(MoveCube, waypointArray, currentPathPercent);
			if(currentPathPercent > 1) break;
		}
		Debug.Log("Path Remeber");
	}

	void OnDrawGizmos()
	{
		Transform[] waypointArray = waypointlist.ToArray();
		iTween.DrawPath(waypointArray);
	}

	void MakeADot(Vector3 newDotPosition)
	{
		// use random identity to make dots looks more different
		//Transform dot = (Transform) Instantiate(DotPrefab, newDotPosition, Quaternion.identity);
		//waypointlist.Add(dot);

		if(lastPointExists)
		{
			float distance = Vector3.Distance(newDotPosition, lastDotPosition);
			if(distance > 0.5f && distance < 1f)
			{
//				GameObject colliderKeeper = new GameObject("collider");
//				BoxCollider bc = colliderKeeper.AddComponent<BoxCollider>();
//				colliderKeeper.transform.position = Vector3.Lerp(newDotPosition, lastDotPosition, 0.5f);
//				colliderKeeper.transform.LookAt(newDotPosition);
//				bc.size = new Vector3(0.1f, 0.1f, Vector3.Distance(newDotPosition, lastDotPosition));

				Vector3 pos = Vector3.Lerp(newDotPosition, lastDotPosition, 0.5f);

				pathposlist.Add(pos);
				
				Transform dot = (Transform) Instantiate(DotPrefab, pos, Quaternion.identity);
				waypointlist.Add(dot);
				lastDotPosition = newDotPosition;
			}
		}

		lastPointExists = true;
	}
}
