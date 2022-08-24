using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] float pitchInSafeZone, normalPitch;
    [SerializeField] AudioSource src;

    public static AudioManager mngr;

    SpriteRenderer sr;
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

        LevelStartCountDown.OnCountDownEnd.AddListener(NormalizePitch);
    }

    void NormalizePitch()
    {
        src.pitch = normalPitch;
    }
}
