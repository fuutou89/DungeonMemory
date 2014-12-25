using UnityEngine;
using System.Collections;

public class DungeonObj : MonoBehaviour 
{
	GameObject colgo;

	void OnTriggerEnter(Collider col)
	{
		Debug.Log("OnTriggerEnter");
		colgo = col.gameObject;
		iTween.Pause(colgo);
		StartCoroutine(DelayDestory());
	}

	IEnumerator DelayDestory()
	{
		yield return new WaitForSeconds(1f);
		iTween.Resume(colgo);
		Destroy(gameObject);
	}
}
