using UnityEngine;
using System.Collections;

public class Timer
{

	float actTime;
	bool isActive;

	public void update (float deltaTime)
	{
		if (isActive) {
			actTime += deltaTime;
		}
	}

	public void start ()
	{
		isActive = true;
	}

	public void stop ()
	{
		isActive = false;
	}

	public Timer ()
	{
		isActive = true;
		actTime = 0;
	}

	public float getTime ()
	{
		return actTime;
	}

	public void setTime (float newTime)
	{
		actTime = newTime;
	}

	public bool getIsActive ()
	{
		return isActive;
	}

	public void reset ()
	{
		actTime = 0f;
	}
}

