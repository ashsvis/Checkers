﻿using System.ServiceModel;

namespace Checkers.Net
{
    [ServiceContract]
    public interface IP2PService
    {
        [OperationContract]
        string GetName();

        [OperationContract(IsOneWay = true)]
        void SendMessage(P2PData message, string from);
    }
}
