using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Skins List", menuName = "Stack the Blocks/Skins List", order = 1)]
public class SkinList : ScriptableObject
{
	public List<SkinItemData> skins;
}
