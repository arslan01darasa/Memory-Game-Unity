using StackTheBlockArslan;
using System;
using System.Collections;
using UnityEngine;

public class GameplayController : LazySingleton<GameplayController>
{
	[Header("Block")]
	[Tooltip("This will contain all the stacked blocks")]
	[SerializeField]
	private Transform stack;

	[Tooltip("The Block from which the Game starts")]
	[SerializeField]
	private Transform startBaseBlock;

	private Transform baseBlock;

	private GameObject currentBlock;

	private Material blockMat;

	private string shaderType = "Standard";

	private Texture selectedSkinTexture;

	[Header("Gameplay")]
	[Tooltip("Threshold for perfect score. Bigger the value, Easier to score a perfect")]
	[SerializeField]
	private float perfectDist;

	private bool xOrZ;

	private float yOffset;

	[Tooltip("Color Controller Reference")]
	public ColorController colorController;

	[Tooltip("Camera Controller Reference")]
	[SerializeField]
	private CameraController cameraController;

	private ObjectPooler objectPooler;

	private bool isInputActivated;

	private bool isGameRunning;

	[Header("Cheat")]
	[Tooltip("Enable this to let the game play on its own.")]
	[SerializeField]
	private bool cheat;

	[Tooltip("Degree of Perfection in cheat mode. smaller value --> more perfects")]
	[SerializeField]
	private float cheatRandomisation;

	[Header("Block Speed")]
	[Tooltip("Default Block Speed when game starts")]
	[SerializeField]
	private float defaultBlockSpeed;

	[Tooltip("The Maximum Block Speed")]
	[SerializeField]
	private float maxBlockSpeed;

	[Tooltip("Block speed increases by this value")]
	[SerializeField]
	private float blockSpeedDelta;

	[Tooltip("Block speed increases every this score")]
	[SerializeField]
	private int blockSpeedIncreaseScore;

	[Tooltip("Block speed resets to default value every this score")]
	[SerializeField]
	private int blockSpeedResetScore;

	private float blockSpeed;

	private bool isARModeOn;

	[Header("Stack Height Control in AR")]
	[Tooltip("Height freezes at this block count")]
	[SerializeField]
	private int heightCutOffInBlockCount = 10;

	[SerializeField]
	[Tooltip("Moving down speed when stack reaches the max height")]
	private float moveDownSpeed = 8f;

	[SerializeField]
	[Tooltip("Moving up speed for the stack at gameover")]
	private float moveUpSpeed = 2.5f;

	private int blockCount;

	private float stackMoveSpeed;

	private Vector3 stackStartPos;

	private Vector3 stackTargetPos;

	[Header("Continue Game")]
	[Tooltip("The size of the block to continue from after gameover")]
	[SerializeField]
	private float continueBlockSize = 1.7f;

	[Tooltip("Option to continue game will be shown these many times")]
	[SerializeField]
	private int frequency = 1;

	[Tooltip("Minimum score required to show the continue game option")]
	[SerializeField]
	private int minScoreToContinue = 15;

	private int gameOverCount;

	private bool alreadyContinued;

	private void Awake()
	{
		objectPooler = GetComponent<ObjectPooler>();
		yOffset = startBaseBlock.lossyScale.y;
		GameManager instance = LazySingleton<GameManager>.Instance;
		instance.GameInitialized = (Action)Delegate.Combine(instance.GameInitialized, new Action(InitGame));
		GameManager instance2 = LazySingleton<GameManager>.Instance;
		instance2.GameStarted = (Action)Delegate.Combine(instance2.GameStarted, new Action(StartGame));
	}

	private void OnDestroy()
	{
		GameManager instance = LazySingleton<GameManager>.Instance;
		instance.GameInitialized = (Action)Delegate.Remove(instance.GameInitialized, new Action(InitGame));
		GameManager instance2 = LazySingleton<GameManager>.Instance;
		instance2.GameStarted = (Action)Delegate.Remove(instance2.GameStarted, new Action(StartGame));
	}

	private void InitGame()
	{
		objectPooler.disableAllPooled();
		baseBlock = startBaseBlock;
		colorController.Reset();
		baseBlock.GetComponent<MeshRenderer>().material.SetColor("_Color", colorController.BlockColor);
		MeshUVAdjuster.AdjustUVs(baseBlock);
		blockSpeed = defaultBlockSpeed - blockSpeedDelta;
		if (isARModeOn)
		{
			blockCount = 0;
		}
		alreadyContinued = false;
	}

	public void StartGame()
	{
		if (!isGameRunning)
		{
			isGameRunning = true;
			isARModeOn = LazySingleton<GameManager>.Instance.gameMode.isARModeOn;
			if (isARModeOn)
			{
				stackMoveSpeed = moveDownSpeed;
				stackStartPos = (stackTargetPos = stack.position);
				baseBlock.GetComponent<MeshRenderer>().material.renderQueue = 3000;
			}
			Invoke("ActivateInputWithDelay", 0.2f);
			BringNewBlock();
		}
	}

	public void UpdateSkin(Texture texture)
	{
		selectedSkinTexture = texture;
		startBaseBlock.GetComponent<MeshRenderer>().material.mainTexture = selectedSkinTexture;
	}

	private void ActivateInputWithDelay()
	{
		isInputActivated = true;
	}

	private void Update()
	{
		if (isInputActivated && UnityEngine.Input.touchCount == 1 && UnityEngine.Input.GetTouch(0).phase == TouchPhase.Began)
		{
			Tap();
		}
		if (isARModeOn && blockCount != 0)
		{
			stack.position = Vector3.Lerp(stack.position, stackTargetPos, Time.deltaTime * stackMoveSpeed);
		}
	}

	private void BringNewBlock()
	{
		xOrZ = !xOrZ;
		GameObject pooledObject = objectPooler.GetPooledObject(ObjectPoolItems.Block, activeState: true);
		pooledObject.transform.SetParent(stack.transform);
		pooledObject.transform.localScale = baseBlock.transform.localScale;
		float x = baseBlock.transform.localPosition.x;
		float y = baseBlock.transform.localPosition.y + yOffset;
		float z = baseBlock.transform.localPosition.z;
		float num = 0f;
		if (isARModeOn)
		{
			blockCount++;
			if (blockCount > heightCutOffInBlockCount)
			{
				stackTargetPos = stack.position - Vector3.up * yOffset;
			}
		}
		BlockScript component = pooledObject.GetComponent<BlockScript>();
		if (xOrZ)
		{
			num = startBaseBlock.transform.lossyScale.x * 0.85f + pooledObject.transform.localScale.x * 0.5f;
			component.setPositions(new Vector3(startBaseBlock.transform.localPosition.x + num, y, z), new Vector3(startBaseBlock.transform.localPosition.x - num, y, z));
		}
		else
		{
			num = startBaseBlock.transform.lossyScale.z * 0.85f + pooledObject.transform.localScale.z * 0.5f;
			component.setPositions(new Vector3(x, y, startBaseBlock.transform.localPosition.z + num), new Vector3(x, y, startBaseBlock.transform.localPosition.z - num));
		}
		Vector3 a = xOrZ ? (num * Vector3.right) : (num * Vector3.forward);
		Vector3 b = Vector3.up * yOffset;
		pooledObject.transform.localPosition = baseBlock.transform.localPosition - a * 2f + b;
		pooledObject.transform.rotation = baseBlock.transform.rotation;
		blockMat = new Material(Shader.Find(shaderType));
		blockMat.SetColor("_Color", colorController.BlockColor);
		blockMat.mainTexture = selectedSkinTexture;
		pooledObject.GetComponent<MeshRenderer>().material = blockMat;
		if (isARModeOn)
		{
			blockMat.renderQueue = 3000;
		}
		if (LazySingleton<ScoreAndCashManager>.Instance.currentScore % blockSpeedResetScore == 0)
		{
			blockSpeed = defaultBlockSpeed;
		}
		else if (LazySingleton<ScoreAndCashManager>.Instance.currentScore % blockSpeedIncreaseScore == 0)
		{
			float num2 = blockSpeed + blockSpeedDelta;
			blockSpeed = ((num2 <= maxBlockSpeed) ? num2 : maxBlockSpeed);
		}
		component.speed = blockSpeed;
		currentBlock = pooledObject;
		component.enabled = true;
		MeshUVAdjuster.CopyUVs(baseBlock, currentBlock.transform);
	}

	private void Tap()
	{
		Vector3 a = xOrZ ? VectorHelper.GetXVector(currentBlock.transform.localPosition) : VectorHelper.GetZVector(currentBlock.transform.localPosition);
		Vector3 b = xOrZ ? VectorHelper.GetXVector(baseBlock.transform.localPosition) : VectorHelper.GetZVector(baseBlock.transform.localPosition);
		float num = xOrZ ? (currentBlock.transform.lossyScale.x * 0.5f + baseBlock.lossyScale.x * 0.5f) : (currentBlock.transform.lossyScale.z * 0.5f + baseBlock.lossyScale.z * 0.5f);
		if (Vector3.Distance(a, b) > num)
		{
			currentBlock.SetActive(value: false);
			GameObject pooledObject = objectPooler.GetPooledObject(ObjectPoolItems.leftoverBlock, activeState: true);
			VectorHelper.CopyTransformProperties(pooledObject.transform, currentBlock.transform);
			pooledObject.GetComponent<MeshRenderer>().material = blockMat;
			if(pooledObject.transform != null)
			{
                MeshUVAdjuster.CopyUVs(currentBlock.transform,
					pooledObject.transform);
            }
			SoundManager.Instance.playSound(AudioClips.split);
			isGameRunning = false;
			isInputActivated = false;
			if (isARModeOn)
			{
				stackTargetPos = stackStartPos;
				stackMoveSpeed = moveUpSpeed;
			}
			gameOverCount++;
			if (!isARModeOn && !alreadyContinued && frequency != 0 && gameOverCount % frequency == 0 && LazySingleton<ScoreAndCashManager>.Instance.currentScore >= minScoreToContinue)
			{
				LazySingleton<GameManager>.Instance.GameContinued();
			}
			else
			{
				LazySingleton<GameManager>.Instance.GameOver();
			}
		}
		else
		{
			if (Vector3.Distance(a, b) < perfectDist)
			{
				PerfectScore();
			}
			else
			{
				ChopBlock();
			}
			BringNewBlock();
			cameraController.UpdatePos();
			LazySingleton<ScoreAndCashManager>.Instance.UpdateScore();
		}
	}

	private void PerfectScore()
	{
		currentBlock.GetComponent<BlockScript>().enabled = false;
		currentBlock.transform.position = new Vector3(baseBlock.position.x, currentBlock.transform.position.y, baseBlock.position.z);
		baseBlock = currentBlock.transform;
		objectPooler.GetPooledObject(ObjectPoolItems.Perfectfx, currentBlock.transform.position, activeState: true).transform.eulerAngles = new Vector3(90f, currentBlock.transform.eulerAngles.y, 0f);
		SoundManager.Instance.playSound(AudioClips.perfect);
	}

	private void ChopBlock()
	{
		VectorHelper.Vector3Coord coordToModify = (!xOrZ) ? VectorHelper.Vector3Coord.z : VectorHelper.Vector3Coord.x;
		SoundManager.Instance.playSound(AudioClips.split);
		currentBlock.GetComponent<MeshRenderer>().material = blockMat;
		float num = xOrZ ? baseBlock.localPosition.x : baseBlock.localPosition.z;
		float num2 = xOrZ ? baseBlock.lossyScale.x : baseBlock.lossyScale.z;
		float num3 = xOrZ ? currentBlock.transform.localPosition.x : currentBlock.transform.localPosition.z;
		float num4 = Mathf.Abs(num - num3);
		float num5 = num2 - num4;
		GameObject pooledObject = objectPooler.GetPooledObject(ObjectPoolItems.leftoverBlock, activeState: true);
		pooledObject.transform.SetParent(stack, worldPositionStays: true);
		int num6 = (!(num3 < num)) ? 1 : (-1);
		float num7 = num + (float)num6 * num2 * 0.5f - (float)num6 * num5 * 0.5f;
		float num8 = num4;
		float value = num7 + (float)num6 * num5 * 0.5f + (float)num6 * num8 * 0.5f;
		pooledObject.transform.localScale = VectorHelper.GetVectorWith(coordToModify, currentBlock.transform.lossyScale, num8);
		pooledObject.transform.localPosition = VectorHelper.GetVectorWith(coordToModify, currentBlock.transform.localPosition, value);
		pooledObject.GetComponent<MeshRenderer>().material = blockMat;
		currentBlock.transform.localPosition = VectorHelper.GetVectorWith(coordToModify, currentBlock.transform.localPosition, num7);
		currentBlock.transform.localScale = VectorHelper.GetVectorWith(coordToModify, currentBlock.transform.lossyScale, num5);
		pooledObject.transform.localRotation = currentBlock.transform.localRotation;
		currentBlock.GetComponent<BlockScript>().enabled = false;
		if (xOrZ)
		{
			MeshUVAdjuster.AjustUVsforpiecesFromX(baseBlock, pooledObject.transform, currentBlock.transform, num3 < num);
		}
		else
		{
			MeshUVAdjuster.AdjustUVsforpiecesFromZ(baseBlock, currentBlock.transform, pooledObject.transform, num3 < num);
		}
		baseBlock = currentBlock.transform;
	}

	public void ContinueGame()
	{
		StartCoroutine(ContinueGameCoruoutine());
	}

	private IEnumerator ContinueGameCoruoutine()
	{
		alreadyContinued = true;
		float xScale = continueBlockSize - currentBlock.transform.lossyScale.x;
		float zScale = continueBlockSize - currentBlock.transform.lossyScale.z;
		int steps = 3;
		for (int i = 0; i < steps; i++)
		{
			if (i < steps - 1)
			{
				SoundManager.Instance.playSound(AudioClips.split);
			}
			else
			{
				SoundManager.Instance.playSound(AudioClips.perfect);
			}
			yield return new WaitForSeconds(0.1f);
			GameObject pooledObject = objectPooler.GetPooledObject(ObjectPoolItems.Block, activeState: true);
			pooledObject.transform.SetParent(stack);
			pooledObject.GetComponent<BlockScript>().enabled = false;
			pooledObject.transform.localScale = baseBlock.transform.localScale + new Vector3(xScale / (float)steps, 0f, zScale / (float)steps);
			pooledObject.transform.localPosition = baseBlock.transform.localPosition + Vector3.up * yOffset;
			Vector3 vector = new Vector3(baseBlock.localPosition.x + baseBlock.lossyScale.x / 2f, 0f, baseBlock.localPosition.z + baseBlock.lossyScale.z / 2f) - new Vector3(pooledObject.transform.localPosition.x + pooledObject.transform.lossyScale.x / 2f, 0f, pooledObject.transform.localPosition.z + pooledObject.transform.lossyScale.z / 2f);
			pooledObject.transform.localPosition -= vector;
			blockMat = new Material(Shader.Find(shaderType));
			blockMat.SetColor("_Color", colorController.BlockColor);
			blockMat.mainTexture = selectedSkinTexture;
			pooledObject.GetComponent<MeshRenderer>().material = blockMat;
			MeshUVAdjuster.AdjustUVs(pooledObject.transform);
			cameraController.UpdatePos();
			baseBlock = pooledObject.transform;
		}
		isGameRunning = true;
		Invoke("ActivateInputWithDelay", 0.2f);
		BringNewBlock();
	}
}
