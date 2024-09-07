using CountersPlus.Counters.Interfaces;
using CountersPlus.Utils;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace BetterMissCounter
{
    /// <summary>
    /// Monobehaviours (scripts) are added to GameObjects.
    /// For a full list of Messages a Monobehaviour can receive from the game, see https://docs.unity3d.com/ScriptReference/MonoBehaviour.html.
    /// </summary>
	public class BetterMissCounter : CountersPlus.Counters.Custom.BasicCustomCounter, INoteEventHandler
    {   
        TMP_Text missText;
        TMP_Text bottomText;
        int missCount = 0;
        int PBMissCount = -1;

        private readonly PlayerBest _pb;
        private readonly BloomFontAsset _bloomFontAsset;
        private readonly bool isThisMapCustomLevel;

        public BetterMissCounter(GameplayCoreSceneSetupData gameplayCoreSceneSetupData, PlayerBest pb, BloomFontAsset bloomFontAsset)
        {
            isThisMapCustomLevel = gameplayCoreSceneSetupData.beatmapLevel.levelID.IndexOf("custom_level_") != -1;
            _bloomFontAsset = bloomFontAsset;
            _pb = pb;
        }

        public override void CounterInit()
        {

            TMP_Text topText = CanvasUtility.CreateTextFromSettings(Settings, new Vector3(
                0 + PluginConfig.Instance.CounterXOffset,
                0 + PluginConfig.Instance.CounterYOffset,
                0));
            missText = CanvasUtility.CreateTextFromSettings(Settings, new Vector3(
                0 + PluginConfig.Instance.CounterXOffset,
                -0.35f + PluginConfig.Instance.CounterYOffset,
                0)
                );
            bottomText = CanvasUtility.CreateTextFromSettings(Settings, new Vector3(
                0 + PluginConfig.Instance.CounterXOffset,
                -0.65f + PluginConfig.Instance.CounterYOffset,
                0));

            topText.fontSize = 3f;
            topText.text = PluginConfig.Instance.TopText;
            topText.color = PluginConfig.Instance.TopColor;
            if (PluginConfig.Instance.TopBloom)
            {
                topText.font = _bloomFontAsset.FontAsset;
            }
            missText.fontSize = 4f;
            missText.text = "0";
            missText.color = PluginConfig.Instance.LessColor;
            if (PluginConfig.Instance.MissesBloom)
            {
                missText.font = _bloomFontAsset.FontAsset;
            }
            bottomText.fontSize = 2f;
            bottomText.color = PluginConfig.Instance.BottomColor;
            if (PluginConfig.Instance.BottomBloom)
            {
                bottomText.font = _bloomFontAsset.FontAsset;
            }

            if (isThisMapCustomLevel)
            {
                if (PluginConfig.Instance.UseScoreSaber)
                {
                    Task.Run(() =>
                    {
                        _pb.ScoreSaberThread(ref bottomText, ref PBMissCount);
                    });
                }
                if (PluginConfig.Instance.UseBeatLeader)
                {
                    Task.Run(() =>
                    {
                        _pb.BeatLeaderThread(ref bottomText, ref PBMissCount);
                    });
                }
            }

        }

        public override void CounterDestroy()
        {

        }

        public void OnNoteCut(NoteData data, NoteCutInfo info)
        {
            if (!info.allIsOK && data.colorType != ColorType.None) UpdateCount(1);
        }

        public void OnNoteMiss(NoteData data)
        {
            if (data.colorType != ColorType.None) UpdateCount(1);
        }

        public void UpdateCount(int add = 0)
        {
            missCount += add;
            missText.text = "" + missCount;
            if (PBMissCount > -1)
            {
                missText.color = missCount < PBMissCount ? PluginConfig.Instance.LessColor :
                    missCount == PBMissCount ? PluginConfig.Instance.EqualColor : PluginConfig.Instance.MoreColor;
            }
        }
    }
}
