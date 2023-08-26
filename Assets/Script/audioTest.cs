
using UnityEngine;

public class audioTest : MonoBehaviour
{
    //“Ù¿÷Œƒº˛
    public AudioSource music;
    public AudioClip[] MyMusic;

    private void Start()
    {
        music = this.GetComponent<AudioSource>();
    }

    /// <summary>≤•∑≈∑≈“Ù¿÷</summary>
    public void playMusic(int iNumber)
    {
        music.clip = MyMusic[iNumber];
        if (music != null && !music.isPlaying)
        {
            music.Play();
        }
    }

    /// <summary>πÿ±’“Ù¿÷≤•∑≈</summary>
    public void stopMusic()
    {
        if (music != null && music.isPlaying)
        {
            music.Stop();
        }
    }

    /// <summary>‘›Õ£“Ù¿÷≤•∑≈</summary>
    public void pauseMusic()
    {
        if (music != null && !music.isPlaying)
        {
            music.Pause();
        }
    }

    /// <summary>
    /// …Ë÷√≤•∑≈“Ù¡ø
    /// </summary>
    /// <param name="volume"></param>
    public void setMusicVolume(float volume)
    {
        if (music != null && !music.isPlaying)
        {
            music.volume = volume;
        }
    }
}
