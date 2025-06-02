using Chaotic.Resources;
using Chaotic.Tasks;
using Chaotic.Tasks.Una;
using Chaotic.User;
using Chaotic.Utilities;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using IP = Chaotic.Utilities.ImageProcessing;

namespace Chaotic
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public MainWindow()
        {
            var appDataDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Chaotic\\";
            _busy = new ManualResetEvent(false);

            if (!Directory.Exists(appDataDirectory))
                Directory.CreateDirectory(appDataDirectory);

            _settings = new UserSettings();
            _settings = _settings.Read(USER_SETTINGS_PATH);

            _config = ApplicationResources.Create(_settings.Resolution);

            _statistics = new SessionStatistics();
            //_statistics = _statistics.Read(USER_STATISTICS_PATH);

            Log = new SessionLog(_settings.LogDetailLevel);
            _logger = new AppLogger(Log, Statistics);

            InitializeComponent();

            _mouse = new MouseUtility();
            _kb = new KeyboardUtility();

            InitializeTasks();

        }

        private readonly ApplicationResources _config;

        private List<string> _Resolutions = new List<string> { "3440x1440", "2560x1440", "1920x1080" };
        private List<Tuple<string, int>> _GemLevels = new List<Tuple<string, int>>()
        {
            new Tuple<string, int>("Level 3+", 3),
            new Tuple<string, int>("Level 4+", 4),
            new Tuple<string, int>("Level 5+", 5),
            new Tuple<string, int>("Level 6+", 6),
            new Tuple<string, int>("Level 7+", 7),
            new Tuple<string, int>("Level 8+", 8),
            new Tuple<string, int>("Level 9+", 9),
        };

        private List<Tuple<string, LogDetailLevel>> _LogLevels = new List<Tuple<string, LogDetailLevel>>()
        {
            new Tuple<string, LogDetailLevel>("None", LogDetailLevel.None),
            new Tuple<string, LogDetailLevel>("Debug", LogDetailLevel.Debug),
            new Tuple<string, LogDetailLevel>("Info", LogDetailLevel.Info),
            new Tuple<string, LogDetailLevel>("Summary", LogDetailLevel.Summary)
        };

        //private ApplicationResources _r;
        private ApplicationResources _r;
        private UITasks _uit;
        private GuildTasks _gt;
        private UnaTasks _ut;
        private ChaosTasks _ct;
        private readonly MouseUtility _mouse;
        private readonly KeyboardUtility _kb;
        private SessionLog _log;
        private readonly AppLogger _logger;


        public SessionLog Log
        {
            get { return _log; }
            set
            {
                if (_log != null)
                    _log.LogEntries.CollectionChanged -= LogEntriesChanged;

                _log = value;
                _log.LogEntries.CollectionChanged += LogEntriesChanged;
                OnPropertyChanged();
            }
        }


        public List<String> Resolutions { get { return _Resolutions; } set { _Resolutions = value; } }
        public List<Tuple<string, int>> GemLevels { get { return _GemLevels; } set { _GemLevels = value; } }
        public List<Tuple<string, LogDetailLevel>> LogLevels { get { return _LogLevels; } set { _LogLevels = value; } }
        private List<int> _AvailableBifrosts = Enumerable.Range(0, 7).ToList();
        public List<int> AvailableBifrosts { get { return _AvailableBifrosts; } }
        public List<String> AvailableClasses { get { return UserCharacter.AllClasses; } }
        public List<String> AvailableUnaTaskNames { get { return UnaTasks.AvailableUnaTasks; } }

        private ObservableCollection<UserKeyboardKey> _AvailableKeyboardKeys = new ObservableCollection<UserKeyboardKey>()
        {
            new UserKeyboardKey() { Display = "A", InputKey = Key.A, Modifier = KeyboardModifier.None },
            new UserKeyboardKey() { Display = "B", InputKey = Key.B, Modifier = KeyboardModifier.None },
            new UserKeyboardKey() { Display = "C", InputKey = Key.C, Modifier = KeyboardModifier.None },
            new UserKeyboardKey() { Display = "D", InputKey = Key.D, Modifier = KeyboardModifier.None },
            new UserKeyboardKey() { Display = "E", InputKey = Key.E, Modifier = KeyboardModifier.None },
            new UserKeyboardKey() { Display = "F", InputKey = Key.F, Modifier = KeyboardModifier.None },
            new UserKeyboardKey() { Display = "G", InputKey = Key.G, Modifier = KeyboardModifier.None },
            new UserKeyboardKey() { Display = "H", InputKey = Key.H, Modifier = KeyboardModifier.None },
            new UserKeyboardKey() { Display = "I", InputKey = Key.I, Modifier = KeyboardModifier.None },
            new UserKeyboardKey() { Display = "J", InputKey = Key.J, Modifier = KeyboardModifier.None },
            new UserKeyboardKey() { Display = "K", InputKey = Key.K, Modifier = KeyboardModifier.None },
            new UserKeyboardKey() { Display = "L", InputKey = Key.L, Modifier = KeyboardModifier.None },
            new UserKeyboardKey() { Display = "M", InputKey = Key.M, Modifier = KeyboardModifier.None },
            new UserKeyboardKey() { Display = "N", InputKey = Key.N, Modifier = KeyboardModifier.None },
            new UserKeyboardKey() { Display = "O", InputKey = Key.O, Modifier = KeyboardModifier.None },
            new UserKeyboardKey() { Display = "P", InputKey = Key.P, Modifier = KeyboardModifier.None },
            new UserKeyboardKey() { Display = "Q", InputKey = Key.Q, Modifier = KeyboardModifier.None },
            new UserKeyboardKey() { Display = "R", InputKey = Key.R, Modifier = KeyboardModifier.None },
            new UserKeyboardKey() { Display = "S", InputKey = Key.S, Modifier = KeyboardModifier.None },
            new UserKeyboardKey() { Display = "T", InputKey = Key.T, Modifier = KeyboardModifier.None },
            new UserKeyboardKey() { Display = "U", InputKey = Key.U, Modifier = KeyboardModifier.None },
            new UserKeyboardKey() { Display = "V", InputKey = Key.V, Modifier = KeyboardModifier.None },
            new UserKeyboardKey() { Display = "W", InputKey = Key.W, Modifier = KeyboardModifier.None },
            new UserKeyboardKey() { Display = "X", InputKey = Key.X, Modifier = KeyboardModifier.None },
            new UserKeyboardKey() { Display = "Y", InputKey = Key.Y, Modifier = KeyboardModifier.None },
            new UserKeyboardKey() { Display = "Z", InputKey = Key.Z, Modifier = KeyboardModifier.None },

            new UserKeyboardKey() { Display = "Alt-A", InputKey = Key.A, Modifier = KeyboardModifier.Alt },
            new UserKeyboardKey() { Display = "Alt-B", InputKey = Key.B, Modifier = KeyboardModifier.Alt },
            new UserKeyboardKey() { Display = "Alt-C", InputKey = Key.C, Modifier = KeyboardModifier.Alt },
            new UserKeyboardKey() { Display = "Alt-D", InputKey = Key.D, Modifier = KeyboardModifier.Alt },
            new UserKeyboardKey() { Display = "Alt-E", InputKey = Key.E, Modifier = KeyboardModifier.Alt },
            new UserKeyboardKey() { Display = "Alt-F", InputKey = Key.F, Modifier = KeyboardModifier.Alt },
            new UserKeyboardKey() { Display = "Alt-G", InputKey = Key.G, Modifier = KeyboardModifier.Alt },
            new UserKeyboardKey() { Display = "Alt-H", InputKey = Key.H, Modifier = KeyboardModifier.Alt },
            new UserKeyboardKey() { Display = "Alt-I", InputKey = Key.I, Modifier = KeyboardModifier.Alt },
            new UserKeyboardKey() { Display = "Alt-J", InputKey = Key.J, Modifier = KeyboardModifier.Alt },
            new UserKeyboardKey() { Display = "Alt-K", InputKey = Key.K, Modifier = KeyboardModifier.Alt },
            new UserKeyboardKey() { Display = "Alt-L", InputKey = Key.L, Modifier = KeyboardModifier.Alt },
            new UserKeyboardKey() { Display = "Alt-M", InputKey = Key.M, Modifier = KeyboardModifier.Alt },
            new UserKeyboardKey() { Display = "Alt-N", InputKey = Key.N, Modifier = KeyboardModifier.Alt },
            new UserKeyboardKey() { Display = "Alt-O", InputKey = Key.O, Modifier = KeyboardModifier.Alt },
            new UserKeyboardKey() { Display = "Alt-P", InputKey = Key.P, Modifier = KeyboardModifier.Alt },
            new UserKeyboardKey() { Display = "Alt-Q", InputKey = Key.Q, Modifier = KeyboardModifier.Alt },
            new UserKeyboardKey() { Display = "Alt-R", InputKey = Key.R, Modifier = KeyboardModifier.Alt },
            new UserKeyboardKey() { Display = "Alt-S", InputKey = Key.S, Modifier = KeyboardModifier.Alt },
            new UserKeyboardKey() { Display = "Alt-T", InputKey = Key.T, Modifier = KeyboardModifier.Alt },
            new UserKeyboardKey() { Display = "Alt-U", InputKey = Key.U, Modifier = KeyboardModifier.Alt },
            new UserKeyboardKey() { Display = "Alt-V", InputKey = Key.V, Modifier = KeyboardModifier.Alt },
            new UserKeyboardKey() { Display = "Alt-W", InputKey = Key.W, Modifier = KeyboardModifier.Alt },
            new UserKeyboardKey() { Display = "Alt-X", InputKey = Key.X, Modifier = KeyboardModifier.Alt },
            new UserKeyboardKey() { Display = "Alt-Y", InputKey = Key.Y, Modifier = KeyboardModifier.Alt },
            new UserKeyboardKey() { Display = "Alt-Z", InputKey = Key.Z, Modifier = KeyboardModifier.Alt },

            new UserKeyboardKey() { Display = "Ctrl-A", InputKey = Key.A, Modifier = KeyboardModifier.Ctrl },
            new UserKeyboardKey() { Display = "Ctrl-B", InputKey = Key.B, Modifier = KeyboardModifier.Ctrl },
            new UserKeyboardKey() { Display = "Ctrl-C", InputKey = Key.C, Modifier = KeyboardModifier.Ctrl },
            new UserKeyboardKey() { Display = "Ctrl-D", InputKey = Key.D, Modifier = KeyboardModifier.Ctrl },
            new UserKeyboardKey() { Display = "Ctrl-E", InputKey = Key.E, Modifier = KeyboardModifier.Ctrl },
            new UserKeyboardKey() { Display = "Ctrl-F", InputKey = Key.F, Modifier = KeyboardModifier.Ctrl },
            new UserKeyboardKey() { Display = "Ctrl-G", InputKey = Key.G, Modifier = KeyboardModifier.Ctrl },
            new UserKeyboardKey() { Display = "Ctrl-H", InputKey = Key.H, Modifier = KeyboardModifier.Ctrl },
            new UserKeyboardKey() { Display = "Ctrl-I", InputKey = Key.I, Modifier = KeyboardModifier.Ctrl },
            new UserKeyboardKey() { Display = "Ctrl-J", InputKey = Key.J, Modifier = KeyboardModifier.Ctrl },
            new UserKeyboardKey() { Display = "Ctrl-K", InputKey = Key.K, Modifier = KeyboardModifier.Ctrl },
            new UserKeyboardKey() { Display = "Ctrl-L", InputKey = Key.L, Modifier = KeyboardModifier.Ctrl },
            new UserKeyboardKey() { Display = "Ctrl-M", InputKey = Key.M, Modifier = KeyboardModifier.Ctrl },
            new UserKeyboardKey() { Display = "Ctrl-N", InputKey = Key.N, Modifier = KeyboardModifier.Ctrl },
            new UserKeyboardKey() { Display = "Ctrl-O", InputKey = Key.O, Modifier = KeyboardModifier.Ctrl },
            new UserKeyboardKey() { Display = "Ctrl-P", InputKey = Key.P, Modifier = KeyboardModifier.Ctrl },
            new UserKeyboardKey() { Display = "Ctrl-Q", InputKey = Key.Q, Modifier = KeyboardModifier.Ctrl },
            new UserKeyboardKey() { Display = "Ctrl-R", InputKey = Key.R, Modifier = KeyboardModifier.Ctrl },
            new UserKeyboardKey() { Display = "Ctrl-S", InputKey = Key.S, Modifier = KeyboardModifier.Ctrl },
            new UserKeyboardKey() { Display = "Ctrl-T", InputKey = Key.T, Modifier = KeyboardModifier.Ctrl },
            new UserKeyboardKey() { Display = "Ctrl-U", InputKey = Key.U, Modifier = KeyboardModifier.Ctrl },
            new UserKeyboardKey() { Display = "Ctrl-V", InputKey = Key.V, Modifier = KeyboardModifier.Ctrl },
            new UserKeyboardKey() { Display = "Ctrl-W", InputKey = Key.W, Modifier = KeyboardModifier.Ctrl },
            new UserKeyboardKey() { Display = "Ctrl-X", InputKey = Key.X, Modifier = KeyboardModifier.Ctrl },
            new UserKeyboardKey() { Display = "Ctrl-Y", InputKey = Key.Y, Modifier = KeyboardModifier.Ctrl },
            new UserKeyboardKey() { Display = "Ctrl-Z", InputKey = Key.Z, Modifier = KeyboardModifier.Ctrl },

            new UserKeyboardKey() { Display = "Shift-A", InputKey = Key.A, Modifier = KeyboardModifier.Shift },
            new UserKeyboardKey() { Display = "Shift-B", InputKey = Key.B, Modifier = KeyboardModifier.Shift },
            new UserKeyboardKey() { Display = "Shift-C", InputKey = Key.C, Modifier = KeyboardModifier.Shift },
            new UserKeyboardKey() { Display = "Shift-D", InputKey = Key.D, Modifier = KeyboardModifier.Shift },
            new UserKeyboardKey() { Display = "Shift-E", InputKey = Key.E, Modifier = KeyboardModifier.Shift },
            new UserKeyboardKey() { Display = "Shift-F", InputKey = Key.F, Modifier = KeyboardModifier.Shift },
            new UserKeyboardKey() { Display = "Shift-G", InputKey = Key.G, Modifier = KeyboardModifier.Shift },
            new UserKeyboardKey() { Display = "Shift-H", InputKey = Key.H, Modifier = KeyboardModifier.Shift },
            new UserKeyboardKey() { Display = "Shift-I", InputKey = Key.I, Modifier = KeyboardModifier.Shift },
            new UserKeyboardKey() { Display = "Shift-J", InputKey = Key.J, Modifier = KeyboardModifier.Shift },
            new UserKeyboardKey() { Display = "Shift-K", InputKey = Key.K, Modifier = KeyboardModifier.Shift },
            new UserKeyboardKey() { Display = "Shift-L", InputKey = Key.L, Modifier = KeyboardModifier.Shift },
            new UserKeyboardKey() { Display = "Shift-M", InputKey = Key.M, Modifier = KeyboardModifier.Shift },
            new UserKeyboardKey() { Display = "Shift-N", InputKey = Key.N, Modifier = KeyboardModifier.Shift },
            new UserKeyboardKey() { Display = "Shift-O", InputKey = Key.O, Modifier = KeyboardModifier.Shift },
            new UserKeyboardKey() { Display = "Shift-P", InputKey = Key.P, Modifier = KeyboardModifier.Shift },
            new UserKeyboardKey() { Display = "Shift-Q", InputKey = Key.Q, Modifier = KeyboardModifier.Shift },
            new UserKeyboardKey() { Display = "Shift-R", InputKey = Key.R, Modifier = KeyboardModifier.Shift },
            new UserKeyboardKey() { Display = "Shift-S", InputKey = Key.S, Modifier = KeyboardModifier.Shift },
            new UserKeyboardKey() { Display = "Shift-T", InputKey = Key.T, Modifier = KeyboardModifier.Shift },
            new UserKeyboardKey() { Display = "Shift-U", InputKey = Key.U, Modifier = KeyboardModifier.Shift },
            new UserKeyboardKey() { Display = "Shift-V", InputKey = Key.V, Modifier = KeyboardModifier.Shift },
            new UserKeyboardKey() { Display = "Shift-W", InputKey = Key.W, Modifier = KeyboardModifier.Shift },
            new UserKeyboardKey() { Display = "Shift-X", InputKey = Key.X, Modifier = KeyboardModifier.Shift },
            new UserKeyboardKey() { Display = "Shift-Y", InputKey = Key.Y, Modifier = KeyboardModifier.Shift },
            new UserKeyboardKey() { Display = "Shift-Z", InputKey = Key.Z, Modifier = KeyboardModifier.Shift },

            new UserKeyboardKey() { Display = "1", InputKey = Key.D1, Modifier = KeyboardModifier.None },
            new UserKeyboardKey() { Display = "2", InputKey = Key.D2, Modifier = KeyboardModifier.None },
            new UserKeyboardKey() { Display = "3", InputKey = Key.D3, Modifier = KeyboardModifier.None },
            new UserKeyboardKey() { Display = "4", InputKey = Key.D4, Modifier = KeyboardModifier.None },
            new UserKeyboardKey() { Display = "5", InputKey = Key.D5, Modifier = KeyboardModifier.None },
            new UserKeyboardKey() { Display = "6", InputKey = Key.D6, Modifier = KeyboardModifier.None },
            new UserKeyboardKey() { Display = "7", InputKey = Key.D7, Modifier = KeyboardModifier.None },
            new UserKeyboardKey() { Display = "8", InputKey = Key.D8, Modifier = KeyboardModifier.None },
            new UserKeyboardKey() { Display = "9", InputKey = Key.D9, Modifier = KeyboardModifier.None },
            new UserKeyboardKey() { Display = "0", InputKey = Key.D0, Modifier = KeyboardModifier.None },

            new UserKeyboardKey() { Display = "F1", InputKey = Key.F1, Modifier = KeyboardModifier.None },
            new UserKeyboardKey() { Display = "F2", InputKey = Key.F2, Modifier = KeyboardModifier.None },
            new UserKeyboardKey() { Display = "F3", InputKey = Key.F3, Modifier = KeyboardModifier.None },
            new UserKeyboardKey() { Display = "F4", InputKey = Key.F4, Modifier = KeyboardModifier.None },
            new UserKeyboardKey() { Display = "F5", InputKey = Key.F5, Modifier = KeyboardModifier.None },
            new UserKeyboardKey() { Display = "F6", InputKey = Key.F6, Modifier = KeyboardModifier.None },
            new UserKeyboardKey() { Display = "F7", InputKey = Key.F7, Modifier = KeyboardModifier.None },
            new UserKeyboardKey() { Display = "F8", InputKey = Key.F8, Modifier = KeyboardModifier.None },
            new UserKeyboardKey() { Display = "F9", InputKey = Key.F9, Modifier = KeyboardModifier.None },
            new UserKeyboardKey() { Display = "F10", InputKey = Key.F10, Modifier = KeyboardModifier.None },
            new UserKeyboardKey() { Display = "F11", InputKey = Key.F11, Modifier = KeyboardModifier.None },
            new UserKeyboardKey() { Display = "F12", InputKey = Key.F12, Modifier = KeyboardModifier.None }
        };
        public ObservableCollection<UserKeyboardKey> AvailableKeyboardKeys { get { return _AvailableKeyboardKeys; } }

        private List<int> _AvailableChaosLevels = new List<int>()
        {
            1415,
            1445,
            1475,
            1490,
            1520,
            1540,
            1560,
            1580,
            1600,
            1610,
            1640,
            1660,
            1680,
            1700
        };

        private List<string> _AvailableRaidPresets = new List<string>()
        {
            "None",
            "A",
            "S",
            "D",
            "F",
            "G",
        };

        public List<string> AvailableRaidPresets { get { return _AvailableRaidPresets; } }

        public List<int> AvailableChaosLevels { get { return _AvailableChaosLevels; } }
        private List<int> _AvailableIndexes = Enumerable.Range(1, 30).ToList();
        public List<int> AvailableIndexes { get { return _AvailableIndexes; } }
        private UserCharacter? _CurrentDailySelectedChar;
        public UserCharacter? CurrentDailySelectedChar
        {
            get { return _CurrentDailySelectedChar; }
            set
            {
                _CurrentDailySelectedChar = value;
                OnPropertyChanged();
            }
        }

        private LogEntry? _CurrentLogEntry;
        public LogEntry? CurrentLogEntry
        {
            get { return _CurrentLogEntry; }
            set
            {
                _CurrentLogEntry = value;
                OnPropertyChanged();
            }
        }

        private UserCharacter? _CurrentSelectedEditChar;
        public UserCharacter? CurrentSelectedEditChar
        {
            get { return _CurrentSelectedEditChar; }
            set
            {
                _CurrentSelectedEditChar = value;
                OnPropertyChanged();
                NotifySkillChanges();
            }
        }

        public void NotifySkillChanges()
        {
            OnPropertyChanged("CurrentQSkill");
            OnPropertyChanged("CurrentWSkill");
            OnPropertyChanged("CurrentESkill");
            OnPropertyChanged("CurrentRSkill");
            OnPropertyChanged("CurrentASkill");
            OnPropertyChanged("CurrentSSkill");
            OnPropertyChanged("CurrentDSkill");
            OnPropertyChanged("CurrentFSkill");
            OnPropertyChanged("CurrentHyperSkill");
            OnPropertyChanged("CurrentAwakening");
        }

        public UserCharacterSkill CurrentQSkill
        {
            get
            {
                if (CurrentSelectedEditChar == null)
                    return new UserCharacterSkill();
                else
                    return CurrentSelectedEditChar.AllResolutionSkills[UserSettings.Resolution].QSkill;
            }
        }
        public UserCharacterSkill CurrentWSkill
        {
            get
            {
                if (CurrentSelectedEditChar == null)
                    return new UserCharacterSkill();
                else
                    return CurrentSelectedEditChar.AllResolutionSkills[UserSettings.Resolution].WSkill;
            }
        }
        public UserCharacterSkill CurrentESkill
        {
            get
            {
                if (CurrentSelectedEditChar == null)
                    return new UserCharacterSkill();
                else
                    return CurrentSelectedEditChar.AllResolutionSkills[UserSettings.Resolution].ESkill;
            }
        }
        public UserCharacterSkill CurrentRSkill
        {
            get
            {
                if (CurrentSelectedEditChar == null)
                    return new UserCharacterSkill();
                else
                    return CurrentSelectedEditChar.AllResolutionSkills[UserSettings.Resolution].RSkill;
            }
        }
        public UserCharacterSkill CurrentASkill
        {
            get
            {
                if (CurrentSelectedEditChar == null)
                    return new UserCharacterSkill();
                else
                    return CurrentSelectedEditChar.AllResolutionSkills[UserSettings.Resolution].ASkill;
            }
        }
        public UserCharacterSkill CurrentSSkill
        {
            get
            {
                if (CurrentSelectedEditChar == null)
                    return new UserCharacterSkill();
                else
                    return CurrentSelectedEditChar.AllResolutionSkills[UserSettings.Resolution].SSkill;
            }
        }
        public UserCharacterSkill CurrentDSkill
        {
            get
            {
                if (CurrentSelectedEditChar == null)
                    return new UserCharacterSkill();
                else
                    return CurrentSelectedEditChar.AllResolutionSkills[UserSettings.Resolution].DSkill;
            }
        }
        public UserCharacterSkill CurrentFSkill
        {
            get
            {
                if (CurrentSelectedEditChar == null)
                    return new UserCharacterSkill();
                else
                    return CurrentSelectedEditChar.AllResolutionSkills[UserSettings.Resolution].FSkill;
            }
        }

        public UserCharacterSkill CurrentHyperSkill
        {
            get
            {
                if (CurrentSelectedEditChar == null)
                    return new UserCharacterSkill();
                else
                    return CurrentSelectedEditChar.AllResolutionSkills[UserSettings.Resolution].HyperSkill;
            }
        }
        public UserCharacterSkill CurrentAwakening
        {
            get
            {
                if (CurrentSelectedEditChar == null)
                    return new UserCharacterSkill();
                else
                    return CurrentSelectedEditChar.AllResolutionSkills[UserSettings.Resolution].Awakening;
            }
        }


        private bool _taskRunning;
        public bool TaskRunning
        {
            get { return this._taskRunning; }
            set
            {
                this._taskRunning = value;
                OnPropertyChanged();
            }
        }
        private UserSettings _settings;
        private SessionStatistics _statistics;

        public UserSettings UserSettings
        {
            get { return this._settings; }
            set
            {
                this._settings = value;
                OnPropertyChanged();
            }
        }

        public SessionStatistics Statistics
        {
            get { return this._statistics; }
            set
            {
                this._statistics = value;
                OnPropertyChanged();
            }
        }


        private string USER_STATISTICS_PATH = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Chaotic\\userStatistics.json";


        //System.IO.Path.GetDirectoryName(new System.Uri(System.Reflection.Assembly.GetExecutingAssembly().Location).LocalPath) + "\\userStatistics.json";

        private string USER_SETTINGS_PATH = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Chaotic\\userSettings.json";
        //System.IO.Path.GetDirectoryName(new System.Uri(System.Reflection.Assembly.GetExecutingAssembly().Location).LocalPath) + "\\userSettings.json";

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }



        private void InitializeTasks()
        {
            _r = ApplicationResources.Create(UserSettings.Resolution);
            //_r = new ApplicationResources("Chaotic.Resources." + UserSettings.Resolution);
            _uit = new UITasks(UserSettings, _mouse, _kb, _r, _logger);
            _gt = new GuildTasks(UserSettings, _uit, _mouse, _kb, _r, _logger);
            _ut = new UnaTasks(UserSettings, _mouse, _kb, _r, _uit, _logger);
            _ct = new ChaosTasks(UserSettings, _mouse, _kb, _r, _uit, _logger);
        }

        private DateTime _currentWorkStartTime;
        private BackgroundWorker _bw;
        private readonly ManualResetEvent _busy;

        private bool ShouldAcceptWeeklies(UserCharacter character)
        {
            bool accept = false;
            DateTime lastReset;
            if (DateTime.Today.DayOfWeek == DayOfWeek.Wednesday)
                lastReset = DateTime.Today.Date;
            else
            {
                DateTime today = DateTime.Today.Date;
                int diff = DateTime.Today.DayOfWeek - DayOfWeek.Wednesday;
                DateTime lastWednesday = diff > 0 ? today.AddDays(-1 * diff) : today.AddDays(-1 * (7 + diff));
                lastReset = lastWednesday;
            }

            if (lastReset > character.LastWeeklyReset)
                accept = true;

            return accept;
        }

        private void Resolution_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _r = ApplicationResources.Create(UserSettings.Resolution);
            //_r = new ApplicationResources("Chaotic.Resources." + UserSettings.Resolution);
            SaveUserSettings();

            InitializeTasks();
        }

        private void SaveUserSettings()
        {
            UserSettings.Save(USER_SETTINGS_PATH);
            NotifySkillChanges();
        }

        private bool RunCharacterDailyRotation(UserCharacter character, bool shouldAcceptWeeklies = false)
        {
            _mouse.ClickCenterScreen(_r.CenterScreen);

            bool success = true;
            success = _gt.PerformGuildTasks(character);
            int dailiesAccepted = _ut.AcceptDailies(character);
            if (dailiesAccepted > 0)
                success = success && _ut.RunDailies(character);

            if (success && shouldAcceptWeeklies)
            {
                success = success && _ut.AcceptWeeklies(character);
                if (character.BuySoloMode)
                    success = success && _uit.BuySoloModeShop(character);

                if (character.BuyGuildShop)
                    success = success && _uit.BuyGuildShop(character);

                if (success)
                {
                    character.LastWeeklyReset = DateTime.Now.Date;
                    SaveUserSettings();
                }
            }

            if (success && character.RunChaos)
                success = _ct.RunChaos(character);

            if (character.RaidPreset != "None")
                _uit.SetCharacterSkillSet(character.RaidPreset);

            BackgroundProcessing.ProgressCheck();
            _uit.ClearOngoingQuests();

            Sleep.SleepMs(1500, 2500, _settings.PerformanceMultiplier);

            if (success && UserSettings.EnableAura)
            {
                if (UserSettings.RepairGear)
                    success = success && _uit.AuraRepair();

                BackgroundProcessing.ProgressCheck();
                if (success && (UserSettings.MoveHoningMaterials || UserSettings.MoveGems))
                {
                    success = success && _uit.OpenInventoryManagement();
                    if (success && UserSettings.MoveHoningMaterials)
                        success = success && _uit.MoveHoningMaterials();
                    if (success && UserSettings.MoveGems && !character.IsMain)
                        success = success && _uit.MoveGems();
                    if (success)
                        success = success && _uit.CloseInventoryManagement();
                }
            }

            _logger.Log(LogDetailLevel.Info, $"Character Dailies Complete on {character.ClassName}({character.ChaosLevel}).  Reported Success: {success}");
            return success;
        }

        private void RunCharacterUnas_Click(object sender, RoutedEventArgs e)
        {
            Action a = () =>
            {
                _mouse.ClickCenterScreen(_r.CenterScreen);
                if (CurrentDailySelectedChar != null)
                    _ut.RunDailies(CurrentDailySelectedChar);
            };
            CreateBackgroundWorker(a);
        }

        private void RunCharacterChaos_Click(object sender, RoutedEventArgs e)
        {
            Action a = () =>
            {
                if (CurrentDailySelectedChar != null)
                    _ct.RunChaos(CurrentDailySelectedChar);
            };
            CreateBackgroundWorker(a);
        }

        private void RunCharacterDailies_Click(object sender, RoutedEventArgs e)
        {
            Action a = () =>
            {
                if (CurrentDailySelectedChar != null)
                {
                    if (_settings.GoOffline)
                        _uit.GoOffline();

                    var success = RunCharacterDailyRotation(CurrentDailySelectedChar);
                    if (success)
                        _logger.Log(LogDetailLevel.Summary, "Character Daily Rotation Complete");
                }
            };

            CreateBackgroundWorker(a);
        }

        private void RunAllDailyRotation_Click(object sender, RoutedEventArgs e)
        {
            Action a = () =>
            {
                _mouse.ClickCenterScreen(_r.CenterScreen);
                Thread.Sleep(1000);

                if (_settings.GoOffline)
                    _uit.GoOffline();

                var charsToRun = UserSettings.Characters.Where(x => x.IsCharSelected).ToList();

                for (int i = 0; i < charsToRun.Count; i++)
                {
                    var character = charsToRun[i];
                    bool acceptWeeklies = ShouldAcceptWeeklies(character);
                    var success = RunCharacterDailyRotation(character, acceptWeeklies);

                    if (!success)
                    {
                        _logger.Log(LogDetailLevel.Info, $"Work Reported as Failure for {character.ClassName}, exiting daily rotation loop.");
                        break;
                    }


                    if (i == charsToRun.Count - 1)
                        break;

                    BackgroundProcessing.ProgressCheck();
                    if (_uit.SwapCharacters(charsToRun[i + 1]))
                    {
                        var backInTown = _uit.InAreaCheck();
                        if (!backInTown)
                            break;
                    }
                }

                if (UserSettings.QuitAfterFullRotation)
                    _uit.ExitGame();
            };

            CreateBackgroundWorker(a);
        }


        private void SaveUserSettings_Click(object sender, RoutedEventArgs e)
        {
            SaveUserSettings();
        }

        private void CancelUserSettings_Click(object sender, RoutedEventArgs e)
        {
            UserSettings = UserSettings.Read(USER_SETTINGS_PATH);
        }

        private void AddNewCharacter_Click(object sender, RoutedEventArgs e)
        {
            var newChar = new UserCharacter()
            {
                ClassName = "Unset",
                CharacterIndex = UserSettings.Characters.Count == 0 ? 1 : UserSettings.Characters.Max(x => x.CharacterIndex) + 1,
                ChaosLevel = 1660
            };
            CurrentSelectedEditChar = newChar;
            UserSettings.Characters.Add(newChar);
        }

        private void DeleteCharacter_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentSelectedEditChar != null)
            {
                UserSettings.Characters.Remove(CurrentSelectedEditChar);
                CurrentSelectedEditChar = null;
            }
        }

        private void CaptureSkills_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentSelectedEditChar != null)
            {

                foreach (var skill in CurrentSelectedEditChar.AllResolutionSkills[UserSettings.Resolution].AllSkills)
                {
                    if (skill == null) continue;

                    var region = (OpenCvSharp.Rect)_r.GetType().GetProperty($"Skill_{skill.SkillKey}").GetValue(_r);

                    if (skill.IsAwakening && CurrentSelectedEditChar.ChaosLevel >= 1640)
                        region = (OpenCvSharp.Rect)_r.GetType().GetProperty($"Skill_Hyper{skill.SkillKey}").GetValue(_r);

                    if (skill.SkillKey == "T" && CurrentSelectedEditChar.ChaosLevel < 1640)
                        continue;

                    var skillScreenshot = IP.CaptureScreen(region);

                    if (skillScreenshot != null)
                    {

                        using (var ms = new MemoryStream())
                        {
                            skillScreenshot.Save(ms, ImageFormat.Png);
                            var sig64 = Convert.ToBase64String(ms.GetBuffer());
                            skill.SkillImageEncoded = sig64;
                        }
                    }
                }
            }
        }

        private void StartWorker()
        {
            _blocked = false;
            _busy.Set();

            if (_bw != null && !_bw.IsBusy)
            {
                //_logger.Log(LogDetailLevel.Debug, "Listening for pause");
                _kb.Listen(Key.Pause, () =>
                {
                    _logger.Log(LogDetailLevel.Info, "Pause pressed");
                    TogglePause();
                    //CancelWorker();
                });
                _kb.Listen(Key.End, () =>
                {
                    _logger.Log(LogDetailLevel.Info, "End Processing Pressed");
                    CancelWorker();
                });
                _bw.RunWorkerAsync();
                TaskRunning = true;
            }
        }

        private bool _blocked = false;
        private DateTime _currentWorkEndTime;

        private void TogglePause()
        {
            if (_blocked)
                _busy.Set();
            else
                _busy.Reset();

            _blocked = !_blocked;
        }

        private void CancelWorker()
        {
            if (_bw.IsBusy)
            {
                //_logger.Log(LogDetailLevel.Debug, "Cancelling worker");
                _bw.CancelAsync();
                TaskRunning = false;

                // Unblock worker so it can see that
                _busy.Set();
            }
        }

        private void WorkCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            _currentWorkEndTime = DateTime.Now;
            _logger.WriteSessionWorkCompleted(_currentWorkStartTime, _currentWorkEndTime);
            _kb.StopListening(Key.Pause);
            _kb.StopListening(Key.End);
            TaskRunning = false;
        }

        private void CreateBackgroundWorker(Action workToDo)
        {
            _currentWorkStartTime = DateTime.Now;
            _bw = new BackgroundWorker();
            _bw.WorkerSupportsCancellation = true;
            _bw.DoWork += (o, e) =>
            {
                try
                {
                    BackgroundProcessing.SetVariables(_bw, _busy, e);
                    workToDo();
                }
                catch (BackgroundCancellationException)
                {
                    _logger.Log(LogDetailLevel.Debug, "Work Stopped");
                }
                catch (Exception ex)
                {
                    _logger.LogException(ex);
                }
            };
            _bw.RunWorkerCompleted += WorkCompleted;
            StartWorker();
        }

        private void Test_Click(object sender, RoutedEventArgs e)
        {
            //IP.SHOW_DEBUG_IMAGES = true;
            //var special = IP.LocateCenterOnScreen(Utility.ImageResourceLocation("class/soulfist.png", _settings.Resolution), _r.CharacterIcon, .7);
            //if (special.Found)
            //    _kb.Press(Key.Z, 200);
            //IP.SHOW_DEBUG_IMAGES = false;

            Action a = () =>
            {

                var okButton = IP.LocateCenterOnScreen(Utility.ImageResourceLocation($"ok_button.png", _settings.Resolution), confidence: .99, breakAfterFirst: true, maxTries: 3);
                if (okButton.Found)
                {
                    _logger.Log(LogDetailLevel.Debug, $"Item OK Button found.  Confidence: {okButton.MaxConfidence}");
                    _mouse.ClickPosition(okButton.Center, 1000);
                }
            };

            CreateBackgroundWorker(a);
        }

        private void LogDetailLevel_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _log.CurrentLoggingLevel = UserSettings.LogDetailLevel;
            _log.RefreshVisibleEntries();
        }

        private void LogView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var listView = (ListView)sender;
            if (VisualTreeHelper.GetChildrenCount(listView) > 0)
            {
                Border border = (Border)VisualTreeHelper.GetChild(listView, 0);
                ScrollViewer scrollViewer = (ScrollViewer)VisualTreeHelper.GetChild(border, 0);
                scrollViewer.ScrollToBottom();
            }
        }


        private void LogEntriesChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            CurrentLogEntry = _log.LogEntries.LastOrDefault();
        }

        private void SelectAll_Click(object sender, RoutedEventArgs e)
        {
            foreach (var character in _settings.Characters)
                character.IsCharSelected = true;
        }

        private void SelectNone_Click(object sender, RoutedEventArgs e)
        {
            foreach (var character in _settings.Characters)
                character.IsCharSelected = false;
        }

        private void ClearLog_Click(object sender, RoutedEventArgs e)
        {
            _logger.ClearLog();
        }

        private void KF_Click(object sender, RoutedEventArgs e)
        {
            foreach (var character in _settings.Characters)
                character.IsCharSelected = character.ChaosLevel >= 1640;
        }

        private void CD_Click(object sender, RoutedEventArgs e)
        {
            foreach (var character in _settings.Characters)
                character.IsCharSelected = character.ChaosLevel < 1640;
        }

        private void TabControl_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.End)
            {
                e.Handled = true;
            }
        }

        private void PauseExecution_Click(object sender, RoutedEventArgs e)
        {
            TogglePause();
        }

        private void CancelExecution_Click(object sender, RoutedEventArgs e)
        {
            CancelWorker();
        }

    }
}