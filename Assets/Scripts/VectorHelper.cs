using UnityEngine;

public class VectorHelper
{
	public enum Vector3Coord
	{
		x,
		y,
		z
	}

	public static Vector3 GetXVector(Vector3 vector)
	{
		return new Vector3(vector.x, 0f, 0f);
	}

	public static Vector3 GetZVector(Vector3 vector)
	{
		return new Vector3(0f, 0f, vector.z);
	}

	public static Vector3 GetVectorWith(Vector3Coord coordToModify, Vector3 vectorToModify, float value)
	{
		Vector3 result = vectorToModify;
		switch (coordToModify)
		{
		case Vector3Coord.x:
			result.x = value;
			break;
		case Vector3Coord.y:
			result.y = value;
			break;
		case Vector3Coord.z:
			result.z = value;
			break;
		}
		return result;
	}

	public static void CopyTransformProperties(Transform to, Transform from)
	{
		to.transform.position = from.transform.position;
		to.transform.rotation = from.transform.rotation;
		to.transform.localScale = from.transform.localScale;
	}
}
