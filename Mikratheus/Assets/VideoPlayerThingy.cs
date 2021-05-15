using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class VideoPlayerThingy : MonoBehaviour
{
    public VideoPlayer videoPlayer;

    private void Start()
    {
        StartCoroutine(SceneChangeRoutine());
    }

    public IEnumerator SceneChangeRoutine()
    {
        while (!Input.GetKey(KeyCode.A))
        {
            yield return new WaitForEndOfFrame();
        }

        videoPlayer.Play();

        yield return new WaitForSeconds(1);
        while (videoPlayer.isPlaying)
        {
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("MainScene");
    }
}
