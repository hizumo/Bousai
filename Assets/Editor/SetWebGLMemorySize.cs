using UnityEditor;

public class SetWebGLMemorySize
{
    [MenuItem("Tools/WebGL/Set Memory Size to 1024MB")]
    public static void SetMemorySize()
    {
        PlayerSettings.WebGL.memorySize = 1024; // MB’PˆÊ
        UnityEngine.Debug.Log("WebGL Memory Size set to 1024MB");
    }
}