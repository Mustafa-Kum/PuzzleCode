namespace _Game.Scripts.RemoteConfig
{
    public class RemoteInGamePopUpController : RemoteMonoManager
    {
        protected override void AssignValueToMono()
        {
            Remoteable.AssignValue(configVariable.GetValueAsInt());
        }
    }
}