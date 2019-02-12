using System;
using System.Collections.Generic;
using System.Net;
using System.Net.PeerToPeer;
using System.ServiceModel;

namespace Checkers.Net
{
    public class NetGame : IDisplayMessage
    {
        private P2PService localService;
        //private string serviceUrl;
        private ServiceHost host;
        private PeerName peerName;
        private PeerNameRegistration peerNameRegistration;

        public bool Started { get; set; }

        public string StartingError { get; set; }

        public bool CanRefreshPeers { get; set; }

        public List<PeerEntry> PeerList { get; set; }

        public PeerEntry Enemy { get; set; }

        public NetGame()
        {
            PeerList = new List<PeerEntry>();
        }

        public void RefreshPeers()
        {
            // Создание распознавателя и добавление обработчиков событий
            PeerNameResolver resolver = new PeerNameResolver();
            resolver.ResolveProgressChanged +=
                new EventHandler<ResolveProgressChangedEventArgs>(resolver_ResolveProgressChanged);
            resolver.ResolveCompleted +=
                new EventHandler<ResolveCompletedEventArgs>(resolver_ResolveCompleted);

            // Подготовка к добавлению новых пиров
            PeerList.Clear();
            CanRefreshPeers = false;

            // Преобразование незащищенных имен пиров асинхронным образом
            resolver.ResolveAsync(new PeerName("0.P2P Sample"), 1);
        }

        public event Action ResolveCompleted = delegate { };

        private void OnResolveCompleted()
        {
            ResolveCompleted();
        }

        void resolver_ResolveCompleted(object sender, ResolveCompletedEventArgs e)
        {
            // Сообщение об ошибке, если в облаке не найдены пиры
            if (PeerList.Count == 0)
            {
                PeerList.Add(
                   new PeerEntry
                   {
                       DisplayString = "Пиры не найдены.",
                       State = PeerState.NotFound,
                       ButtonsEnabled = false
                   });
            }
            // Повторно включаем кнопку "обновить"
            CanRefreshPeers = true;
            OnResolveCompleted();
        }

        public event Action ResolveProgressChanged = delegate { };

        private void OnResolveProgressChanged()
        {
            ResolveProgressChanged();
        }

        void resolver_ResolveProgressChanged(object sender, ResolveProgressChangedEventArgs e)
        {
            PeerNameRecord peer = e.PeerNameRecord;

            foreach (IPEndPoint ep in peer.EndPointCollection)
            {
                if (ep.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    try
                    {
                        string endpointUrl = string.Format("net.tcp://{0}:{1}/P2PService", ep.Address, ep.Port);
                        NetTcpBinding binding = new NetTcpBinding();
                        binding.Security.Mode = SecurityMode.None;
                        IP2PService serviceProxy = ChannelFactory<IP2PService>.CreateChannel(
                            binding, new EndpointAddress(endpointUrl));
                        PeerList.Add(
                           new PeerEntry
                           {
                               PeerName = peer.PeerName,
                               ServiceProxy = serviceProxy,
                               DisplayString = serviceProxy.GetName(),
                               State = PeerState.User,
                               ButtonsEnabled = true
                           });
                        OnResolveProgressChanged();
                    }
                    catch (EndpointNotFoundException)
                    {
                        PeerList.Add(
                           new PeerEntry
                           {
                               PeerName = peer.PeerName,
                               DisplayString = "Неизвестный пир",
                               State = PeerState.Unknown,
                               ButtonsEnabled = false
                           });
                        OnResolveProgressChanged();
                    }
                }
            }
        }

        public void SendMessasge(PeerEntry peerEntry, P2PData message)
        {
            // Получение пира и прокси, для отправки сообщения
            if (peerEntry != null && peerEntry.ServiceProxy != null)
            {
                try
                {
                    peerEntry.ServiceProxy.SendMessage(message, Properties.Settings.Default.P2PUserName);
                }
                catch (CommunicationException)
                {

                }
            }
        }

        public event Action<P2PData, string> DisplayPeerMessage = delegate { };

        private void OnDisplayPeerMessage(P2PData message, string from)
        {
            DisplayPeerMessage(message, from);
        }

        public void DisplayMessage(P2PData message, string from)
        {
            OnDisplayPeerMessage(message, from);
        }

        public bool Start(string port, string username, string machineName)
        {
            if (Started) return true;
            Enemy = null;
            Started = false;

            #region Взято отюда: https://professorweb.ru/my/csharp/web/level8/8_3.php

            // Конфигурирование сети 
            // Получение конфигурационной информации из app.config
            string serviceUrl = null;

            //  Получение URL-адреса службы с использованием адресаIPv4 
            //  и порта из конфигурационного файла
            foreach (IPAddress address in Dns.GetHostAddresses(Dns.GetHostName()))
            {
                if (address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    serviceUrl = string.Format("net.tcp://{0}:{1}/P2PService", address, port);
                    break;
                }
            }

            // Выполнение проверки, не является ли адрес null
            if (serviceUrl == null)
            {
                // Отображение ошибки и завершение работы приложения
                StartingError = "Не удается определить адрес конечной точки WCF.";
                return false;
            }

            // Регистрация и запуск службы WCF
            localService = new P2PService(this, username);

            host = new ServiceHost(localService, new Uri(serviceUrl));
            NetTcpBinding binding = new NetTcpBinding();
            binding.Security.Mode = SecurityMode.None;
            host.AddServiceEndpoint(typeof(IP2PService), binding, serviceUrl);
            try
            {
                host.Open();
            }
            catch (AddressAlreadyInUseException)
            {
                // Отображение ошибки и завершение работы приложения
                StartingError = "Не удаётся начать прослушивание, порт занят.";
                return false;
            }

            // Создание имени равноправного участника (пира)
            peerName = new PeerName("P2P Sample", PeerNameType.Unsecured);

            // Подготовка процесса регистрации имени равноправного участника в локальном облаке
            peerNameRegistration = new PeerNameRegistration(peerName, int.Parse(port))
            {
                Cloud = Cloud.AllLinkLocal
            };

            // Запуск процесса регистрации
            peerNameRegistration.Start();

            #endregion

            Started = true;
            return Started;
        }

        public void Stop()
        {
            if (!Started) return;

            #region Взято отюда: https://professorweb.ru/my/csharp/web/level8/8_3.php

            // Остановка регистрации
            peerNameRegistration.Stop();

            // Остановка WCF-сервиса
            host.Close();

            #endregion
        }
    }
}
