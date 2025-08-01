using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UndestroyableObjectScript : MonoBehaviour
{
    private void Awake()
    {
        UndestroyableObjectScript[] objects = FindObjectsOfType<UndestroyableObjectScript>();

        if (objects.Length > 2)
        {
            Destroy(gameObject);
            return;
        }

        if (SceneManager.GetActiveScene().buildIndex != 2)
        {
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex != 0 && SceneManager.GetActiveScene().buildIndex != 1)
        {
            Destroy(gameObject);
        }
    }
}
