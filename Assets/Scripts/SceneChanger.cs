using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    PlayerCharacter player;
    public string nextScene;
    public Animator transition;
    public float transitionTime = 1f;
    void Start()
    {
        player = FindObjectOfType<PlayerCharacter>();
    }

    void Update()
    {
        LoseScene();
    }

    public void LoseScene()
    {
        if (player.health <= 0f)
        {
            Cursor.lockState = CursorLockMode.None;
            SceneManager.LoadScene("Lose");
            player.health += 100f;
        }
    }

    public void OnTriggerEnter(Collider col)
    {
        Cursor.lockState = CursorLockMode.None;
        
        PlayerCharacter player = col.GetComponent<PlayerCharacter>();
        if (player != null) StartCoroutine(LoadAnim());

        IEnumerator LoadAnim()
        {
            transition.SetTrigger("Start");

            yield return new WaitForSeconds(transitionTime);
            SceneManager.LoadScene(nextScene);

        }

    }

}
