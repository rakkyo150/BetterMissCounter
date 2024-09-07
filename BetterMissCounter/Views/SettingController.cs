using BeatSaberMarkupLanguage.Attributes;
using UnityEngine;

namespace BetterMissCounter.Views
{
    [HotReload(RelativePathToLayout = @"SettingsUI.bsml")]
    [ViewDefinition("BetterMissCounter.Views.SettingsUI.bsml")]
    internal class SettingController
    {
        [UIValue("CounterXOffset")]
        public float CounterXOffset
        {
            get => PluginConfig.Instance.CounterXOffset;
            set => PluginConfig.Instance.CounterXOffset = value;
        }

        [UIValue("CounterYOffset")]
        public float CounterYOffset
        {
            get => PluginConfig.Instance.CounterYOffset;
            set => PluginConfig.Instance.CounterYOffset = value;
        }

        [UIValue("TopText")]
        public string TopText
        {
            get => PluginConfig.Instance.TopText;
            set => PluginConfig.Instance.TopText = value;
        }

        [UIValue("TopColor")]
        public Color TopColor
        {
            get => PluginConfig.Instance.TopColor;
            set => PluginConfig.Instance.TopColor = value;
        }

        [UIValue("TopBloom")]
        public bool TopBloom
        {
            get => PluginConfig.Instance.TopBloom;
            set => PluginConfig.Instance.TopBloom = value;
        }

        [UIValue("BottomText")]
        public string BottomText
        {
            get => PluginConfig.Instance.BottomText;
            set => PluginConfig.Instance.BottomText = value;
        }

        [UIValue("BottomColor")]
        public Color BottomColor
        {
            get => PluginConfig.Instance.BottomColor;
            set => PluginConfig.Instance.BottomColor = value;
        }

        [UIValue("BottomBloom")]
        public bool BottomBloom
        {
            get => PluginConfig.Instance.BottomBloom;
            set => PluginConfig.Instance.BottomBloom = value;
        }

        [UIValue("LessColor")]
        public Color LessColor
        {
            get => PluginConfig.Instance.LessColor;
            set => PluginConfig.Instance.LessColor = value;
        }

        [UIValue("EqualColor")]
        public Color EqualColor
        {
            get => PluginConfig.Instance.EqualColor;
            set => PluginConfig.Instance.EqualColor = value;
        }

        [UIValue("MoreColor")]
        public Color MoreColor
        {
            get => PluginConfig.Instance.MoreColor;
            set => PluginConfig.Instance.MoreColor = value;
        }

        [UIValue("UseScoreSaber")]
        public bool UseScoreSaber
        {
            get => PluginConfig.Instance.UseScoreSaber;
            set => PluginConfig.Instance.UseScoreSaber = value;
        }

        [UIValue("UseBeatLeader")]
        public bool UseBeatLeader
        {
            get => PluginConfig.Instance.UseBeatLeader;
            set => PluginConfig.Instance.UseBeatLeader = value;
        }

        [UIValue("MissesBloom")]
        public bool MissesBloom
        {
            get => PluginConfig.Instance.MissesBloom;
            set => PluginConfig.Instance.MissesBloom = value;
        }
    }
}
