using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using DG.Tweening;

public class sceneControllor : MonoBehaviour {

    public GameObject baseLayerOfVideo;
    public GameObject grayCoverLayer;

    public GameObject[] sectionVideoPlayer;
    public GameObject[] scanAreaCylinder;
    public double currentClipPlayedTime;
    public double thisClipTime;

    private bool sectionVideoIsOn = false;
    private bool fadeStateOfGrayCover = false;
    private int currentSectionNum;
    private Color colorStart, colorEnd;

	// Use this for initialization
	void Start () {
        colorStart = grayCoverLayer.GetComponent<Renderer>().material.color;
        colorEnd = new Color(colorStart.r, colorStart.g, colorStart.b, 0.0f);

        hideSectionVideoGroups();
        hideGrayCover();

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
                baseLayerOfVideo.GetComponent<VideoPlayer>().frame = 0;
                baseLayerOfVideo.GetComponent<VideoPlayer>().Play();
                fadeStateOfGrayCover = true;
                sectionVideoIsOn = false;
            }
        }
        if(grayCoverLayer.activeInHierarchy && !fadeStateOfGrayCover)
        {
            //fadeStateOfGrayCover: true means faded, false means ready to leave fade state;
            Debug.Log("!fadestateOfGrayCover");
            for (float t = 0f; t < 1f; t += Time.deltaTime)
            {
                grayCoverLayer.GetComponent<Renderer>().material.color = Color.Lerp(colorStart, colorEnd, (1 - t));
            }
            fadeStateOfGrayCover = false;
        }

        if (grayCoverLayer.activeInHierarchy && fadeStateOfGrayCover)
        {
            for (float t = 0f; t < 1f; t += Time.deltaTime)
            {
                grayCoverLayer.GetComponent<Renderer>().material.color = Color.Lerp(colorStart, colorEnd, t);
                Debug.Log("t = " + t);
            }
            Debug.Log("fadestateOfGrayCover");
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

        showGrayCover();
        currentSectionNum = Num;
        currentClipPlayedTime = 0;
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
        Debug.Log("call hideGrayCover once");
    }
}
