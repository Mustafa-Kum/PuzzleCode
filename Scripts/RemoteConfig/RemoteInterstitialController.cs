namespace _Game.Scripts.RemoteConfig
{
    public class RemoteInterstitialController : RemoteMonoManager
    {
        protected override void AssignValueToMono()
        {
            Remoteable.AssignValue(configVariable.GetValueAsInt());
        }
    }
}