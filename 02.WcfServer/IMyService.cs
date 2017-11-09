using System.ServiceModel;

namespace WcfServer
{
    [ServiceContract]
    public interface IMyService
    {
        [OperationContract]
        string Hello();
    }
}
