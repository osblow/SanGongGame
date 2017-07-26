#if !UNITY_EDITOR && (UNITY_IOS || UNITY_ANDROID)
#define MOBILE
#endif

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System;

/// <summary>
/// 日志管理器
/// @author 邱洪波
/// @mail littlerbird@gmail.com
/// </summary>
namespace littlerbird.units
{

    internal enum Level : byte
    {
        DEBUG = 0,
        INFO = 1,
        WARN = 2,
        FAIL = 3,
        ERROR = 4,
        MSG = 5//不打印日期,和debug同色
    }

    //更换使用GUI方式,不受相机和遮挡影响
    public class LogManager : MonoBehaviour
    {
        public delegate void Callback();
        public delegate void Callback<T>(T t);
        public delegate string ParseFunc<T>(T t);

        class Cmd
        {
            public string cmd;
            public Callback<object[]> action;
            public string desc;
            public string helpCmd { get { string temp = cmd; while (temp.Length < 12)temp += " "; return temp; } }
        }

        /// <summary>
        /// 是否开启调试
        /// </summary>
        public static bool isDebug
        {
            internal set;
            get;
        }

        public static int Developer = 1;//开发者编号, 由编辑器设置, 防止不同的开发者log信息干扰

        /// <summary>
        /// 是否在使用unity自带的Debug.log
        /// </summary>
        public static bool showNativeOut = true;

        /// <summary>
        /// fps更新时间间隔,单位毫秒
        /// </summary>
        public float updateDelay = 0.5f;

        /// <summary>
        /// 最多存储消息条数,超过将自动删除最老的日志
        /// </summary>
        public static int MaxSize = 100;

        private static TextList txt;
        //private static SpriteRenderer background;

        private int offset = 10, dragOffset = 30;

        private new static GameObject gameObject;

        /// <summary>
        /// 日志面板是否开启
        /// </summary>
        public static bool isOpenPanel
        {
            get;
            internal set;
        }


        internal static string[] colors = new string[] { 
             "ffffffff","00ff00ff","ffa500ff","ffff00ff","ffff00ff","ffffffff"
        };

        /// <summary>
        /// 开启日志功能,不开启将不会在界面中显示日志,但仍然可以在控制台打印消息
        /// </summary>
        public static void openDebug(GameObject root)
        {
            if (isDebug)
                return;
            gameObject = new GameObject("LogManager");
            if (root)
            {
                Transform t = gameObject.transform;
                t.parent = root.transform;
                t.localScale = Vector3.one;
                t.localPosition = Vector3.zero;
                gameObject.layer = root.layer;
            }
            else
            {
                DontDestroyOnLoad(gameObject);
            }
            isDebug = true;
            gameObject.AddComponent<LogManager>().initPanel();
            registeSysCmd();

#if !UNITY_EDITOR
            showNativeOut = false;
            Application.RegisterLogCallback(RegisterSystemLog);
#endif
        }

        internal static string getColor(Level level)
        {
            return "<color=#" + colors[(byte)level] + ">";
        }

        #region cmd
        private static Dictionary<string, Cmd> cmds = new Dictionary<string, Cmd>();
        /// <summary>
        /// 注册带参数命令
        /// </summary>
        /// <param name="_cmd"></param>
        /// <param name="_action"></param>
        /// <param name="_desc"></param>
        public static void registCmd(string _cmd, Callback<object[]> _action, string _desc)
        {
            if (cmds.ContainsKey(_cmd))
            {
                msg(Level.WARN, false, _cmd, "命令已经存在,请更换其它命令");
                return;
            }
            cmds[_cmd] = new Cmd() { cmd = _cmd, action = _action, desc = _desc };
        }

        /// <summary>
        /// 注册无参数命令
        /// </summary>
        /// <param name="_cmd"></param>
        /// <param name="_action"></param>
        /// <param name="_desc"></param>
        public static void registCmd(string _cmd, Callback _action, string _desc)
        {
            registCmd(_cmd, (object[] args) => { _action(); }, _desc);
        }

        private static void registeSysCmd()
        {
            registCmd("screen", () => { msg("width:", Screen.width, "height:", Screen.height); }, "无参数,显示设备分辨率");
            registCmd("cls", (object[] args) => { txt.clear(); }, "清空日志");
            registCmd("obj", (object[] args) =>
            {
                string name = (string)args[0];
                GameObject obj = GameObject.Find(name);
                if (obj)
                {
                    msg(PublicUnits.toString(obj));
                }
                else
                    warn(Level.WARN, false, "can't find gameobject of name ", name);
            }, "参数为GameObject名称,获取GameObject详细信息");

            registCmd("tree", (object[] args) =>
            {
                if (args != null && args.Length > 0)
                {
                    GameObject obj = GameObject.Find((string)args[0]);//PublicUnits.find((string)args[0]);//
                    msg(PublicUnits.tree(obj.transform));
                }
                else
                {
                    msg(PublicUnits.tree(null));
                }
            }, "如果无参数,则展示hierarchy视图,参数为GameObject名称,则展示该参数的对象树");

            registCmd("cpt", (object[] args) =>
            {
                string name = (string)args[0];
                GameObject obj = PublicUnits.FindByName(name);
                if (obj)
                {
                    Behaviour[] cs = obj.GetComponents<Behaviour>();
                    string result = "total Component count:" + cs.Length;
                    for (int i = 0; i < cs.Length; i++)
                    {
                        result += "\n" + cs[i].GetType().FullName + "(" + cs[i].enabled + ")";
                    }
                    msg(result);
                }
                else
                    msg(Level.WARN, false, "can't find gameobject of name ", name);
            }, "参数为GameObject名称,获取GameObject对象绑定的Component列表");

            registCmd("call", (object[] args) => { 
                string name = (string)args[0];
                GameObject obj = PublicUnits.FindByName(name);
                if (obj)
                {
                    string method = (string)args[1];
                    if (args.Length < 3)
                        obj.SendMessage(method, SendMessageOptions.DontRequireReceiver);
                    else
                    {
                        string param = (string)args[2];
                        obj.SendMessage(method, param, SendMessageOptions.DontRequireReceiver);
                    }
                }
            }, "调用指定name的GameObject上脚本的方法");

            registCmd("stack", (object[] args) => {
                int id = System.Convert.ToInt32(args[0]);
                TextList.Line line = txt.GetLine(id);
                if (line != null)
                    msg(line.stack);
            }, "查看某一条日志的stack");

            //registCmd("level", (object[] args) => {lp.setLevel(args);}, "调整日志等级,输入日志等级名称开启等级,-日志等级关闭等级");
            registCmd("help", () =>
            {
                Cmd cmd;
                foreach (var item in cmds)
                {
                    cmd = item.Value;
                    msg(cmd.helpCmd, cmd.desc);
                }
            }, "显示所有命令");
        }

        internal static void doCmd(string str)
        {
            Cmd cmd = null;
            string[] strs = str.Split(' ');
            if (strs.Length == 0)
                return;

            if (cmds.ContainsKey(strs[0]))
                cmd = cmds[strs[0]];

            string[] args = null;
            if (strs.Length > 1)
            {
                args = new string[strs.Length - 1];
                for (int i = 0; i < strs.Length - 1; )
                {
                    args[i] = strs[++i];
                }
            }

            if (cmd != null)
            {
                msg(Level.INFO, false, "---------------BEGIN " + str + "-----------------");
                cmd.action(args);
                if (strs[0] != "cls")
                    msg(Level.INFO, false, "---------------END " + str + "-----------------");
            }
        }
        #endregion

        #region log msg

        private static Dictionary<Type, ParseFunc<object>> specialList = new Dictionary<Type, ParseFunc<object>>();
        /// <summary>
        /// 添加特殊类型解析器
        /// </summary>
        /// <param name="t"></param>
        /// <param name="func"></param>
        public static void addSpecial(Type t, ParseFunc<object> func)
        {
            specialList[t] = func;
        }

        /// <summary>
        /// 将对象转换成字符串
        /// </summary>
        public static string toString(params object[] args)
        {
            StringBuilder result = new StringBuilder();
            IList objList = new List<object>();
            for (int i = 0; i < args.Length; i++)
            {
                result.Append(_toString(objList, args[i])).Append(" \t");
            }
            return result.ToString();
        }

        public static void debug(params object[] args)
        {
            msg(Level.DEBUG, showNativeOut, args);
        }

        public static void warn(params object[] args)
        {
            msg(Level.WARN, showNativeOut, args);
        }

        public static void fail(params object[] args)
        {
            msg(Level.FAIL, showNativeOut, args);
        }

        public static void error(Exception e, string msg = "")
        {
            if (isDebug == true)
                txt.add(msg + "\n" + e.Message + "\n", e.StackTrace, Level.ERROR);
            if (showNativeOut && Application.isEditor)
                Debug.LogException(e);
        }

        public static void methodStack()
        {
            msg(Level.MSG, showNativeOut, (new System.Exception("===================method  stack=====================")));
        }

        public static void info(params object[] args)
        {
            msg(Level.INFO, showNativeOut, args);
        }

        /// <summary>
        /// 和debug类似,不会自动添加日期和消息类型
        /// </summary>
        public static void msg(params object[] args)
        {
            msg(Level.MSG, false, args);

        }


        private static void RegisterSystemLog(string condition, string stackTrace, LogType type)
        {
            switch (type)
            {
                case LogType.Assert:
                    break;
                case LogType.Error:
                    txt.add(condition, stackTrace, Level.ERROR);
                    break;
                case LogType.Exception:
                    txt.add(condition, stackTrace, Level.FAIL);
                    break;
                case LogType.Log:
                    txt.add(condition, stackTrace, Level.DEBUG);
                    break;
                case LogType.Warning:
                    txt.add(condition, stackTrace, Level.WARN);
                    break;
            }
        }

        private static void msg(Level level, bool useNative, params object[] args)
        {
            if (args.Length == 0)
                return;
            StringBuilder result = new StringBuilder();
            IList objList = new List<object>();
            for (int i = 0; i < args.Length; i++)
            {
                result.Append(_toString(objList, args[i])).Append("\t");
            }

            if (isDebug == true)
                txt.add(result.ToString(), string.Empty, level);

            if (useNative && Application.isEditor)
            {
                string text = getColor(level) + result + "</color>";
                switch (level)
                {
                    case Level.FAIL:
                        Debug.LogError(text);
                        break;
                    case Level.WARN:
                        Debug.LogWarning(text);
                        break;
                    default:
                        Debug.Log(text);
                        break;
                }
            }
        }


        //将对象转换成字符串
        private static string _toString(IList objList, object obj)
        {
            StringBuilder result = new StringBuilder();
            if (obj == null)
                return "null";

            Type type = obj.GetType();
            if (specialList.ContainsKey(type))
            {
                return specialList[type](obj);
            }
            if (objList.IndexOf(obj) != -1)//已经存在,防止对象重复引用
            {
                return result.Append("{ref:").Append(obj.GetType().FullName).Append("}").ToString();
            }
            else if (obj is IDictionary)
            {
                result.Append("{");
                int index = 0;
                foreach (DictionaryEntry item in (IDictionary)obj)
                {
                    if (index++ > 0)
                        result.Append(",");
                    result.Append(Convert.ToString(item.Key)).Append(":").Append(_toString(objList, item.Value));
                }
                result.Append("}");
            }
            else if (obj is Enum)
            {
                result.Append("Enum:(key:").Append(obj.ToString()).Append(",value:").Append(Convert.ToInt32(obj)).Append(")").ToString();
            }
            else if (obj is IList || obj is Array)
            {
                IList list = (IList)obj;
                result.Append("[");
                for (int i = 0; i < list.Count; i++)
                {
                    if (i > 0)
                        result.Append(",");
                    result.Append(_toString(objList, list[i]));
                }
                result.Append("]");
            }
            else
            {
                if (type.IsValueType || type.FullName.StartsWith("UnityEngine") || type.FullName.StartsWith("System"))//如果是unity或者System的包,除集合类之外将都使用系统自带的toString
                {
                    return Convert.ToString(obj);
                }
                else
                {
                    result.Append(obj.GetType().FullName).Append("\n");
                    result.Append("{");
                    FieldInfo[] fields = obj.GetType().GetFields();
                    for (int i = 0; i < fields.Length; i++)
                    {
                        if (i > 0)
                            result.Append(",");
                        result.Append(fields[i].Name).Append(":").Append(_toString(objList, fields[i].GetValue(obj)));
                    }
                    PropertyInfo[] propertys = obj.GetType().GetProperties();
                    if (fields.Length > 0)
                        result.Append(",");
                    for (int i = 0; i < propertys.Length; i++)
                    {
                        if (i > 0)
                            result.Append(",");
                        result.Append(propertys[i].Name).Append(":");
                        if (propertys[i].CanRead)
                        {
                            object v = propertys[i].GetValue(obj, null);
                            result.Append(_toString(objList, v));
                        }
                        else
                            result.Append("not getter");
                    }
                    result.Append("}");
                }
            }
            objList.Add(obj);
            return result.ToString();
        }

        #endregion


        //查看是否有GUILayer,NGUI的摄像机默认是没有该层的
        private void addGuiLayer()
        {
            Camera[] cs = Camera.allCameras;
            if (cs.Length == 0)
            {
                Camera cam = gameObject.AddComponent<Camera>();
                cam.clearFlags = CameraClearFlags.Depth;
                cam.orthographic = true;
            }
            bool hasGuiLayer = false;
            for (int i = 0; i < cs.Length; i++)
            {
                if (cs[i].GetComponent<GUILayer>())//GUILayer必须跟Camera同时存在
                {
                    hasGuiLayer = true;
                    break;
                }
            }
            if (hasGuiLayer == false)
            { 
                cs[0].gameObject.AddComponent<GUILayer>();
            }
        }

        private void initPanel()
        {
            addGuiLayer();

            //GUIText会被depth相机遮挡,GUI.Label不会
            txt = gameObject.AddComponent<GUIText>().gameObject.AddComponent<TextList>();
            txt.width = Screen.width - offset * 2;
            txt.height = Screen.height - 20;
            txt.transform.localPosition = new Vector3((float)offset / (float)Screen.width, (float)offset / (float)Screen.height, 0);
            //txt.txt = new GUIText();
            txt.txt = gameObject.GetComponent<GUIText>();
            txt.txt.richText = true;
            txt.txt.fontSize = TextList.fontSize;
            txt.txt.anchor = TextAnchor.LowerLeft;


            txt.style = new GUIStyle() { alignment = TextAnchor.LowerLeft, fontSize = TextList.fontSize, normal = new GUIStyleState() { textColor = Color.white} };
            txt.rect = new Rect(offset, 10, Screen.width - offset * 2, Screen.height - 20);
            //背景
            Texture2D texture = new Texture2D(2, 2);
            Color32 c = new Color32(0,0,0,128);
            texture.SetPixels32(new Color32[]{c,c,c,c});
            texture.Apply();
            txt.texture = texture;

            setPanel(false);

        }

        //更换场景后查看是否有guilayer
        void OnLevelWasLoaded(int level)
        {
            addGuiLayer();
        }

        void Update()
        {
            ListenOperate();

            if (isOpenPanel)
            {
                if (listenInput)
                    OnInput();
            }
            else
                fps();
        }

        #region touch
        private void setPanel(bool open)
        {
            isOpenPanel = open;
            //background.enabled = open;
            txt.showBackground = open;
            txt.showFps = !open;
            if (open)
            {
                txt.showMsg();
            }
            else
            {
                txt.outTxt = "FPS:";
                stopInput();
            }
        }

        private float accum = 0;
        private int frames = 0;
        private float timeleft;
        private void fps()
        {
            timeleft -= Time.deltaTime;//Time.deltaTime;
            accum += Time.timeScale / Time.deltaTime;
            ++frames;
            if (timeleft <= 0)
            {
                txt.outTxt = "FPS:" + (int)(accum / frames);
                timeleft = updateDelay;
                accum = 0;
                frames = 0;
            }
        }

        //点击或滑动事件
        private Vector2 v = Vector2.zero;
        private void ListenOperate()
        {
            int isTouch = 0;//1点击,2拖拽
            float td = 0;
#if UNITY_EDITOR || UNITY_STANDALONE_OSX || UNITY_STANDALONE_WIN || UNITY_STANDALONE_LINUX
            if (isOpenPanel)
            {
                if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    txt.index--;
                }
                else if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    txt.index++;
                }
            }
            if (Input.GetMouseButtonDown(0))
            {
                v = Input.mousePosition;
                isTouch = 1;
                offset = 10;
                //Debug.Log(v.ToString());
            }
            else if (Input.GetMouseButton(0))
            {
                isTouch = 2;
                dragOffset = 10;
                td = v.y - Input.mousePosition.y;
                if (Mathf.Abs(td) > dragOffset)
                {
                    v = Input.mousePosition;
                }
            }
            else if (Input.GetAxis("Mouse ScrollWheel") != 0)
            {
                isTouch = 2;
                td = -Input.GetAxis("Mouse ScrollWheel") * dragOffset * 10;//滚动以0.1为基本单位
            }
#endif
            
#if MOBILE
                if (Input.touchCount > 0)
                {
                    Touch tc = Input.GetTouch(0);
                    if (tc.phase == TouchPhase.Began)
                    {
                        v = tc.position;
                        isTouch = 1;
                        offset = 30;
                    }
                    else if(tc.phase == TouchPhase.Moved)
                    {
                        isTouch = 2;
                        td = v.y - tc.position.y;
                        if (Mathf.Abs(td) > dragOffset)
                        {
                            v = tc.position;
                        }
                    }
                }
#endif
            if (isTouch == 1)
            {
                if (v.y < offset + TextList.fontSize)
                {
                    if (v.x < offset + TextList.fontSize * 3)
                    {
                        setPanel(!isOpenPanel);
                    }
                    else if (isOpenPanel && v.x > offset + TextList.fontSize * 3)
                    {
                        startInput();
                    }
                }
            }
            else if (isTouch == 2)
            {
                txt.index -= (int)(td / dragOffset);//拖拽日志
            }
        }
        #endregion

        #region input
        private static float cursorIndex = 0;
        private static string cursor = "";
        private static readonly float cursorInterval = 0.3f;//光标闪烁间隔
        private static string inputStr;
#if MOBILE
        private static TouchScreenKeyboard mKeyboard;
#endif
        private bool listenInput = false;

        private void onInputChange(string msg)
        {
            txt.outTxt = txt.str + msg;
        }

        private void onInputSubmit(string inputStr)
        {
            stopInput();
            doCmd(inputStr);
            txt.outTxt = txt.str;
        }

        private void startInput()
        {
            if (listenInput)
                return;

            inputStr = "";
            listenInput = true;
            switch (Application.platform)
            {
                case RuntimePlatform.Android:
                case RuntimePlatform.IPhonePlayer:
                case RuntimePlatform.WP8Player:
                case RuntimePlatform.PS3:
                case RuntimePlatform.TizenPlayer:
                case RuntimePlatform.XBOX360:
#if MOBILE
                    mKeyboard = TouchScreenKeyboard.Open(inputStr, (TouchScreenKeyboardType)((int)TouchScreenKeyboardType.Default), false);
#endif
                    break;
                default:
                    cursorIndex = cursorInterval;
                    cursor = "";
                    break;
            }
        }

        /// <summary>
        /// 停止接受输入
        /// </summary>
        public void stopInput()
        {
            listenInput = false;
#if MOBILE
            if (mKeyboard != null)
            {
                mKeyboard.active = false;
                mKeyboard = null;
            }
#endif
        }

        // 处理pc输入,带光标
        private void OnInput()
        {
#if MOBILE
            // 处理移动平台输入(软键盘)
            if (mKeyboard != null)
            {
                string text = mKeyboard.text;

                if (inputStr != text)
                {
                    inputStr = "";

                    for (int i = 0; i < text.Length; ++i)
                    {
                        char ch = text[i];
                        if (ch != 0) inputStr += ch;
                    }

                    if (inputStr != text) mKeyboard.text = inputStr;
                    onInputChange(inputStr);
                }

                if (mKeyboard.done)
                {
                    mKeyboard.active = false;
                    mKeyboard = null;
                    onInputSubmit(inputStr);
                }
            }
#else
            foreach (char c in Input.inputString)
            {
                //退格 - 移除最后一个字符
                if (c == '\b')
                {
                    if (inputStr.Length != 0)
                        inputStr = inputStr.Substring(0, inputStr.Length - 1);
                    //MessageManager.dispatch(onInputChange.key, inputStr);
                    onInputChange(inputStr);
                }
                //完成进入（登录，登入）
                else if (c == '\n' || c == '\r')
                {// "\n" 用于 Mac, "\r" 用于 windows.
                    //MessageManager.dispatch(onInputSubmit.key, inputStr);
                    onInputSubmit(inputStr);
                    return;
                }
                //普通文本输入 - 只是添加到最后
                else
                {
                    inputStr += c;
                    //MessageManager.dispatch(onInputChange.key, inputStr);

                    onInputChange(inputStr);
                }
            }

            //显示光标
            cursorIndex += Time.deltaTime;
            if ((cursorIndex - cursorInterval) > 0)
            {
                cursorIndex -= cursorInterval;
                if (cursor == "|")
                    cursor = "";
                else
                    cursor = "|";

                //MessageManager.dispatch(onInputChange.key, inputStr + cursor);
                onInputChange(inputStr + cursor);
            }
#endif
            
        }
        #endregion
    }


    //文本显示列表
    internal class TextList : MonoBehaviour
    {
        public const int fontSize = 16;
        /// <summary>
        /// 原始字符串
        /// </summary>
        public string str;
        private int lineHeight = fontSize + 2;
        public GUIText txt;
        public string outTxt;

        public int width = 0, height = 0;

        private List<Line> list = new List<Line>();
        private int _index = 0;
        private int totalLineCount = 0;

        public bool showFps = false;

        public class Line
        {
            public int id;
            public Level level;
            public string stack;
            public string[] lines;
        }


        #region logger
        void OnDestroy()
        {
            LogManager.isDebug = false;
        }

        //将字符串根据长度和换行符切割成数组
        private string[] toLines(string msg)
        {
            string[] strs = msg.Split('\n');
            StringBuilder sb = new StringBuilder();
            StringBuilder result = new StringBuilder();
            float lineWidth = 0;
            string ch, line;
            for (int i = 0; i < strs.Length; i++)
            {
                line = strs[i];
                txt.text = line;
                if (txt.GetScreenRect().width > width)
                {
                    lineWidth = 0;
                    for (int j = 0; j < line.Length; j++)
                    {
                        ch = line[j].ToString();
                        txt.text = ch;
                        lineWidth += txt.GetScreenRect().width;
                        if (lineWidth > width)
                        {
                            result.Append(sb);
                            result.Append('\n');
                            lineWidth = 0;
                            sb.Remove(0, sb.Length);
                        }
                        sb.Append(ch);
                    }
                    result.Append(sb);
                    result.Append('\n');
                }
                else
                {
                    result.Append(line);
                    result.Append('\n');
                }
            }
            string str = result.ToString();
            if(str.Length>0)
               str = str.Substring(0, str.Length - 1);

            txt.text = "";
            return str.Split('\n');
        }

        public Line GetLine(int id)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if(list[i].id == id)
                    return list[i];
            }
            return null;
        }

        public void clear()
        {
            list.Clear();
            index = 0;
        }

        public void add(string msg, string stack, Level level)
        {
            if (level != Level.MSG)
            {
                DateTime dt = DateTime.Now;
                msg = "[" + level.ToString() + "] " + dt.ToString("HH:mm:ss") + "  " + msg;
                //msg = "[" + level.ToString() + "] " + dt.Hour + ":" + dt.Minute + ":" + dt.Second + "  " + msg;
            }
            string[] lines = toLines(msg);
            //if (lines == null || lines.Length == 0)//过滤空消息
            //    return;

            Line line = null;
            if (list.Count >= LogManager.MaxSize)
            {
                line = list[0];
                list.RemoveAt(0);
                //list[0] = list[list.Count - 1];
                //list[list.Count - 1] = line;
                list.Add(line);
            }
            if (line == null)
            {
                line = new Line();
                list.Add(line);
            }
            line.level = level;
            line.stack = stack;
            line.lines = lines;

#if !UNITY_EDITOR
            if (list.Count > 1)
                line.id = list[list.Count - 2].id + 1;
            else
                line.id = 0;
            line.lines[0] = "[" + line.id + "]" + line.lines[0];
#endif
            //文字总行数
            totalLineCount = 0;
            for (int i = 0; i < list.Count; i++)
                totalLineCount += list[i].lines.Length;

            index = totalLineCount;
        }

        public int index
        {
            get { return _index; }
            set
            {
                int lineCount = pageCount;
                if (totalLineCount - value <= lineCount)
                    value = totalLineCount - lineCount;
                if (value < 0)
                    value = 0;
                //if (_index != value)
                {
                    _index = value;
                    showMsg();
                }
            }
        }

        public int pageCount
        {
            get { return height / lineHeight - 1; }
        }

        public void showMsg()
        {
            if (showFps)
                return;
            int lineCount = pageCount;
            StringBuilder sb = new StringBuilder();
            int currentIndex = 0, count = 0;
            for (int i = 0; i < list.Count; i++)
            {
                sb.Append(LogManager.getColor(list[i].level));
                for (int j = 0; j < list[i].lines.Length; j++)
                {
                    if (currentIndex++ >= _index)
                    {
                        if (count < lineCount)
                        {
                            sb.Append(list[i].lines[j]);
                            sb.Append('\n');
                            count++;
                        }
                        else
                        {
                            sb.Append("</color>");
                            goto finish;
                        }
                    }
                }
                sb.Append("</color>");
            }
            finish:
            sb.Append("Close  ");
            str = sb.ToString();
            outTxt = str;
            txt.text = "";
            outTxt = str;
        }

        void OnGUI()
        {
            if (showBackground)
            {
                GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), texture);
            }

            string showTxt = outTxt.Length <= 16382 ? outTxt : outTxt.Substring(outTxt.Length - 16382, 16382);
            GUI.Label(rect, showTxt, style);
        }
        public bool showBackground = false;
        public Rect rect;
        public GUIStyle style;
        public Texture2D texture;
        
        #endregion


    }

}
