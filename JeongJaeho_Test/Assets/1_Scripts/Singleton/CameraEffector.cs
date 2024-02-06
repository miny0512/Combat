using Cinemachine;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class CameraEffector : Singleton<CameraEffector>
{
    public Camera MainCam { get; private set; }
    public CinemachineVirtualCamera MainVcam => Camera.main.GetComponentInChildren<CinemachineVirtualCamera>();
    public PostProcessProfile Profile => Camera.main.GetComponent<PostProcessVolume>().profile;
    public Bloom Bloom => Profile.GetSetting<Bloom>();
    public ChromaticAberration ChromaticAberration => Profile.GetSetting<ChromaticAberration>();
    public LensDistortion LensDistortion => Profile.GetSetting<LensDistortion>();

    public IEnumerator ShakeCamera(float duration, float maxIntensity = 1)
    {
        MainVcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = maxIntensity;
        yield return new WaitForSeconds(duration);
        MainVcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 0;
    }
}
