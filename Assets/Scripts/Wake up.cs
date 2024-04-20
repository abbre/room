using System.Collections;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

public class Wakeup : MonoBehaviour
{
    public GameObject firstCameraController;
    public Camera mainCamera;
    public Camera sleepCamera;
    public Image maskImage; // 遮罩的 Image UI 元素
    [CanBeNull]public AudioClip wakeupSound; // 醒来时播放的音频4
    private AudioSource stepSound;

    [SerializeField]private AudioClip footStep1;


    private bool audioEnd = false;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(FadeMask());

        stepSound = GetComponent<AudioSource>();
    }

    void Update()
    {
        
        if ((wakeupSound != null && audioEnd) || wakeupSound == null)
        {
            Color currentColor = maskImage.color;
            currentColor.a -= Time.deltaTime * 2f;
            maskImage.color = currentColor;

            if (maskImage.color.a <= 0)
            {
                maskImage.enabled = false; // 遮罩完全透明时禁用遮罩
            }
        }
    }

    IEnumerator FadeMask()
    {
        if (wakeupSound != null && audioSource != null)
        {
            audioSource.clip = wakeupSound;
            audioSource.Play();
            yield return new WaitForSeconds(audioSource.clip.length - 1);
        }
        else
        {
            yield return new WaitForSeconds(2f);
        }

        // 等待音频播放结束
      

        audioEnd = true;

        // 切换 Camera 的逻辑
        mainCamera.enabled = true;
        sleepCamera.enabled = false;

        if (firstCameraController != null)
        {
            firstCameraController.SetActive(true);
            stepSound.PlayOneShot(footStep1);
        }
    }
}
