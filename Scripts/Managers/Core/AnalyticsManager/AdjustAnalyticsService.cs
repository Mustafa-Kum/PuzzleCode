using com.adjust.sdk;
using UnityEngine.Purchasing;

namespace _Game.Scripts.General.AnalyticsManager
{
    public class AdjustAnalyticsService : IAnalyticsService
    {
        private const string AdjustAppToken = "ehs744";
        
        public void LogEvent(string eventName, AnalyticData analyticData)
        {
            if(eventName == "PurchaseSuccess")
            {
                var purchaseInfo = analyticData.Parameters["PurchaseInfo"] as PurchaseEventArgs;
                var product = purchaseInfo.purchasedProduct;
                AdjustEvent adjustEvent = new AdjustEvent(eventName);
                adjustEvent.setRevenue((float)product.metadata.localizedPrice, product.metadata.isoCurrencyCode);
                adjustEvent.addCallbackParameter("product_id", product.definition.id);
                adjustEvent.setTransactionId(product.transactionID);
                Adjust.trackEvent(adjustEvent);

                // Send InAppPurchaseEvent to Adjust
                AdjustEvent adjustInAppPurchaseEvent = new AdjustEvent(AdjustAppToken);
                adjustInAppPurchaseEvent.addCallbackParameter("product_id", product.definition.id);
                Adjust.trackEvent(adjustInAppPurchaseEvent);
            }
        }
    }
}