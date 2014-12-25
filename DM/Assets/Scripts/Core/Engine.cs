using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Core.Interface;
using Core.Manager;
using System.IO;

/// <summary>
/// Everything start from here
/// </summary>
public class Engine : MonoBehaviour 
{
	private static Engine _instance;
	public static Engine instance { get { return _instance; }}

	#region TickObjectQueue
	private List<ITickObject> _tickObjects = new List<ITickObject>();
	public static void AddTickObject(ITickObject tickObject)
	{
		if(tickObject == null)
		{
			Log.Warning("Warning: Don't add the null object to tick object list");
			return;
		}
		if(_instance._tickObjects.Exists(t => t == tickObject))
		{
			Log.Warning("Warning: Don't add the same tick object twice.");
			return;
		}
		_instance._tickObjects.Add(tickObject);
	}

	public static void RemoveTickObject(ITickObject tickObject)
	{
		if(tickObject == null)
		{
			Log.Warning("Warning: Can not remove the null object from the tick object list");
			return;
		}
		if(!_instance._tickObjects.Remove(tickObject))
		{
			Log.Warning("Warning: Remove tick object error. May be the tick object is not in list.");
		}
	}
	#endregion

	void Awake()
	{
		if(_instance != null)
		{
			Destroy(this);
		}
		else
		{
			_instance = this;
			DontDestroyOnLoad(this);
		}
	}

	// Use this for initialization
	void Start () 
	{
		// Load All Supporter
		EventManager.instance.Init();
		//ModuleManager.instance.AddAdditionalModule(new StartModule());
		//ModuleManager.instance.GotoModule(new StartModule());
		//ModuleManager.instance.GotoModule(new InitalizeModule());

		//StartCoroutine(DelayStart());
	}

//	IEnumerator DelayStart()
//	{
//		yield return new WaitForSeconds(1f);
//		ModuleManager.instance.GotoModule(new InitalizeModule());
//	}

	/// <summary>
	/// Update Game (None MonoBehaviour Partition.)
	/// </summary>
	protected void Update()
	{
		_tickObjects.ForEach(tickObject => tickObject.Update());
	}
}
