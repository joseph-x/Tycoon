using UnityEngine;
using System.Collections;

public static class ExtensionMethod
{
	private const float dotThreshold = 0.5f;

	public static bool IsFacingTarget(this Transform transform, Transform target)
	{
		var vectorToTarget = target.position - transform.position;
		vectorToTarget.Normalize();

		float dot = Vector3.Dot(transform.forward, vectorToTarget);

		if (dot >= dotThreshold)
		{
			return true;
		}

		return false;
	}
}

