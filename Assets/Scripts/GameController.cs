using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {
    public string[] scenelist;
    public GameObject playerPrefab;

    public PlayerController.PlayerData data { get; private set; }
    private Scene startScene;
    public PlayerController player { get; private set; }

    void Awake() {
        Instance = this;
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoad;
        GameObject go = GameObject.Find("Player");
        if (go != null) {
            player = go.GetComponent<PlayerController>();
            data = player.data;
        }
        startScene = SceneManager.GetActiveScene();
    }

    void OnSceneLoad(Scene scene, LoadSceneMode mode) {
        if (scene != startScene) {
            GameObject sp = GameObject.Find("PlayerSpawn");
            GameObject go = Instantiate(playerPrefab, sp.transform.position, Quaternion.identity);
            player = go.GetComponent<PlayerController>();
            player.data = data;
        }

        foreach (LevelTrigger lt in GameObject.FindObjectsOfType<LevelTrigger>()) {
            if (!lt.isActiveAndEnabled) continue;
            lt.onHit = scenelist[UnityEngine.Random.Range(0, scenelist.Length - 1)];
        }
    }

    void OnDisable() {
        SceneManager.sceneLoaded -= OnSceneLoad;
    }

    public void KillPlayer() {
        Destroy(player.gameObject);
        player = null;
    }

    public static GameController Instance { get; private set; }
}
