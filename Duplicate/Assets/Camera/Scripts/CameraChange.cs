using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraChange : MonoBehaviour
{

    public bool swap;

    public bool swapback;

    public Animator animators;
    public List<Cinemachine.CinemachineVirtualCamera> regularVCameras;
    public List<Cinemachine.CinemachineVirtualCamera> finalVCameras;
    public List<GameObject> followCameras;
    public GameObject finalCamera;

    void Update() {
        if(swap) {
            swap = false;
            animators.SetTrigger("Full");
            finalVCameras.ForEach(n => n.Priority = 101);
            regularVCameras.ForEach(n => n.Priority = 99);
        }

        if(swapback) {
            swapback = false;
            finalCamera.SetActive(false);
            regularVCameras.ForEach(n => n.Priority = 101);
            finalVCameras.ForEach(n => n.Priority = 99);
        }
    }

    public void Final() {
        finalCamera.SetActive(true);
    }
}