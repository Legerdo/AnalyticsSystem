using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace AnalyticsSystem
{
    public class AnalyticsUpdater
    {
        private static readonly AnalyticsUpdater _instance = new AnalyticsUpdater();
        private readonly CancellationTokenSource _cts = new CancellationTokenSource();

        private AnalyticsUpdater()
        {
            StartUpdateLoop().Forget();
        }

        public static AnalyticsUpdater Instance => _instance;

        private async UniTaskVoid StartUpdateLoop()
        {
            while (!_cts.IsCancellationRequested)
            {
                await UpdateAnalytics();
                await UniTask.Delay(TimeSpan.FromSeconds(5), cancellationToken: _cts.Token);
            }
        }

        private async UniTask UpdateAnalytics()
        {
            var values = AnalyticsManager.GetAllValues();
            await SendAnalyticsData(values);
        }

        private async UniTask SendAnalyticsData(Dictionary<string, object> data)
        {
            // 여기에 서버로 데이터를 전송하는 로직을 구현합니다.
            await UniTask.CompletedTask;
        }

        public void StopUpdates()
        {
            _cts.Cancel();
        }
    }
}