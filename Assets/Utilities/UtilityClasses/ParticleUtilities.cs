using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ParticleUtilities
{
	public static void SetParticlesRotation(ParticleSystem particles, float rotationMin, float rotationMax)
	{
		var main = particles.main;
		var startRotation = main.startRotation;
		if (rotationMin == rotationMax)
		{
			startRotation.mode = ParticleSystemCurveMode.Constant;
			startRotation.constant = rotationMin;
		}
		else
		{
			startRotation.mode = ParticleSystemCurveMode.TwoConstants;
			startRotation.constantMin = rotationMin;
			startRotation.constantMax = rotationMax;
		}
		main.startRotation = startRotation;
	}

	public static void SetParticleDuration(ParticleSystem particles, float duration)
	{
		var main = particles.main;
		main.duration = duration;
	}

	public static void SetParticlesColor(ParticleSystem particles, Color color)
	{
		var main = particles.main;
		var startColor = main.startColor;
		startColor.color = color;
		main.startColor = startColor;
	}

	public static void SetParticlesSize(ParticleSystem particles, float sizeMin, float sizeMax)
	{
		var main = particles.main;
		var startSize = main.startSize;
		if (sizeMin == sizeMax)
		{
			startSize.mode = ParticleSystemCurveMode.Constant;
			startSize.constant = sizeMin;
		}
		else
		{
			startSize.mode = ParticleSystemCurveMode.TwoConstants;
			startSize.constantMin = sizeMin;
			startSize.constantMax = sizeMax;
		}
		main.startSize = startSize;
	}

	public static void SetParticleCircleShape(ParticleSystem particles, float radius, float radiusThickness)
	{
		var shape = particles.shape;
		shape.radius = radius;
		shape.radiusThickness = radiusThickness;
	}

	public static void SetParticlesEmissionBurst(ParticleSystem particles, float time, short count, short cycles, float interval)
	{
		var emission = particles.emission;
		emission.burstCount = 1;
		emission.SetBurst(0, new ParticleSystem.Burst(time, count, count, cycles, interval));
	}
}
