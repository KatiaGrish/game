using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class ItemPickup : MonoBehaviour
{
    public bool isMoney = true;
    public int amount = 1;
    public Image levelCompleteImage;

    // Статическая переменная для хранения собранных монет
    public static int coinsCollected = 0;
    public const int coinsRequiredToComplete = 5;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (isMoney)
            {
                // Воспроизводим звук сбора монетки
                AudioSource audioSource = GetComponent<AudioSource>();
                if (audioSource != null)
                {
                    audioSource.Stop(); // Останавливаем предыдущий звук
                    audioSource.Play(); // Воспроизводим новый
                }

                // Увеличиваем счетчик собранных монет
                coinsCollected += amount;
                Debug.Log($"Собрано монет: {coinsCollected}/{coinsRequiredToComplete}");

                // Проверяем собраны ли все необходимые монеты
                if(coinsCollected >= coinsRequiredToComplete)
                {
                    CompleteLevel();
                }
            }

            // Отключаем визуальную часть монетки, но оставляем объект для воспроизведения звука
            GetComponent<MeshRenderer>().enabled = false;
            GetComponent<Collider>().enabled = false;

            // Уничтожаем объект после завершения звука
            if (GetComponent<AudioSource>() != null && GetComponent<AudioSource>().isPlaying)
            {
                Destroy(gameObject, GetComponent<AudioSource>().clip.length);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    void CompleteLevel()
    {
        Debug.Log("Уровень завершен! Собрано 5 монет!");

        // Показываем изображение финала
        if(levelCompleteImage != null)
        {
            levelCompleteImage.gameObject.SetActive(true);
        }

        // Останавливаем время в игре
        Time.timeScale = 0f;

        StartCoroutine(LoadNextLevelAfterDelay(3f));
    }

    IEnumerator LoadNextLevelAfterDelay(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    // Сброс счетчиков при загрузке новой сцены
    public static void ResetCoinCounters()
    {
        coinsCollected = 0;
    }
}