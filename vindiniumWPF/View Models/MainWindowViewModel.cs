using Common.Configuration;
using Common.Helpers;
using Common.Messaging.Messages;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using vindiniumWPF.Helpers;
using vindiniumWPF.Messaging;
using vindiniumWPF.Views;

namespace vindiniumWPF.View_Models
{
    class MainWindowViewModel : ViewModelBase
    {
        #region Members
        public ICommand ExitCommand { get; set; }
        public ICommand ShowDetailsWindowCommand { get; set; }
        #endregion

        public MainWindowViewModel()
        {
            ExitCommand = new RelayCommand((_) =>
            {
                Application.Current.Shutdown();
            });

            ShowDetailsWindowCommand = new RelayCommand((_) =>
            {
                EventAggregator.Instance.ProcessMessage(new ConfigRequestMessage()
                    {
                        RequestedSection = AvailableSections.GameSettings,
                        Callback = ConfigSectionDetailsReceived
                    });
            });
        }

        private void ConfigSectionDetailsReceived(ConfigurationSection configSection)
        {
            var gameSettings = configSection as GameSettingsConfiguration;

            if (gameSettings != null)
            {
                DetailsWindow detailsWindow = new DetailsWindow();
                DetailsWindowViewModel detailsWindowVM = new DetailsWindowViewModel(gameSettings);
                detailsWindow.DataContext = detailsWindowVM;

                detailsWindow.ShowDialog();

                if (detailsWindowVM.SettingsChanged())
                {
                    EventAggregator.Instance.ProcessMessage(new GameSettingsConfigUpdated
                        {
                            NumberOfTurns = detailsWindowVM.NumberOfTurns,
                            PrivateKey = detailsWindowVM.PrivateKey,
                            ServerURL = detailsWindowVM.ServerURL
                        });
                }
            }
            else
            {
                Log4netManager.ErrorFormat("Received a Config Section that could not be cast to a Game Settings Configuration section, unable to show Details Window", GetType());
            }
        }
    }
}
