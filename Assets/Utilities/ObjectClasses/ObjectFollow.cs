using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ObjectFollow : Function<ObjectFollow>
{
	public static ObjectFollow Create(Transform objectWhichFollow, Transform objectToFollow, Vector3? offset = null, string name = "")
	{
		ObjectFollow objectFollow = Create(name, false);
		if (offset == null) offset = Vector3.zero;
		objectFollow.offset = (Vector3)offset;
		objectFollow.objectToFollow = objectToFollow;
		objectFollow.objectWhichFollow = objectWhichFollow;
		objectFollow.createdThroughCode = true;
		return null;
	}

	private bool createdThroughCode;

	[SerializeField] Transform objectWhichFollow;
	[SerializeField] Transform objectToFollow;

	[SerializeField] Vector3 offset;

	protected override void UpdateAction()
	{
		if(objectToFollow == null || objectWhichFollow == null)
		{
			Destroy();
		}
		else
		{
			transform.position = objectToFollow.position + offset;
		}
	}

	private void Start()
	{
		if (!createdThroughCode)
		{
			Create(objectWhichFollow, objectToFollow, offset, name);
			objectWhichFollow = null;
			Destroy();
			return;
		}
		else
		{
			transform.parent = objectWhichFollow.parent;
			objectWhichFollow.parent = transform;
		}
	}

	public override void Destroy()
	{
		if (objectWhichFollow != null)
		{
			transform.AttachChildrenToParent();
		}
		base.Destroy();
	}
}
