using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTrigger : MonoBehaviour {
    public string onHit;

    void OnTriggerEnter2D(Collider2D collider) {
        if (collider.CompareTag("Player")) {
            SceneManager.LoadScene(onHit);
        }
    }
}
