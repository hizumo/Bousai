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

        // 所持アイテムをリロードして必要な処理を実行
        foreach (var item in ItemManager.Instance.GetOwnedItems())
        {
            Debug.Log($"所持アイテム: {item.type} がロードされました。");
        }
    }
}

