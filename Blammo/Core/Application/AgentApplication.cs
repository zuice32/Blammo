using System;
using Agent.Core.Exceptions;
using Agent.Core.Logging;
using Agent.Core.Services;
using Agent.Core.Settings;

namespace Agent.Core.Application
{
    public static class AgentApplication
    {
        private const string ThisClassName = "Agent.Core.AgentApplication";

//        private const string Shutdownkey = "Agent Last Clean Shutdown";
        private const string RestartApplicationNowSettingName = "RestartApplicationNow";

        private static IApplicationServiceBus _serviceBus;
        private static IExceptionHandler _exceptionHandler;
        private static ISettingsProvider _settingsProvider;
        private static Action _onShutDown;
        private static IApplicationLog _applicationLog;
        private static IAgentIdentity _agentIdentity;

//        private static bool _initCalled;

//        public static void BeginInit(string appName, Func<ApplicationServiceBus> loadServiceBus)
//        {
//            var appThread = new Thread(() => Init(appName, loadServiceBus))
//            {
//                Priority = ThreadPriority.AboveNormal,
//                Name = "main application"
//            };
//
//            appThread.Start();
//        }

//        public static bool IsRunning { get; private set; }

        public static bool RestartRequested { get; private set; }

//        public static bool LoggingInitialized { get; private set; }
//
//        public static bool SettingsProviderInitialized { get; private set; }

//        /// <summary>
//        ///     Defaults to 2 minutes.
//        /// </summary>
//        public static int AppShutdownTimeoutSeconds
//        {
//            get
//            {
//                string timeout = _settingsProvider.GetSetting(ThisClassName, "AppShutdownTimeoutSeconds",
//                    120.ToString());
//
//                return Int32.Parse(timeout);
//            }
//        }

//        private static void LogIPAddress(ILogWriter logWriter)
//        {
//            IPHostEntry ipHostEntry = Dns.GetHostEntry(Dns.GetHostName());
//
//            foreach (IPAddress address in ipHostEntry.AddressList)
//            {
//                logWriter.AddEntry(new LogEntry(MessageLevel.Verbose, ThisClassName, address.ToString()));
//            }
//        }

        public static void ShutDown()
        {
//            if (!IsRunning)
//            {
//                throw new InvalidOperationException("Agent is not running.");
//            }

            if (_onShutDown != null) _onShutDown();
            //            WriteShutdownTimeToRegistry(DateTime.Now.ToString());
            if (_applicationLog != null)
            {
                string agentId = _agentIdentity == null ? "(unknown)" : _agentIdentity.ID;

                _applicationLog.AddEntry(
                    new LogEntry(
                        MessageLevel.AppLifecycle,
                        ThisClassName,
                        string.Format("Agent {0} shutting down.", agentId)));
            }

            StopAllServices();

            //            ApplicationLog.LogWriters.RemoveAll(w => w is DbLogWriter);
        }

        private static void StopAllServices()
        {
            try
            {
                if (_serviceBus != null) _serviceBus.Dispose();

//                IsRunning = false;
            }
            catch (Exception e)
            {
                if (_exceptionHandler != null) _exceptionHandler.HandleException(e);
            }
        }

        private static bool RestartApplicationNow
        {
            get
            {
                string boolean = _settingsProvider.GetSetting(
                    ThisClassName,
                    RestartApplicationNowSettingName,
                    false.ToString());

                return Convert.ToBoolean(boolean);
            }
            set { _settingsProvider.UpsertSetting(ThisClassName, RestartApplicationNowSettingName, value.ToString()); }
        }

        public static void Init(
            IApplicationLog applicationLog,
            Func<ILogWriter, IApplicationWatchdog> initWatchdog,
            Func<ILogWriter, IExceptionHandler> initExceptionHandler,
            Func<ILogWriter, IAgentIdentity> initAgentIdentity,
            Func<ILogWriter, IAgentIdentity, ISettingsProvider> initSettingsProvider,
            Action<ISettingsProvider, IApplicationLog, IAgentIdentity> initApplicationLog,
            Func
                <IExceptionHandler, ILogWriter, ISettingsProvider, IAgentIdentity, IApplicationWatchdog,
                    IApplicationServiceBus> loadServiceBus,
            Action onShutDown)
        {
            if (applicationLog == null) throw new ArgumentNullException("applicationLog");
            if (initWatchdog == null) throw new ArgumentNullException("initWatchdog");
            if (initApplicationLog == null) throw new ArgumentNullException("initApplicationLog");
            if (initExceptionHandler == null) throw new ArgumentNullException("initExceptionHandler");
            if (initSettingsProvider == null) throw new ArgumentNullException("initSettingsProvider");
            if (initAgentIdentity == null) throw new ArgumentNullException("initAgentIdentity");
            if (onShutDown == null) throw new ArgumentNullException("onShutDown");
            if (loadServiceBus == null) throw new ArgumentNullException("loadServiceBus");

            _onShutDown = onShutDown;

            IApplicationWatchdog watchdog = initWatchdog(applicationLog);

            watchdog.BeatDog();

            _exceptionHandler = initExceptionHandler(applicationLog);

            _agentIdentity = initAgentIdentity(applicationLog);
              
            applicationLog.AddEntry(
                new LogEntry(
                    MessageLevel.AppLifecycle,
                    ThisClassName,
                    string.Format("Agent {0} started.", _agentIdentity.ID)));

            _settingsProvider = initSettingsProvider(applicationLog, _agentIdentity);

            initApplicationLog(_settingsProvider, applicationLog, _agentIdentity);

            _applicationLog = applicationLog;

            _serviceBus = loadServiceBus(_exceptionHandler, applicationLog, _settingsProvider, _agentIdentity, watchdog);

            _settingsProvider.SettingsChanged += SettingsProvider_SettingsChanged;
        }

        private static void SettingsProvider_SettingsChanged()
        {
            if (RestartApplicationNow)
            {
                RestartApplicationNow = false;

                ShutDown();

                RestartRequested = true;
            }
        }

//        public static void WriteShutdownTimeToRegistry(string value)
//        {
//            Registry.LocalMachine.SetValue(Shutdownkey, value);
//
//            Registry.LocalMachine.Flush();
//        }
//
//        public static string ReadShutdownTimeFromRegistry()
//        {
//            object keyValue = Registry.LocalMachine.GetValue(Shutdownkey);
//
//            if (keyValue == null)
//            {
//                return string.Empty;
//            }
//
//            return keyValue.ToString();
//        }

//        private static void InitializeApplicationLog()
//        {
//            ApplicationLog.LogWriters.Add(new DbLogWriter(IdentityManager.PlatformSpecificConnectionString));
//
//#if !WINFULL
//            ApplicationLog.LogWriters.Add(new ConsoleLogWriter());
//            ApplicationLog.LogWriters.Add(new TextFileLogWriter("\\USB Storage\\agent_log.txt"));
//#endif
//            InMemoryLogWriter tempWriter = ApplicationLog.LogWriters.OfType<InMemoryLogWriter>().First();
//
//            ApplicationLog.LogWriters.Remove(tempWriter);
//
//            foreach (ILogEntry entry in tempWriter.Entries)
//            {
//                ApplicationLog.AddEntry(entry);
//            }
//
//            ApplicationLog.LogFilters.Add(new WhiteSourceLogFilter());
//
//            ApplicationLog.LogFilters.Add(new BlackSourceLogFilter("Recording Polling"));
//
//            ApplicationLog.LogFilters.Add(new MessageLevelLogFilter());
//
//            LoggingInitialized = true;
//        }

//        private static void PerformMaintenance()
//        {
//            if (!Utilities.VerifyCertificates())
//            {
//                Utilities.InstallCerts();
//            }
//
//            Utilities.CleanUpSandboxDirectories();
//
//            // cannot perform this operation until servicebus is loaded
//            // be careful, can't delete dlls until we are sure obfuscated and merged
//            // version of code is running
////            if (_serviceBus != null)
////            {
////                Utilities.CleanUpApplicationDirectory();
////            }
//        }


//        private static void InitExceptionHandler()
//        {
//            ExceptionHandler.HandleUnhandledAppDomainExceptions(new NullStrategy());
//
//            LoggingStrategy.LogEntryModifiers.Add(new ExceptionAsWarningLogModifier<ServiceDisposeTimeoutException>());
//
//            LoggingStrategy.LogEntryModifiers.Add(new ExceptionAsWarningLogModifier<CommunicationsFailedException>());
//
//            LoggingStrategy.LogEntryModifiers.Add(new ExceptionAsWarningLogModifier<CommunicationErrorException>());
//
//            LoggingStrategy.LogEntryModifiers.Add(new ExceptionAsWarningLogModifier<EndpointNotFoundException>());
//
//            LoggingStrategy.LogEntryModifiers.Add(new ExceptionAsWarningLogModifier<SensorPollTimeoutException>());
//
//            LoggingStrategy.LogEntryModifiers.Add(new ExceptionAsWarningLogModifier<TimeoutException>());
//
//            LoggingStrategy.LogEntryModifiers.Add(new ExceptionAsWarningLogModifier<SecurityNegotiationException>());
//
//            LoggingStrategy.LogEntryModifiers.Add(new ExceptionAsWarningLogModifier<UnwrappedCFFaultException>());
//
//            IList<IExceptionStrategy> defaultStrategies = ExceptionHandler.DefaultStrategies.Strategies;
//
//            defaultStrategies.Insert(0, new CFFaultExceptionStrategy());
//
//            defaultStrategies.Insert(0, new UnwrapTargetInvocationExceptionsStrategy());
//
////#if !DEBUG
//            var generalFloodStrategy = new GeneralExceptionFloodStrategy(TimeSpan.FromMinutes(30), TimeSpan.FromHours(1));
//
//            generalFloodStrategy.FloodWatchers.Add(typeof (EndpointNotFoundException),
//                new ExceptionFloodWatcher(TimeSpan.FromHours(1), TimeSpan.FromHours(1)));
//
//            generalFloodStrategy.FloodWatchers.Add(typeof (CommunicationsFailedException),
//                new ExceptionFloodWatcher(TimeSpan.FromMinutes(30), TimeSpan.FromHours(1)));
//
//            generalFloodStrategy.FloodWatchers.Add(typeof (CommunicationErrorException),
//                new ExceptionFloodWatcher(TimeSpan.FromMinutes(30), TimeSpan.FromHours(1)));
//
//            generalFloodStrategy.FloodWatchers.Add(typeof (SensorPollTimeoutException),
//                new ExceptionFloodWatcher(TimeSpan.FromMinutes(30), TimeSpan.FromHours(1)));
//
//            generalFloodStrategy.FloodWatchers.Add(typeof (UnhandledMessageException),
//                new ExceptionFloodWatcher(TimeSpan.FromMinutes(30), TimeSpan.FromHours(1)));
//
//            defaultStrategies.Insert(0, generalFloodStrategy);
////#endif
////            defaultStrategies.Insert(0,
////                                     new ExceptionFloodStrategy<EndpointNotFoundException>(
////                                         new TimeSpan(1, 0, 0)));
////
////            defaultStrategies.Insert(0,
////                                     new ExceptionFloodStrategy<CommunicationsFailedException>(
////                                         new TimeSpan(0, 30, 0)));
////
////            defaultStrategies.Insert(0,
////                                     new ExceptionFloodStrategy<SensorPollTimeoutException>(
////                                         new TimeSpan(0, 30, 0)));
////
////            defaultStrategies.Insert(0,
////                                     new ExceptionFloodStrategy<UnhandledMessageException>(
////                                         new TimeSpan(0, 30, 0)));
//        }


//        private static void InitializeWatchdog()
//        {
//#if !WINFULL
//            try
//            {
//                if (WatchdogCommon.WatchdogEnabled())
//                {
//                    WatchdogCommon.InitWatchdog(WatchdogCommon.DefaultWatchPeriodSeconds);
//                }
//                else
//                {
//                    string pathToWatchdogExe = GetPathToAgentExe() + "\\Watchdog\\";
//
//                    pathToWatchdogExe = Path.Combine(pathToWatchdogExe, WatchdogCommon.watchdogExeFilename);
//
//                    ApplicationLog.AddEntry(MessageLevel.Verbose, ThisClassName,
//                        "Launching watchdog process at:" + pathToWatchdogExe);
//
//                    if (!File.Exists(pathToWatchdogExe))
//                    {
//                        ApplicationLog.AddEntry(MessageLevel.Warning, ThisClassName,
//                            "Agent watchdog not found at:" + pathToWatchdogExe);
//
//                        return;
//                    }
//
//                    using (Process agentProcess = Process.Start(pathToWatchdogExe, string.Empty))
//                    {
//                        if (agentProcess == null || agentProcess.HasExited)
//                        {
//                            //TODO: Use custom exception type.
//                            throw new ApplicationException(string.Format("Could not start {0}.", pathToWatchdogExe));
//                        }
//                    }
//                }
//            }
//            catch (Exception e)
//            {
//                ExceptionHandler.HandleException(e);
//            }
//#endif
//        }

//        public static ApplicationServiceBus ServiceBus { get; private set; }
//
//        public static string GetPathToAgentExe()
//        {
//            return IdentityManager.PathToAppFolder;
//        }


//#if WINFULL
//        public static void Sync()
//        {
//            SyncService syncService = ServiceBus.GetService<SyncService>();
//
//            syncService.SynchronizeLocalDataWithServerDb();
//        }
//#endif
    }
}