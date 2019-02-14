using System;
using System.Collections.Generic;
using System.Net;
using System.Net.PeerToPeer;
using System.ServiceModel;
using System.Threading.Tasks;

namespace Checkers.Net
{
    public class NetGame : IDisplayMessage
    {
        private P2PService localService;
        //private string serviceUrl;
        private ServiceHost host;
        private PeerName peerName;
        private PeerNameRegistration peerNameRegistration;
        private Game _game;
        private readonly Board _board;

        public bool Started { get; set; }

        public string StartingError { get; set; }

        public bool CanRefreshPeers { get; set; }

        public List<PeerEntry> PeerList { get; set; }

        public PeerEntry Enemy { get; set; }

        public Guid Id { get; private set; }

        public string Caption
        {
            get
            {
                if (_game.Mode == PlayMode.NetGame)
                    return Enemy == null 
                        ? string.Format("Шашки - {0} ожидает противника...", Properties.Settings.Default.P2PUserName)
                        : _game.Player == Player.White 
                              ? string.Format("Шашки - {0} против {1}", Properties.Settings.Default.P2PUserName, Enemy.DisplayString)
                              : string.Format("Шашки - {0} против {1}", Enemy.DisplayString, Properties.Settings.Default.P2PUserName);

                return "Шашки";
            }
        }

        public NetGame(Game game, Board board)
        {
            PeerList = new List<PeerEntry>();
            Id = Guid.NewGuid();
            _game = game;
            _board = board;
        }

        public void SetGame(Game game)
        {
            _game = game;
        }

        public void SendNetGameStatus()
        {
            if (Started && Enemy != null)
            {
                var item = _game.Log[_game.Log.Count - 1];
                var step = string.Format("{0}", _game.Direction ? item.White : item.Black);
                SendMessage(Enemy, new P2PData(_game.Direction ? Player.Black : Player.White, Id,
                    step, _game.BlackScore, _game.WhiteScore)
                { Map = _board.ToString() });
            }
        }

        public Task RefreshPeersAsync()
        {
            var task = new Task(() => RefreshPeers());
            task.Start();
            return task;
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

        void resolver_ResolveCompleted(object sender, ResolveCompletedEventArgs e)
        {
            CanRefreshPeers = true;
            ResolveCompleted();
        }

        public event Action ResolveProgressChanged = delegate { };

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
                               Player = serviceProxy.GetPlayer(),
                               PlayerId = serviceProxy.GetPlayerId()
                           });
                        ResolveProgressChanged();
                    }
                    catch (EndpointNotFoundException)
                    {
                    }
                }
            }
        }

        public void SendConnect(PeerEntry peerEntry)
        {
            // Получение пира и прокси, для отправки сообщения
            if (peerEntry != null && peerEntry.ServiceProxy != null)
            {
                try
                {
                    peerEntry.ServiceProxy.SendConnect(Id, Properties.Settings.Default.P2PUserName);
                }
                catch (CommunicationException)
                {

                }
            }
        }

        public void SendMessage(PeerEntry peerEntry, P2PData message)
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

        public void DisplayMessage(P2PData message, string from)
        {
            DisplayPeerMessage(message, from);
        }

        public void DisplayConnect(Guid id, string from)
        {
            if (Started && Enemy == null)
            {
                ResolveCompleted += _net_ResolveCompleted;
                _connectId = id;
                _connCount = 10;
                RefreshPeersAsync();
            }
        }

        private Guid _connectId;
        private int _connCount;

        private void _net_ResolveCompleted()
        {
            foreach (var peer in PeerList)
            {
                if (peer.PlayerId == _connectId)
                {
                    Enemy = peer;
                    ResolveCompleted -= _net_ResolveCompleted;
                    _game.WinPlayer = WinPlayer.Game;
                    CaptionChanged();
                    return;
                }
            }
            _connCount--;
            if (_connCount <= 0) return;
            RefreshPeersAsync();
        }

        public event Action CaptionChanged = delegate { };

        private bool Start(string port, string username, Player player, string machineName)
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
            localService = new P2PService(this, username, player, Id);

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

        public Task<bool> StartAsync(string port, string username, Player player, string machineName)
        {
            var task = new Task<bool>(() => Start(port, username, player, machineName));
            task.Start();
            return task;
        }

        private void Stop()
        {
            if (!Started) return;
            Started = false;

            #region Взято отюда: https://professorweb.ru/my/csharp/web/level8/8_3.php

            // Остановка регистрации
            peerNameRegistration.Stop();

            // Остановка WCF-сервиса
            host.Close();

            #endregion
        }

        public Task StopAsync()
        {
            var task = new Task(() => Stop());
            task.Start();
            return task;
        }

        public Task RestartAsync(string port, string username, Player player, string machineName)
        {
            var task = new Task(() => 
            {
                Stop();
                Start(port, username, player, machineName);
            });
            task.Start();
            return task;

        }
    }
}
