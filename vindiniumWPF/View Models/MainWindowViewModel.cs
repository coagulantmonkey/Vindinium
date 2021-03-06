﻿using Common.Configuration;
using Common.Helpers;
using Common.Messaging;
using Common.Messaging.Messages;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Windows;
using System.Windows.Input;
using Common.Views;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace Common.View_Models
{
    class MainWindowViewModel : ViewModelBase, IMessageConsumer
    {
        private readonly Log4netManager _logger;

        public ICommand ExitCommand { get; set; }
        public ICommand ShowDetailsWindowCommand { get; set; }
        public ICommand StartNewGameCommand { get; set; }
        public ObservableCollection<TabItem> Tabs { get; set; }

        public List<Type> GetMessageTypesHandled()
        {
            throw new NotImplementedException();
        }

        public void RegisterAggregator(EventAggregator aggregator)
        {
            throw new NotImplementedException();
        }

        public MainWindowViewModel()
        {
            EventAggregator.Instance.RegisterAsListener(typeof(NewGameStartedMessage), this);

            Tabs = new ObservableCollection<TabItem>();

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

            StartNewGameCommand = new RelayCommand((_) =>
            {
                EventAggregator.Instance.ProcessMessage(new ConfigRequestMessage()
                    {
                        RequestedSection = AvailableSections.GameSettings,
                        Callback = StartNewGame
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
                            ServerURL = detailsWindowVM.ServerURL,
                            TrainingMode = detailsWindowVM.TrainingMode
                        });
                }
            }
            else
            {
                _logger.ErrorFormat("Received a Config Section that could not be cast to a Game Settings Configuration section, unable to show Details Window", GetType());
            }
        }

        private void StartNewGame(ConfigurationSection configSection)
        {
            var gameSettings = configSection as GameSettingsConfiguration;

            if (gameSettings != null)
            {
                EventAggregator.Instance.ProcessMessage(new StartNewGameMessage
                    {
                        NumberOfTurns = gameSettings.NumberOfTurns,
                        PrivateKey = gameSettings.PrivateKey,
                        ServerURL = gameSettings.ServerURL,
                        TrainingMode = gameSettings.TrainingMode
                    });
            }
            else
            {
                _logger.ErrorFormat("Received a Config Section that could not be cast to a Game Settings Configuration section, unable to start a new game",
                    GetType());
            }
        }

        public void ProcessMessage(InternalMessage message)
        {
            if (message is NewGameStartedMessage)
            {
                _logger.DebugFormat("NewGameStartedMessage received.", typeof(MainWindowViewModel));
                NewGameStarted(message as NewGameStartedMessage);
            }
        }
        private void NewGameStarted(NewGameStartedMessage message)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                TabItem tabItem = new TabItem();
                tabItem.Header = message.ViewUrl;
                WebBrowser webBrowser = new WebBrowser();
                tabItem.Content = webBrowser;
                webBrowser.Source = new Uri(message.ViewUrl);

                Tabs.Add(tabItem);
            });
        }
    }
}
