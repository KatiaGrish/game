using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement; // Было "SceneMenagement" (опечатка)

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth; // Максимальное здоровье
    private int health;   // Текущее здоровье

    void Start()
    {
        health = maxHealth; // Устанавливаем здоровье на максимум при старте
    }

    void Update()
    {
        // Можно добавить проверки здесь, если нужно
    }

    public void DamagePlayer(int damage)
    {
        health -= damage; // Уменьшаем здоровье на урон

        // Проверяем, умер ли игрок
        if (health <= 0)
        {
            Debug.Log("Игрок умер!"); // Было без точки с запятой

            // Перезагружаем текущую сцену
            Scene currentScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(currentScene.buildIndex);
        }
    }
}