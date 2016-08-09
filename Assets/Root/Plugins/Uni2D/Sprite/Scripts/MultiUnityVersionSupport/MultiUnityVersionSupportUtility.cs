#if UNITY_3_5 || UNITY_4_0 || UNITY_4_0_1 || UNITY_4_1 || UNITY_4_2
#define BEFORE_UNITY_4_3
#else
#define AFTER_UNITY_4_3
#endif

#if UNITY_3_5 || UNITY_4_0 || UNITY_4_0_1 || UNITY_4_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_4 || UNITY_4_5 || UNITY_4_6
#define BEFORE_UNITY_5
#else
#define AFTER_UNITY_5
#endif

using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public static class MultiUnityVersionSupportUtility
{

#if UNITY_EDITOR
	public static bool InAnimationMode()
	{
		#if AFTER_UNITY_4_3
		return AnimationMode.InAnimationMode();
		#else
		return AnimationUtility.InAnimationMode();
		#endif
	}
	
	public static void UnloadUnusedAssets()
	{
		#if AFTER_UNITY_5
		EditorUtility.UnloadUnusedAssetsImmediate();
		#else
		EditorUtility.UnloadUnusedAssets();
		#endif
	}
#endif
}
