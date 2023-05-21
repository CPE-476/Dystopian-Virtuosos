using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class Tutorial : MonoBehaviour
{
    public Spine spine;
    private VideoPlayer player;
    public int videoIndex = -1;
    public Image window;
    public RawImage videoImage;
    public GameObject right;
    public GameObject left;
    public GameObject cont;
    public TMPro.TextMeshProUGUI clipName;

    private Image right_image;
    private Image left_image;
    private Button right_button;
    private Button left_button;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<VideoPlayer>();
        right_image = right.GetComponent<Image>();
        left_image = left.GetComponent<Image>();
        right_button = right.GetComponent<Button>();
        left_button = left.GetComponent<Button>();
        cont.SetActive(false);
        
        window.enabled = false;
        clipName.enabled = false;
        videoImage.enabled = false;
        right_image.enabled = false;
        right_button.interactable = false;
        left_image.enabled = false;
        left_button.interactable = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Enable()
    {
        window.enabled = true;
        videoImage.enabled = true;
        clipName.enabled = true;

        right_image.enabled = true;
        right_button.interactable = true;
        left_image.enabled = false;
        left_button.interactable = false;

        videoIndex = -1;
        player.source = VideoSource.Url;
        NextVideo();
    }

    public void Disable()
    {
        window.enabled = false;
        clipName.enabled = false;
        videoImage.enabled = false;
        cont.SetActive(false);
        right_image.enabled = false;
        right_button.interactable = false;
        left_image.enabled = false;
        left_button.interactable = false;
        player.url = "";
    }

    public void NextVideo(){
        videoIndex++;
        checkArrow();
        if (videoIndex < spine.sections[spine.section_index].tutorialVideos.Length){
            player.url = spine.sections[spine.section_index].tutorialVideos[videoIndex].video_path;
            player.Play();
            clipName.text = spine.sections[spine.section_index].tutorialVideos[videoIndex].name;

            if (videoIndex == spine.sections[spine.section_index].tutorialVideos.Length - 1)
                cont.SetActive(true);

        }
    }

    public void PrevVideo(){
        videoIndex--;
        checkArrow();
        if (videoIndex >= 0){
            player.url = spine.sections[spine.section_index].tutorialVideos[videoIndex].video_path;
            player.Play();
            clipName.text = spine.sections[spine.section_index].tutorialVideos[videoIndex].name;
        }
    }

    private void checkArrow(){
        if (videoIndex == 0){
            right_image.enabled = true;
            right_button.interactable = true;
            left_image.enabled = false;
            left_button.interactable = false;
        }
        else if (videoIndex == spine.sections[spine.section_index].tutorialVideos.Length - 1){
            right_image.enabled = false;
            right_button.interactable = false;
            left_image.enabled = true;
            left_button.interactable = true;
        }
        else{
            right_image.enabled = true;
            right_button.interactable = true;
            left_image.enabled = true;
            left_button.interactable = true;
        }
    }
}
