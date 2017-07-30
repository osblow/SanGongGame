using System;
using System.IO;
using System.Collections;
using UnityEngine;

public class MicrophoneMng : MonoBehaviour
{
    public const int RECORD_RATE = 16000;


    //private static string[] micArray = null; //录音设备列表
    private AudioClip audioClip;
    private DateTime beginTime;

    public float sensitivity = 100;
    public float loudness = 0;

    const int HEADER_SIZE = 44;
    const int RECORD_TIME = 10;
    //const int RECORD_RATE = 6000;//44100; //录音采样率
    

    /// <summary>
    /// 开始录音
    /// </summary>
    public void StartRecord(System.Action<AudioClip> callback)
    {
        


        //if (micArray.Length == 0)
        //{
        //    Debug.Log("No Record Device!");
        //    return;
        //}
        Microphone.End(null);//录音时先停掉录音，录音参数为null时采用默认的录音驱动
        beginTime = DateTime.Now;
        audioClip = Microphone.Start(null, false, RECORD_TIME, RECORD_RATE);
        while (!(Microphone.GetPosition(null) > 0))
        {
        }
        if(null == callback)
        {
            callback.Invoke(audioClip);
        }

        Debug.Log("StartRecord");
    }

    /// <summary>
    /// 停止录音
    /// </summary>
    public void StopRecord(Action<AudioClip> callback)
    {
        //if (micArray.Length == 0)
        //{
        //    Debug.Log("No Record Device!");
        //    return;
        //}
        if (!Microphone.IsRecording(null))
        {
            return;
        }
        Microphone.End(null);

        if(null != callback)
        {
            callback.Invoke(audioClip);
        }
        Debug.Log("StopRecord");
    }


    //public static void SetData(this AudioClip clip, byte[] bytes)
    //{
    //    bytes = SVZip.ConvertBytesZlib(bytes, Ionic.Zlib.CompressionMode.Decompress);

    //    float[] data = new float[bytes.Length / 4];
    //    Buffer.BlockCopy(bytes, 0, data, 0, data.Length);

    //    clip.SetData(data, 0);
    //}

//    public static byte[] ConvertBytesZlib(byte[] data, CompressionMode compressionMode)
//    {
//        CompressionMode mode = compressionMode;
//        if (mode != CompressionMode.Compress)
//        {
//            if (mode != CompressionMode.Decompress)
//            {
//                throw new NotImplementedException();
//            }
//            return ZlibStream.UncompressBuffer(data);
//        }
//        return ZlibStream.CompressBuffer(data);
//    }
}
