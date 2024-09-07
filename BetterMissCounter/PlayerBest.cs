using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Web;
using TMPro;

namespace BetterMissCounter
{
    public class PlayerBest
    {
        private readonly UserInfo userInfo;
        private readonly MapInfo mapInfo;

        public PlayerBest(IPlatformUserModel platformUserModel, GameplayCoreSceneSetupData gameplayCoreSceneSetupData)
        {
            userInfo = platformUserModel.GetUserInfo(CancellationToken.None).Result;
            BeatmapLevel beatmapLevel = gameplayCoreSceneSetupData.beatmapLevel;
            BeatmapKey beatmapKey = gameplayCoreSceneSetupData.beatmapKey;
            int difficultyRank = -1;
            switch (beatmapKey.difficulty)
            {
                case BeatmapDifficulty.Easy:
                    difficultyRank = 1;
                    break;
                case BeatmapDifficulty.Normal:
                    difficultyRank = 3;
                    break;
                case BeatmapDifficulty.Hard:
                    difficultyRank = 5;
                    break;
                case BeatmapDifficulty.Expert:
                    difficultyRank = 7;
                    break;
                case BeatmapDifficulty.ExpertPlus:
                    difficultyRank = 9;
                    break;
            }
            string difficulty = beatmapKey.difficulty.ToString();
            string levelHash = beatmapKey.levelId.Substring(13);
            string characteristic = beatmapKey.beatmapCharacteristic.serializedName;
            this.mapInfo = new MapInfo(difficultyRank, difficulty, levelHash, characteristic);
        }

        string[] GetStringsBetweenStrings(string str, string start, string end)
        {
            List<string> list = new List<string>();
            for (int found = str.IndexOf(start); found > 0; found = str.IndexOf(start, found + 1))
            {
                int startIndex = found + start.Length;
                int endIndex = str.IndexOf(end, startIndex);
                endIndex = endIndex != -1 ? endIndex : str.IndexOf("\n", startIndex);
                list.Add(str.Substring(startIndex, endIndex - startIndex));
            }
            return list.ToArray();
        }

        public void ScoreSaberThread(ref TMP_Text bottomText, ref int PBMissCount)
        {
            WebClient client = new WebClient();
            string endpoint = "";
            for (int page = 1; ; page++)
            {
                try
                {
                    endpoint = "https://scoresaber.com/api/leaderboard/by-hash/" + mapInfo.LevelHash + "/scores?page=" + page + "&difficulty=" + mapInfo.DifficultyRank + "&gameMode=Solo" + mapInfo.Characteristic + "&search=" + HttpUtility.UrlEncode(userInfo.userName);
                    string res = client.DownloadString(endpoint);

                    String[] ids = GetStringsBetweenStrings(res, "\"id\": \"", "\"");
                    String[] missedNotes = GetStringsBetweenStrings(res, "\"missedNotes\": ", ",");
                    String[] badCuts = GetStringsBetweenStrings(res, "\"badCuts\": ", ",");

                    String[] totalItems = GetStringsBetweenStrings(res, "\"total\": ", ",");
                    String[] itemsPerPage = GetStringsBetweenStrings(res, "\"itemsPerPage\": ", ",");

                    for (int i = 0; i < ids.Length; i++)
                    {
                        if (ids[i] == userInfo.platformUserId)
                        {
                            int totalMisses = Int32.Parse(missedNotes[i]) + Int32.Parse(badCuts[i]);
                            if (PBMissCount == -1 || totalMisses < PBMissCount)
                            {
                                PBMissCount = totalMisses;
                                bottomText.text = PluginConfig.Instance.BottomText + PBMissCount;
                            }
                            return;
                        }
                    }

                    if (page == ((Int32.Parse(totalItems[0]) - 1) / Int32.Parse(itemsPerPage[0]) + 1))
                        return;
                }
                catch
                {
#if DEBUG
                    Plugin.Log.Error("ScoreSaber API failed");
                    Plugin.Log.Error($"endpoint:{endpoint}");
#endif
                    return;
                }
            }
        }

        public void BeatLeaderThread(ref TMP_Text bottomText, ref int PBMissCount)
        {
            WebClient client = new WebClient();
            string endpoint = "";
            try
            {
                endpoint = "https://api.beatleader.xyz/score/" + userInfo.platformUserId + "/" + mapInfo.LevelHash + "/" + mapInfo.Difficulty + "/" + mapInfo.Characteristic;
                string res = client.DownloadString(endpoint);
                String[] missedNotes = GetStringsBetweenStrings(res, "\"missedNotes\":", ",");
                String[] badCuts = GetStringsBetweenStrings(res, "\"badCuts\":", ",");
                if (missedNotes.Length > 0)
                {
                    int totalMisses = Int32.Parse(missedNotes[0]) + Int32.Parse(badCuts[0]);
                    if (PBMissCount == -1 || totalMisses < PBMissCount)
                    {
                        PBMissCount = totalMisses;
                        bottomText.text = PluginConfig.Instance.BottomText + PBMissCount;
                    }
                    return;
                }

                return;
            }
            catch
            {
#if DEBUG
                Plugin.Log.Error("BeatLeader API failed");
                Plugin.Log.Error($"endpoint:{endpoint}");
#endif
                return;
            }
        }

        internal class MapInfo
        {
            public MapInfo(int difficultyRank, string difficulty, string levelHash, string characteristic)
            {
                DifficultyRank = difficultyRank;
                Difficulty = difficulty;
                LevelHash = levelHash;
                Characteristic = characteristic;
            }

            internal int DifficultyRank { get; }
            internal string Difficulty { get; }
            internal string LevelHash { get; }
            internal string Characteristic { get; }
        }
    }
}
