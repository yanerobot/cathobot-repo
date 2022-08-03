using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] float pitchInSafeZone, normalPitch;
    [SerializeField] AudioSource src;

    public static AudioManager mngr;

    void Awake()
    {
        if (mngr != null && mngr.src.clip == src.clip)
            Destroy(gameObject);
        else
        {
            if (mngr != null)
                Destroy(mngr.gameObject);
            mngr = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        src.pitch = pitchInSafeZone;

        SafeZone.OnSafeZoneOut.AddListener(NormalizePitch);
    }

    void NormalizePitch()
    {
        src.pitch = normalPitch;
    }
}
