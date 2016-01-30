using System;
using System.Collections;
using System.Collections.Generic;

public static class Extensions
{
	private static Random _randomGenerator = new Random();  
	public static Random RandomGenerator 
	{ 
		get { return _randomGenerator; }
		set { _randomGenerator = value; }
	}
	
	public static void Shuffle<T>(this IList<T> list)  
	{  
		int n = list.Count;  
		while (n > 1) 
		{  
			n--;  
			int k = _randomGenerator.Next(n + 1);  
			T value = list[k];  
			list[k] = list[n];  
			list[n] = value;  
		}  
	}
	
}