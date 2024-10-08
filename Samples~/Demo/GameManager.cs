using System;
using AnalyticsSystem;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace AnalyticsSystem
{
    public static class GameManager
    {
        public static void InitializeAnalytics()
        {
            // 필요한 경우 초기값을 설정할 수 있습니다.
            AnalyticsManager.SetValue("TotalKills", 0);
            AnalyticsManager.SetValue("AverageScore", 0f);
            AnalyticsManager.SetValue("TotalPlayTime", TimeSpan.Zero);

            // AnalyticsUpdater 인스턴스 생성 (자동으로 업데이트 루프 시작)
            var updater = AnalyticsUpdater.Instance;
        }

        public static void OnEnemyKilled()
        {
            AnalyticsManager.AddValue("TotalKills", 1);
        }

        public static void UpdateAverageScore(float newScore)
        {
            var currentAverage = AnalyticsManager.GetValue<float>("AverageScore");
            var totalKills = AnalyticsManager.GetValue<int>("TotalKills");
            var newAverage = (currentAverage * totalKills + newScore) / (totalKills + 1);
            AnalyticsManager.SetValue("AverageScore", newAverage);
        }

        public static void UpdatePlayTime(TimeSpan sessionTime)
        {
            AnalyticsManager.AddValue("TotalPlayTime", sessionTime);
        }

        [System.Serializable]
        private class SerializableAnalyticsData
        {
            public List<AnalyticsEntry> entries = new List<AnalyticsEntry>();
        }

        [System.Serializable]
        private class AnalyticsEntry
        {
            public string key;
            public string value;
            public string type;
        }

        public static async Task ExportAnalyticsToFile()
        {
            try
            {
                // 모든 통계 데이터 가져오기
                var allStats = AnalyticsManager.GetAllValues();

                // 직렬화 가능한 형식으로 변환
                var serializableData = new SerializableAnalyticsData();
                foreach (var kvp in allStats)
                {
                    serializableData.entries.Add(new AnalyticsEntry
                    {
                        key = kvp.Key,
                        value = kvp.Value.ToString(),
                        type = kvp.Value.GetType().Name
                    });
                }

                // JSON 형식으로 변환
                string jsonString = JsonUtility.ToJson(serializableData, true);

                // 파일 이름 생성 (현재 날짜와 시간 포함)
                string fileName = $"GameStats_{DateTime.Now:yyyyMMdd_HHmmss}.json";

                // 파일 저장 경로 설정 (Unity의 persistentDataPath 사용)
                string filePath = Path.Combine(Application.persistentDataPath, fileName);

                // 파일에 비동기로 쓰기
                await File.WriteAllTextAsync(filePath, jsonString);

                Debug.Log($"Analytics data exported to: {filePath}");
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error exporting analytics data: {ex.Message}");
            }
        }
    }
}