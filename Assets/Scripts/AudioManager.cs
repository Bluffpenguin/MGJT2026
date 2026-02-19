using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("Music Setting")]
    [SerializeField] AudioSource m_Source;

    [Header("UI Sound Settings")]
    [SerializeField] AudioSource ui_Source;

    [Header("Dialog Sound Settings")]
    [SerializeField] AudioSource typewritter_Source;
	// Start is called once before the first execution of Update after the MonoBehaviour is created
	private void Awake()
	{
        instance = this;
	}

    void Start()
    {
        m_Source.Play();
    }
    public void UI_One_Shot(AudioClip clip,float vol)
    {
        ui_Source.PlayOneShot(clip, vol);
    }

    public void UI_Change_Volume(float vol)
    {
        ui_Source.volume = vol;
    }

    public void Music_Switch(AudioClip track)
    {
        m_Source.generator = track;
    }

    public void Music_Play()
    {
        m_Source.Play();

    }

    public void Music_Stop()
    {
        m_Source.Stop();
    }

    public void M_Change_Volume(float volume)
    {
        m_Source.volume = volume;
    }

    public void TW_Checker()
    {
        if (!typewritter_Source.isPlaying)
        {
			typewritter_Source.Play();
		}
        
    }

    public void TW_Stop()
    {
        typewritter_Source.Stop();
    }

    public void TW_Change_Volume(float vol)
    {
        typewritter_Source.volume = vol;
    }
}
