using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Polygon : MonoBehaviour
{
	private List<Vector2> corners;

	public List<Vector2> GetCorners()
	{
		if (corners == null)
			return new List<Vector2>();
		else
			return new List<Vector2>(corners);
	}

	protected void SetCorners(List<Vector2> newCorners)
	{
		corners = newCorners;
	}
}
