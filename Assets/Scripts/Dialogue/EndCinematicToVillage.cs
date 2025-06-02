using UnityEngine;
using UnityEngine.Video;

public class EndCinematicToVillage : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.gameObject.SetActive(false);
            videoPlayer.loopPointReached += OnVideoFinished;
            videoPlayer.Play();
        }
    }

    private void OnVideoFinished (VideoPlayer vp)
    {
        SceneManager.Instance.ChangeScene(4);
    }
}
