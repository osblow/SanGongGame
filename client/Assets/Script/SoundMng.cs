using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Osblow.Util;


namespace Osblow.App
{
    public class SoundMng : MonoBehaviour
    {
        class FrontSound
        {
            public bool bEnable = false;        //当前音源是否有效
            public AudioSource aS = null;        //音源
            public float startTime = 0;          //开始播放的时间
            public float playTime = 0;           //需要播放的时间
        }

        AudioListener mListener;                 //音乐接收器

        bool m_frontSoundLoaded = false;         //是否加载前景音乐音量设置
        float m_frontSoundVolume = 1;            //前景音量大小参数
        bool m_backSoundLoaded = false;                   //是否加载背景音乐音量设置
        float m_backSoundVolume = 1f;               //背景音量大小参数
        AudioSource backSound = null;           //背景音乐播放(只会有一个)

        static readonly int MAXFRONTSOUND = 5;  //最多同时播放5个音效
        FrontSound[] arrFrondSound = new FrontSound[MAXFRONTSOUND];     //前景音乐数据


        public void Update()
        {
            for (int i = 0; i < MAXFRONTSOUND; ++i)
            {
                if (arrFrondSound[i].bEnable)
                {
                    if (((Time.time - arrFrondSound[i].startTime) > arrFrondSound[i].playTime
                         && arrFrondSound[i].aS.loop == false))
                    {//播放结束
                        arrFrondSound[i].aS.Stop();
                        arrFrondSound[i].aS.enabled = false;
                        arrFrondSound[i].bEnable = false;
                    }
                    if (arrFrondSound[i] == null)
                    {
                        arrFrondSound[i].bEnable = false;
                    }
                }
            }
        }

        public void Start()
        {
            if (mListener == null)
            {
                mListener = GameObject.FindObjectOfType(typeof(AudioListener)) as AudioListener;
                if (mListener == null)
                {
                    Camera cam = Camera.main;

                    if (cam == null) cam = GameObject.FindObjectOfType(typeof(Camera)) as Camera;
                    if (cam != null) mListener = cam.gameObject.AddComponent<AudioListener>();
                }
            }
            if (mListener != null)
            {//添加音源
                backSound = mListener.gameObject.AddComponent<AudioSource>();
                backSound.Stop();

                for (int i = 0; i < MAXFRONTSOUND; ++i)
                {
                    arrFrondSound[i] = new FrontSound();
                    arrFrondSound[i].aS = mListener.gameObject.AddComponent<AudioSource>();
                    arrFrondSound[i].aS.enabled = false;
                    arrFrondSound[i].bEnable = false;
                    arrFrondSound[i].startTime = 0;
                    arrFrondSound[i].playTime = 0;
                }
            }

        }

        FrontSound GetUseFrontSound()
        {
            for (int i = 0; i < MAXFRONTSOUND; ++i)
            {
                if (!arrFrondSound[i].bEnable)
                {
                    return arrFrondSound[i];
                }
            }
            Debug.LogError("Sound Count out of MAXFRONTSOUND!");
            return null;
        }

        /// <summary>
        /// 设置前景音量
        /// </summary>
        public float FrontSoundVolume
        {
            get
            {
                if (!m_frontSoundLoaded)
                {
                    m_frontSoundLoaded = true;
                    m_frontSoundVolume = PlayerPrefs.GetFloat("FrontSound", 1f);
                }
                return m_frontSoundVolume;
            }
            set
            {
                if (m_frontSoundVolume != value)
                {
                    m_frontSoundLoaded = true;
                    m_frontSoundVolume = value;
                    PlayerPrefs.SetFloat("FrontSound", value);

                    
                }
            }
        }

        /// <summary>
        /// 设置背景音量
        /// </summary>
        public float BackSoundVolume
        {
            get
            {
                if (!m_backSoundLoaded)
                {
                    m_backSoundLoaded = true;
                    m_backSoundVolume = PlayerPrefs.GetFloat("BackSound", 1f);
                }
                return m_backSoundVolume;
            }
            set
            {
                if (m_backSoundVolume != value)
                {
                    m_backSoundLoaded = true;
                    m_backSoundVolume = value;
                    PlayerPrefs.SetFloat("BackSound", value);

                    if (m_backSoundVolume == 0)
                    {
                        backSound.Stop();
                    }
                    else
                    {
                        backSound.volume = BackSoundVolume * 0.4f;
                        if(!backSound.isPlaying) backSound.Play();
                    }
                }
            }
        }
        //播放背景音乐 是否循环播放
        public void PlayBackSound(string _name, bool isLoop)
        {
            string path =  _name;
            AudioClip ac = Resources.Load(path) as AudioClip;
            if (ac == null)
            {
                Debug.LogError("Back Music Not Found:" + path);
                return;
            }
            if (isLoop == true)
            {
                backSound.loop = true;
            }
            else
            {
                backSound.loop = false;
            }
            backSound.Stop();
            backSound.volume = BackSoundVolume * 0.4f;
            backSound.pitch = 1.0f;
            backSound.clip = ac;
            backSound.Play();
        }

        public void StopBackSound()
        {
            if(null != backSound)
            {
                backSound.Stop();
            }
        }

        public void PlayCommonButtonSound()
        {
            PlayFrontSound("Audio/button");
        }

        //播放一个音效 名字 次数 音量 音速
        public void PlayFrontSound(string _name, bool Isloop = false, float volume = 1.0f, float pitch = 1.0f)
        {
            Debug.Log(_name);

            volume *= FrontSoundVolume;
            FrontSound fs = GetUseFrontSound();
            if (fs == null)
            {
                Debug.Log("aaaaaaaaaaaa");
                return;
            }

            string path = _name;


            AudioClip ac = Resources.Load(path) as AudioClip;
            if (ac == null)
            {
                Debug.LogError("Front Music Not Found:" + path);
                return;
            }
            fs.aS.enabled = true;
            fs.aS.pitch = pitch;
            if (!Isloop)
            {//一次
                fs.aS.PlayOneShot(ac, volume);
                fs.aS.loop = Isloop;
            }
            else
            {//多次
                fs.aS.loop = Isloop;
                fs.aS.clip = ac;
                fs.aS.volume = volume;
                fs.aS.Play();
            }
            fs.startTime = Time.time;
            fs.playTime = ac.length;
            fs.bEnable = true;
        }

        public void PlayFrontSound(byte[] data, string uuid, bool Isloop = false, float volume = 10.0f, float pitch = 1.0f)
        {
            Debug.Log("语音消息");

            volume *= FrontSoundVolume;
            FrontSound fs = GetUseFrontSound();
            if (fs == null)
            {
                Debug.Log("aaaaaaaaaaaa");
                return;
            }

            short[] samples = new short[data.Length / 2];
            for (int i = 0; i < samples.Length; i++)
            {
                samples[i] = System.BitConverter.ToInt16(data, i * 2);
            }

            float[] finalSamples = CompressUtil.PreDecompressAudio(samples);

            AudioClip ac = AudioClip.Create("newVoiceMsg", samples.Length, 1, MicrophoneMng.RECORD_RATE, false);
            
            ac.SetData(finalSamples, 0);
            
            fs.aS.enabled = true;
            fs.aS.pitch = pitch;
            if (!Isloop)
            {//一次
                fs.aS.PlayOneShot(ac, volume);
                fs.aS.loop = Isloop;
            }
            else
            {//多次
                fs.aS.loop = Isloop;
                fs.aS.clip = ac;
                fs.aS.volume = volume;
                fs.aS.Play();
            }
            fs.startTime = Time.time;
            fs.playTime = ac.length;
            fs.bEnable = true;

            MsgMng.Dispatch(MsgType.ShowVoiceImg, uuid, ac.length);
        }

        public void CloseSound(string SoundName)
        {
            for (int i = 0; i < MAXFRONTSOUND; ++i)
            {
                if (arrFrondSound[i].bEnable)
                {
                    if (arrFrondSound[i].aS.clip != null)
                        Debug.Log("   " + arrFrondSound[i].aS.clip.name);
                    if (arrFrondSound[i].aS.clip != null && arrFrondSound[i].aS.clip.name == SoundName)
                    {//播放结束
                        arrFrondSound[i].aS.Stop();
                        arrFrondSound[i].aS.enabled = false;
                        arrFrondSound[i].bEnable = false;
                        arrFrondSound[i].aS.clip = null;
                    }
                }
            }
        }

        public void SoundPuase()
        {
            if (backSound != null)
                backSound.pitch = 0;
            for (int i = 0; i < MAXFRONTSOUND; ++i)
            {
                if (arrFrondSound[i].bEnable)
                {
                    arrFrondSound[i].aS.pitch = 0;
                }
            }
        }
        public void SoundResum()
        {
            if (backSound != null)
                backSound.pitch = 1;
            for (int i = 0; i < MAXFRONTSOUND; ++i)
            {
                if (arrFrondSound[i].bEnable)
                {
                    arrFrondSound[i].aS.pitch = 1;
                }
            }
        }
    }
}
