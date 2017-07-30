using System.IO;
using UnityEngine;

using ICSharpCode.SharpZipLib;
using ICSharpCode.SharpZipLib.GZip;

public class CompressUtil 
{
    public static byte[] Compress(byte[] data)
    {
        MemoryStream ms = new MemoryStream();
        GZipOutputStream gzip = new GZipOutputStream(ms);
        gzip.Write(data, 0, data.Length);
        gzip.Close();
        return ms.ToArray();
    }

    public static byte[] Decompress(byte[] data)
    {
        GZipInputStream gzi = new GZipInputStream(new MemoryStream(data));

        MemoryStream re = new MemoryStream();
        int count = 0;
        byte[] res = new byte[65536];
        while ((count = gzi.Read(res, 0, res.Length)) != 0)
        {
            re.Write(res, 0, count);
        }
        return re.ToArray();
    }

    public static short[] PreCompressAudio(float[] audioData)
    {
        float multiplier = 1000000;
        short[] result = new short[audioData.Length];
        for (int i = 0; i < audioData.Length; i++)
        {
            result[i] = (short)(audioData[i] * multiplier);
        }

        return result;
    }

    public static float[] PreDecompressAudio(short[] audioData)
    {
        float multiplier = 1000000;
        float[] result = new float[audioData.Length];
        for (int i = 0; i < audioData.Length; i++)
        {
            result[i] = (float)(audioData[i] / multiplier);
        }

        return result;
    }
}
