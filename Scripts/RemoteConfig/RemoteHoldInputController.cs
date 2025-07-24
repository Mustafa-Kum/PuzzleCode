namespace _Game.Scripts.RemoteConfig
{
    public class RemoteHoldInputController : RemoteMonoManager
    {
        protected override void AssignValueToMono()
        {
            Remoteable.AssignValue(configVariable.GetValueAsInt());
        }
    }
}