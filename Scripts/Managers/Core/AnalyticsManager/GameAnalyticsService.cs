using System.Text;
using GameAnalyticsSDK;

namespace _Game.Scripts.General.AnalyticsManager
{
    public class GameAnalyticsService : IAnalyticsService
    {
        public void LogEvent(string eventName, AnalyticData analyticData)
        {
            if (analyticData == null || analyticData.Parameters.Count == 0)
            {
                GameAnalytics.NewDesignEvent(eventName);
                return;
            }

            var parametersBuilder = new StringBuilder(eventName);

            foreach (var param in analyticData.Parameters) parametersBuilder.Append($":{param.Key}_{param.Value}");

            // Log the event
            GameAnalytics.NewDesignEvent(parametersBuilder.ToString());
        }
    }
}