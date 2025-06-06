using Chaotic.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.TextFormatting;

namespace Chaotic.User
{
    public class UserSettings : INotifyPropertyChanged
    {
        public UserSettings()
        {
            Characters = new ObservableCollection<UserCharacter>();
        }


        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }



        public LogDetailLevel LogDetailLevel { get; set; }

        public string Resolution { get; set; } = "3440x1440";
        public bool QuitAfterFullRotation { get; set; }
        public int UsePotionPercent { get; set; } = 40;
        public string HealthPotionKey { get; set; } = "F1";
        public bool EnableAura { get; set; }
        public bool RepairGear { get; set; }
        public bool MoveHoningMaterials { get; set; }
        public bool MoveGems { get; set; }
        public int GemLevelThreshold { get; set; }
        public double PerformanceMultiplier { get; set; } = 1;

        public bool CaptureTimeoutScreenshot { get; set; } = true;

        private bool _PreferKeyboardShortcuts = false;
        public bool PreferKeyboardShortcuts
        {
            get { return _PreferKeyboardShortcuts; }
            set
            {
                _PreferKeyboardShortcuts = value;
                OnPropertyChanged();
            }
        }


        public UserKeyboardKey InteractShortcutKey { get; set; }
        public UserKeyboardKey FastInteractShortcutKey { get; set; }
        public UserKeyboardKey Special1ShortcutKey { get; set; }
        public UserKeyboardKey Special2ShortcutKey { get; set; }
        public UserKeyboardKey GuildShortcutKey { get; set; }
        public UserKeyboardKey FriendsShortcutKey { get; set; }
        public UserKeyboardKey PetShortcutKey { get; set; }
        public UserKeyboardKey ContentShortcutKey { get; set; }
        public UserKeyboardKey UnaShortcutKey { get; set; }
        public UserKeyboardKey BifrostShortcutKey { get; set; }
        public UserKeyboardKey MusicShortcutKey { get; set; }
        public UserKeyboardKey ProfileShortcutKey { get; set; }


        public bool GoOffline { get; set; } = true;

        public DateTime? LastWeeklyReset { get; set; }
        public ObservableCollection<UserCharacter> Characters { get; set; }

        public void Save(string fileName)
        {
            var sb = new StringBuilder();

            using (var sw = new StringWriter(sb))
            {
                using (JsonTextWriter jw = new(sw))
                {
                    JsonSerializer serializer = new JsonSerializer() { Formatting = Formatting.Indented };
                    serializer.Serialize(jw, this);
                }
            }

            File.WriteAllText(fileName, sb.ToString());
        }

        public UserSettings Read(string fileName)
        {
            try
            {
                using (StreamReader sr = new StreamReader(fileName))
                {
                    using (var tr = new JsonTextReader(sr))
                    {
                        var serializer = new JsonSerializer();
                        return serializer.Deserialize<UserSettings>(tr);
                    }
                }
            }
            catch (FileNotFoundException fnfe)
            {
                this.Save(fileName);
                return this.Read(fileName);
            }

        }
    }
}
