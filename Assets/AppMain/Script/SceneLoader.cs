using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (ItemManager.Instance == null) return;

        // �����A�C�e���������[�h���ĕK�v�ȏ��������s
        foreach (var item in ItemManager.Instance.GetOwnedItems())
        {
            Debug.Log($"�����A�C�e��: {item.type} �����[�h����܂����B");
        }
    }
}

