using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonTexChange : MonoBehaviour 
{
    public GameObject Image1;
    public GameObject Image2;
    
	/// <summary>
    /// 设置图片状态
    /// </summary>
    /// <param name="status">0普通状态，1激活状态</param>
    public void Set(int status)
    {
        Image1.SetActive(status == 0);
        Image2.SetActive(status == 1);
    }
}
