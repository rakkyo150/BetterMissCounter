using UnityEngine;

namespace BetterMissCounter
{
    internal class PluginConfig
    {
        public static PluginConfig Instance { get; set; }
        public virtual float CounterXOffset { get; set; } = 0.0f;
        public virtual float CounterYOffset { get; set; } = 0.0f;
        public virtual string TopText { get; set; } = "Misses";
        public virtual Color TopColor { get; set; } = Color.white;
        public virtual bool TopBloom { get; set; } = false;
        public virtual string BottomText { get; set; } = "PB: ";
        public virtual Color BottomColor { get; set; } = Color.white;
        public virtual bool BottomBloom { get; set; } = false;
        public virtual Color LessColor { get; set; } = Color.white;
        public virtual Color EqualColor { get; set; } = Color.yellow;
        public virtual Color MoreColor { get; set; } = Color.red;
        public virtual bool UseScoreSaber { get; set; } = true;
        public virtual bool UseBeatLeader { get; set; } = true;
        public virtual bool MissesBloom { get; set; } = true;
    }
}
