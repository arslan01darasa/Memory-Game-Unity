using System;
using UnityEngine;

[Serializable]
public class Ad
{
	public string placementID;

	public bool useFrequency = true;

	public int adFrequency = 1;

	public float delay;

	[HideInInspector]
	public int callCount;

	public Action success;

	public Action failure;
}
