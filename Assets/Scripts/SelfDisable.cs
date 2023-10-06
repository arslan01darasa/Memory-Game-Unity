using StackTheBlockArslan;
using UnityEngine;

public class SelfDisable : MonoBehaviour
{
	[Tooltip("Will make the Gameobject's Rigidbody kinematic after this times")]
	[SerializeField]
	private int makeKinematicTime;

	[Tooltip("Will disable the Gameobject afer this time")]
	[SerializeField]
	private int disableTime;

	private Rigidbody rbody;

	private void Awake()
	{
		rbody = GetComponent<Rigidbody>();
	}

	private void OnEnable()
	{
		if (LazySingleton<GameManager>.Instance.gameMode.isARModeOn && (bool)rbody)
		{
			rbody.isKinematic = false;
			Invoke("UnParent", 2f);
			return;
		}
		if ((bool)rbody)
		{
			rbody.isKinematic = false;
			Invoke("MakeKinematic", makeKinematicTime);
		}
		Invoke("Disable", disableTime);
	}

	private void MakeKinematic()
	{
		rbody.isKinematic = true;
	}

	private void Disable()
	{
		base.gameObject.SetActive(value: false);
	}

	private void OnDisable()
	{
		CancelInvoke();
		if ((bool)rbody)
		{
			rbody.isKinematic = true;
		}
	}

	private void UnParent()
	{
		base.transform.parent = null;
	}
}
