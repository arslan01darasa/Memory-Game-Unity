using StackTheBlockArslan;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SpatialTracking;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class GameMode : MonoBehaviour
{
	[HideInInspector]
	public bool isAREnabled;

	[Tooltip("Scale of gameobjects in the real world")]
	[SerializeField]
	[Range(0f, 1f)]
	private float ARScaleFactor = 0.3f;

	[SerializeField]
	private GameObject SurfaceNotFoundGraphic;

	[SerializeField]
	public GameObject startGameGraphic;

	[HideInInspector]
	public bool isARModeOn;

	private Camera arCamera;

	private ARRaycastManager aRRaycastManager;

	[SerializeField]
	private GameObject stack;

	[SerializeField]
	private List<GameObject> nonARAssets;

	private List<GameObject> ARAssets = new List<GameObject>();

	[SerializeField]
	private GameObject maskPlane;

	private Light directionLight;

	private float AROriginScale => ARScaleFactor * 100f;

	private void Start()
	{
		StartCoroutine(SetupAR());
		SurfaceNotFoundGraphic.SetActive(value: false);
		maskPlane.SetActive(value: false);
		directionLight = UnityEngine.Object.FindObjectOfType<Light>();
	}

	private IEnumerator SetupAR()
	{
		UIManager.Instance.ARBtn.gameObject.SetActive(value: false);
		GameObject arSessionGO = new GameObject("AR Session");
		arSessionGO.AddComponent<ARSession>();
		if (ARSession.state == ARSessionState.None || ARSession.state == ARSessionState.CheckingAvailability)
		{
			yield return ARSession.CheckAvailability();
		}
		if (ARSession.state == ARSessionState.Unsupported)
		{
			UnityEngine.Object.Destroy(arSessionGO);
			yield break;
		}
		UIManager.Instance.ARBtn.gameObject.SetActive(value: true);
		arSessionGO.SetActive(value: false);
		arSessionGO.AddComponent<ARInputManager>();
		GameObject gameObject = new GameObject("AR Session Origins");
		gameObject.SetActive(value: false);
		ARSessionOrigin aRSessionOrigin = gameObject.AddComponent<ARSessionOrigin>();
		aRRaycastManager = gameObject.AddComponent<ARRaycastManager>();
		gameObject.AddComponent<ARPlaneManager>();
		GameObject gameObject2 = new GameObject("AR Camera");
		gameObject2.transform.SetParent(gameObject.transform);
		arCamera = gameObject2.AddComponent<Camera>();
		arCamera.clearFlags = CameraClearFlags.Color;
		arCamera.backgroundColor = Color.black;
		arCamera.nearClipPlane = 0.1f;
		arCamera.farClipPlane = 80f;
		aRSessionOrigin.camera = arCamera;
		TrackedPoseDriver trackedPoseDriver = gameObject2.AddComponent<TrackedPoseDriver>();
		gameObject2.AddComponent<ARCameraManager>();
		gameObject2.AddComponent<ARCameraBackground>();
		trackedPoseDriver.SetPoseSource(TrackedPoseDriver.DeviceType.GenericXRDevice, TrackedPoseDriver.TrackedPose.ColorCamera);
		gameObject.transform.localScale = Vector3.one * AROriginScale;
		ARAssets.Add(arSessionGO);
		ARAssets.Add(gameObject);
		ARAssets.Add(maskPlane);
		ARAssets.Add(SurfaceNotFoundGraphic);
	}

	public void SwitchGameMode()
	{
		isARModeOn = !isARModeOn;
		ARAssets.ForEach(delegate(GameObject obj)
		{
			obj.SetActive(isARModeOn);
		});
		nonARAssets.ForEach(delegate(GameObject obj)
		{
			obj.SetActive(!isARModeOn);
		});
		stack.transform.position = new Vector3(5f, -0.5f, 0f);
		if (!isARModeOn)
		{
			stack.gameObject.SetActive(value: true);
			stack.transform.rotation = Quaternion.identity;
		}
		directionLight.shadows = (isARModeOn ? LightShadows.Soft : LightShadows.None);
		UIManager.Instance.ToggleARModeSprite(isARModeOn);
	}

	private void Update()
	{
		if (isARModeOn)
		{
			UpdateStartBlockARPos();
		}
	}

	private void UpdateStartBlockARPos()
	{
		Vector3 v = arCamera.ViewportToScreenPoint(new Vector2(0.5f, 0.5f));
		List<ARRaycastHit> list = new List<ARRaycastHit>();
		bool flag = false;
		if (aRRaycastManager.Raycast(v, list, TrackableType.PlaneWithinPolygon))
		{
			flag = true;
			stack.transform.position = list[0].pose.position;
			maskPlane.transform.position = stack.transform.position;
			Quaternion quaternion = Quaternion.LookRotation(new Vector3(arCamera.transform.position.x, list[0].pose.position.y, arCamera.transform.position.z) - list[0].pose.position) * Quaternion.Euler(0f, -45f, 0f);
			if (Quaternion.Angle(stack.transform.rotation, quaternion) > 20f)
			{
				stack.transform.rotation = quaternion;
			}
			else
			{
				stack.transform.rotation = Quaternion.Lerp(stack.transform.rotation, quaternion, Time.deltaTime * 10f);
			}
		}
		stack.SetActive(flag);
		SurfaceNotFoundGraphic.SetActive(!flag);
		startGameGraphic.SetActive(flag);
	}

	public void StartGame()
	{
		if (isARModeOn)
		{
			if (stack.activeSelf)
			{
				LazySingleton<GameManager>.Instance.StartGame();
				SoundManager.Instance.playSound(AudioClips.UI);
				base.enabled = false;
			}
		}
		else
		{
			LazySingleton<GameManager>.Instance.StartGame();
			SoundManager.Instance.playSound(AudioClips.UI);
			base.enabled = false;
		}
	}
}
