using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PlayerControls : ScriptableObject
{
	[SerializeField] KeyCode Up = KeyCode.None;
	[SerializeField] KeyCode Left = KeyCode.None;
	[SerializeField] KeyCode Down = KeyCode.None;
	[SerializeField] KeyCode Right = KeyCode.None;
	[SerializeField] KeyCode Fire = KeyCode.None;

	public KeyCode up => Up;
	public KeyCode down => Down;
	public KeyCode left => Left;
	public KeyCode right => Right;
	public KeyCode fire => Fire;
}