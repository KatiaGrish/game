using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class ItemPickup : MonoBehaviour
{
    public bool isMoney = true;
    public int amount = 1;
    public Image levelCompleteImage;

    public static int coinsCollected = 0;
    public const int coinsRequiredToComplete = 5;

    private CanvasManager canvasManager;

    private void Start()
    {
        // Находим CanvasManager в сцене
        canvasManager = FindObjectOfType<CanvasManager>();
        
        // Обновляем UI при старте (если монеты уже есть)
        if (canvasManager != null)
        {
            canvasManager.UpdateMoneyText(coinsCollected);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && isMoney)
        {
            // Звук сбора
            AudioSource audioSource = GetComponent<AudioSource>();
            if (audioSource != null)
            {
                audioSource.Play();
            }

            // Увеличиваем счётчик
            coinsCollected += amount;

            // Обновляем текст на UI
            if (canvasManager != null)
            {
                canvasManager.UpdateMoneyText(coinsCollected);
            }

            Debug.Log($"Монет: {coinsCollected}/{coinsRequiredToComplete}");

            // Проверка победы
            if (coinsCollected >= coinsRequiredToComplete)
            {
                CompleteLevel();
            }

            // "Скрываем" монету (но оставляем для звука)
            GetComponent<MeshRenderer>().enabled = false;
            GetComponent<Collider>().enabled = false;

            // Уничтожаем после звука
            if (audioSource != null && audioSource.isPlaying)
            {
                Destroy(gameObject, audioSource.clip.length);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    void CompleteLevel()
    {
        if (levelCompleteImage != null)
        {
            levelCompleteImage.gameObject.SetActive(true);
        }

        Time.timeScale = 0f;
        StartCoroutine(LoadNextLevelAfterDelay(3f));
    }

    IEnumerator LoadNextLevelAfterDelay(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public static void ResetCoinCounters()
    {
        coinsCollected = 0;
    }
}