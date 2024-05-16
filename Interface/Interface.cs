using System.ServiceModel;


namespace Interface
{
    [ServiceContract]
    public interface IService
    {
        [OperationContract]
        int GetRandomNumber();
    }
}
