using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Osblow.App
{
    public class SettingUIContext : BaseContext
    {
        public SettingUIContext() : base(UIType.SettingView) { }
    }

    public class SettingView : BaseView
    {
        #region 场景引用 
        public Slider VoiceSlider;
        public Slider MusicSlider;
        public Button VoiceBtn;
        public Button MusicBtn;
        #endregion

        #region 场景事件
        public void OnExitBtn()
        {
            Globals.SceneSingleton<ContextManager>().Pop();
            Globals.SceneSingleton<SoundMng>().PlayCommonButtonSound();
        }

        public void OnVoiceChange(float value)
        {
            VoiceBtn.interactable = value > 0;
            Globals.SceneSingleton<SoundMng>().FrontSoundVolume = value;
        }

        public void OnMusicChange(float value)
        {
            MusicBtn.interactable = value > 0;
            Globals.SceneSingleton<SoundMng>().BackSoundVolume = value;
        }

        public void OnVoiceBtn()
        {
            VoiceSlider.value = 0;
            VoiceBtn.interactable = false;
            Globals.SceneSingleton<SoundMng>().PlayCommonButtonSound();
        }

        public void OnMusicBtn()
        {
            MusicSlider.value = 0;
            MusicBtn.interactable = false;
            Globals.SceneSingleton<SoundMng>().PlayCommonButtonSound();
        }

        public void OnChangeLogin()
        {
            PlayerPrefs.SetString("username", "");
            Globals.SceneSingleton<ContextManager>().Push(new LoginUIContext());
        }
        #endregion



        public override void OnEnter(BaseContext context)
        {
            base.OnEnter(context);

            MusicSlider.onValueChanged.AddListener(OnMusicChange);
            VoiceSlider.onValueChanged.AddListener(OnVoiceChange);

            MusicSlider.value = Globals.SceneSingleton<SoundMng>().BackSoundVolume;
            VoiceSlider.value = Globals.SceneSingleton<SoundMng>().FrontSoundVolume;
        }

        public override void OnExit(BaseContext context)
        {
            base.OnExit(context);
        }

        public override void OnPause(BaseContext context)
        {
            base.OnPause(context);
        }

        public override void OnResume(BaseContext context)
        {
            base.OnResume(context);
        }
    }
}
