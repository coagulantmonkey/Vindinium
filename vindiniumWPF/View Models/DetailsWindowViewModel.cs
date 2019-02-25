using Common.Configuration;

namespace Common.View_Models
{
    public class DetailsWindowViewModel : ViewModelBase
    {
        private GameSettingsConfiguration initialSettings;

        public string PrivateKey { get; set; }
        public int NumberOfTurns { get; set; }
        public string ServerURL { get; set; }
        public bool TrainingMode { get; set; }

        public DetailsWindowViewModel()
        {
        }

        public DetailsWindowViewModel(GameSettingsConfiguration gameSettings)
        {
            PrivateKey = gameSettings.PrivateKey;
            NumberOfTurns = gameSettings.NumberOfTurns;
            ServerURL = gameSettings.ServerURL;
            TrainingMode = gameSettings.TrainingMode;

            initialSettings = gameSettings;
        }

        public bool SettingsChanged()
        {
            return PrivateKey != initialSettings.PrivateKey
                || NumberOfTurns != initialSettings.NumberOfTurns
                || ServerURL != initialSettings.ServerURL
                || TrainingMode != initialSettings.TrainingMode;
        }
    }
}
