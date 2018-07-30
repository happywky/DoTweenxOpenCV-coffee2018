using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using DG.Tweening;

public class sceneControllor : MonoBehaviour {

    public GameObject baseLayerOfVideo;           //背景影片Plane
    public GameObject grayCoverLayer;            //子影片播放時的遮擋plane with Image control by DOfade

    public GameObject[] sectionVideoPlayer;     //子影片播放用的plane
    public GameObject[] scanAreaCylinder;       //用於掃描定位的圓柱
    public double currentClipPlayedTime;        //用於檢測子影片播放進度
    public double thisClipTime;                 //子影片的長度，添加影片時手動在Update中定

    private bool sectionVideoIsOn = false;      //子影片播放狀態，Upadate中的偵側開關
    private int currentSectionNum;                  //按下鼠標是定義，用於Update中追蹤子影片放映進度
    private long baseLayerPausedAtFrame;            //記錄底面影片暫停時的位置
  

	// Use this for initialization
	void Start () {


        hideSectionVideoGroups();
        StartCoroutine(waitThenStartBGVideo(4f));

	}

    IEnumerator waitThenStartBGVideo(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        baseLayerOfVideo.GetComponent<VideoPlayer>().Play();

        yield return new WaitForSeconds(1f);
       
        fadeoutGrayCover();
        baseLayerOfVideo.GetComponent<MeshRenderer>().materials[0].color = Color.white;

    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetMouseButton(0)) 
        { 
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.name == "DetectArea00") showPlayThenGray(0, 13f);
                if (hit.transform.name == "DetectArea01") showPlayThenGray(1, 26f);
                if (hit.transform.name == "DetectArea02") showPlayThenGray(2, 20f);
                if (hit.transform.name == "DetectArea03") showPlayThenGray(3, 27f);
                if (hit.transform.name == "DetectArea04") showPlayThenGray(4, 15f);

                
            }
        }
        if (sectionVideoIsOn)
        {

            currentClipPlayedTime = sectionVideoPlayer[currentSectionNum].GetComponent<VideoPlayer>().time;
            if (currentClipPlayedTime >= thisClipTime)
            {
                showDetectAreaAll();
                hideSectionVideoGroups();
                //baseLayerOfVideo.GetComponent<VideoPlayer>().frame = 0;
                baseLayerOfVideo.GetComponent<VideoPlayer>().frame = baseLayerPausedAtFrame;
                baseLayerOfVideo.GetComponent<VideoPlayer>().Play();

                fadeoutGrayCover();
                sectionVideoIsOn = false;
            }
        }
      
	}


    public void hideSectionVideoGroups()
    {
        for (int i = 0; i < sectionVideoPlayer.Length; i++)
        {
            sectionVideoPlayer[i].SetActive(false);
        }
    }

    public void showPlayThenGray(int Num, float VCRTime)
    {

        //showGrayCover();
        fadeinGrayCover();

        currentSectionNum = Num;
        currentClipPlayedTime = 0;
        baseLayerPausedAtFrame = baseLayerOfVideo.GetComponent<VideoPlayer>().frame;
        baseLayerOfVideo.GetComponent<VideoPlayer>().Pause();
        
        sectionVideoPlayer[Num].SetActive(true);
        thisClipTime = sectionVideoPlayer[Num].GetComponent<VideoPlayer>().clip.length;

        sectionVideoPlayer[Num].GetComponent<VideoPlayer>().Play();
        hideDetectAreaAll();
        sectionVideoIsOn = true;


    }

    public void hideDetectAreaAll()
    {
        for (int i = 0; i < sectionVideoPlayer.Length; i++)
        {
            scanAreaCylinder[i].SetActive(false);
        }
    }

    public void showDetectAreaAll()
    {
        for (int i = 0; i < sectionVideoPlayer.Length; i++)
        {
            scanAreaCylinder[i].SetActive(true);
        }
    }

    public void showGrayCover()
    {
        grayCoverLayer.SetActive(true);


    }
    public void hideGrayCover()
    {
        // call on click cylender and both fade tween
        grayCoverLayer.SetActive(false);
    }
    public void fadeinGrayCover()
    {
        grayCoverLayer.GetComponent<MeshRenderer>().materials[0].DOFade(1f, 0.8f);
    }

    public void fadeoutGrayCover()
    {
        grayCoverLayer.GetComponent<MeshRenderer>().materials[0].DOFade(0f, 0.8f);

    }
}
