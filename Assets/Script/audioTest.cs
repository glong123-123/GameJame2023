
using UnityEngine;

public class audioTest : MonoBehaviour
{
    //�����ļ�
    public AudioSource music;
    public AudioClip[] MyMusic;

    private void Start()
    {
        music = this.GetComponent<AudioSource>();
    }

    /// <summary>���ŷ�����</summary>
    public void playMusic(int iNumber)
    {
        music.clip = MyMusic[iNumber];
        if (music != null && !music.isPlaying)
        {
            music.Play();
        }
    }

    /// <summary>�ر����ֲ���</summary>
    public void stopMusic()
    {
        if (music != null && music.isPlaying)
        {
            music.Stop();
        }
    }

    /// <summary>��ͣ���ֲ���</summary>
    public void pauseMusic()
    {
        if (music != null && !music.isPlaying)
        {
            music.Pause();
        }
    }

    /// <summary>
    /// ���ò�������
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
