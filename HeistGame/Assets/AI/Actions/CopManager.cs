using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RAIN.Action;
using RAIN.Core;

public class CopManager : RAINAction
{
	public List <RAIN.Core.AI> Cops;

	void Start()
	{
		Cops = new List<RAIN.Core.AI> ();
	}

	void Update (RAIN.Core.AI ai)
	{

		foreach (var element in Cops)
		{
			element.WorkingMemory.SetItem("varPath", "PatrolPath");
		}
	}
}
