using System;
using System.ServiceModel;

namespace Checkers.Net
{
    [ServiceContract]
    public interface IP2PService
    {
        [OperationContract]
        string GetName();

        [OperationContract]
        Player GetPlayer();

        [OperationContract]
        Guid GetPlayerId();

        [OperationContract(IsOneWay = true)]
        void SendMessage(P2PData message, string from);

        [OperationContract(IsOneWay = true)]
        void SendConnect(Guid id, string from);

    }
}
