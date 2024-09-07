using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Threading;
using TMPro;

namespace BetterMissCounter
{
    internal class PlayerBest
    {
        private readonly UserInfo userInfo;
        private readonly MapInfo mapInfo;

        public PlayerBest(IPlatformUserModel platformUserModel,GameplayCoreSceneSetupData gameplayCoreSceneSetupData)
        {
            var userInfo = platformUserModel.GetUserInfo(CancellationToken.None).Result;
            IDifficultyBeatmap beatmap = gameplayCoreSceneSetupData.difficultyBeatmap;
            int difficultyRank = beatmap.difficultyRank;
            string difficulty = beatmap.difficulty.SerializedName();
            string levelHash = beatmap.level.levelID.Substring(13);
            string characteristic = beatmap.parentDifficultyBeatmapSet.beatmapCharacteristic.serializedName;
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

        public int ScoreSaberThread(ref TMP_Text missText, ref TMP_Text bottomText, int PBMissCount)
        {
            WebClient client = new WebClient();
            for (int page = 1; ; page++)
            {
                try
                {
                    string endpoint = "https://scoresaber.com/api/leaderboard/by-hash/" + mapInfo.LevelHash + "/scores?page=" + page + "&difficulty=" + mapInfo.DifficultyRank + "&gameMode=Solo" + mapInfo.Characteristic + "&search=" + HttpUtility.UrlEncode(userInfo.userName);
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
                            return PBMissCount;
                        }
                    }

                    if (page == ((Int32.Parse(totalItems[0]) - 1) / Int32.Parse(itemsPerPage[0]) + 1))
                        return PBMissCount;
                }
                catch
                {
                    return PBMissCount;
                }
            }
        }

        public int BeatLeaderThread(ref TMP_Text missText, ref TMP_Text bottomText, int PBMissCount)
        {
            WebClient client = new WebClient();
            try
            {
                string endpoint = "https://api.beatleader.xyz/score/" + userInfo.platformUserId + "/" + mapInfo.LevelHash + "/" + mapInfo.Difficulty + "/" + mapInfo.Characteristic;
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
                    return PBMissCount;
                }

                return PBMissCount;
            }
            catch
            {
                return PBMissCount;
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
