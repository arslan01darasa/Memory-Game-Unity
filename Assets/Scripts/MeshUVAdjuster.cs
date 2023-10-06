using UnityEngine;

public class MeshUVAdjuster : MonoBehaviour
{
	public static float scaleFactor = 3f;

	public static void CopyUVs(Transform copyFrom, Transform copyTo)
	{
		Mesh mesh = copyFrom.GetComponent<MeshFilter>().mesh;
		copyTo.GetComponent<MeshFilter>().mesh.uv = mesh.uv;
	}

	public static void AdjustUVs(Transform _transform)
	{
		Mesh mesh = _transform.GetComponent<MeshFilter>().mesh;
		Vector2[] array = new Vector2[mesh.vertices.Length];
		array[4] = (array[12] = new Vector2(0f, 0f));
		array[5] = (array[15] = new Vector2(0f, _transform.localScale.x / scaleFactor));
		array[8] = (array[13] = new Vector2(_transform.localScale.z / scaleFactor, 0f));
		array[9] = (array[14] = new Vector2(_transform.localScale.z / scaleFactor, _transform.localScale.x / scaleFactor));
		array[16] = (array[23] = new Vector2(0f, _transform.localScale.z / scaleFactor));
		array[17] = (array[22] = new Vector2(_transform.localScale.y / scaleFactor, _transform.localScale.z / scaleFactor));
		array[18] = (array[21] = new Vector2(_transform.localScale.y / scaleFactor, 0f));
		array[19] = (array[20] = new Vector2(0f, 0f));
		array[0] = (array[6] = new Vector2(0f, 0f));
		array[1] = (array[7] = new Vector2(0f, _transform.localScale.x / scaleFactor));
		array[2] = (array[10] = new Vector2(_transform.localScale.y / scaleFactor, 0f));
		array[3] = (array[11] = new Vector2(_transform.localScale.y / scaleFactor, _transform.localScale.x / scaleFactor));
		mesh.uv = array;
	}

	public static void AjustUVsforpiecesFromX(Transform baseBlock, Transform piece1, Transform piece2, bool switchOrder = false)
	{
		Mesh mesh = baseBlock.GetComponent<MeshFilter>().mesh;
		Transform transform = switchOrder ? piece2.transform : piece1.transform;
		Mesh mesh2 = switchOrder ? piece2.GetComponent<MeshFilter>().mesh : piece1.GetComponent<MeshFilter>().mesh;
		Mesh mesh3 = switchOrder ? piece1.GetComponent<MeshFilter>().mesh : piece2.GetComponent<MeshFilter>().mesh;
		Vector2[] uv = mesh.uv;
		float y = (uv[5].y - uv[4].y) / baseBlock.lossyScale.x * transform.lossyScale.x + uv[4].y;
		uv[5] = (uv[15] = new Vector2(uv[5].x, y));
		uv[9] = (uv[14] = new Vector2(uv[9].x, y));
		uv[1] = (uv[7] = new Vector2(uv[1].x, y));
		uv[3] = (uv[11] = new Vector2(uv[3].x, y));
		mesh2.uv = uv;
		uv = mesh.uv;
		uv[4] = (uv[12] = new Vector2(uv[4].x, y));
		uv[8] = (uv[13] = new Vector2(uv[8].x, y));
		uv[0] = (uv[6] = new Vector2(uv[0].x, y));
		uv[2] = (uv[10] = new Vector2(uv[2].x, y));
		mesh3.uv = uv;
	}

	public static void AdjustUVsforpiecesFromZ(Transform baseBlock, Transform piece1, Transform piece2, bool switchOrder = false)
	{
		Mesh mesh = baseBlock.GetComponent<MeshFilter>().mesh;
		Transform transform = switchOrder ? piece2.transform : piece1.transform;
		Mesh mesh2 = switchOrder ? piece2.GetComponent<MeshFilter>().mesh : piece1.GetComponent<MeshFilter>().mesh;
		Mesh mesh3 = switchOrder ? piece1.GetComponent<MeshFilter>().mesh : piece2.GetComponent<MeshFilter>().mesh;
		Vector2[] uv = mesh.uv;
		float num = (uv[8].x - uv[4].x) / baseBlock.lossyScale.z * transform.lossyScale.z + uv[4].x;
		uv[8] = (uv[13] = new Vector2(num, uv[8].y));
		uv[9] = (uv[14] = new Vector2(num, uv[9].y));
		uv[17] = (uv[22] = new Vector2(uv[17].x, num));
		uv[16] = (uv[23] = new Vector2(uv[16].x, num));
		mesh2.uv = uv;
		uv = mesh.uv;
		uv[4] = (uv[12] = new Vector2(num, uv[4].y));
		uv[5] = (uv[15] = new Vector2(num, uv[5].y));
		uv[18] = (uv[21] = new Vector2(uv[18].x, num));
		uv[19] = (uv[20] = new Vector2(uv[19].x, num));
		mesh3.uv = uv;
	}
}
