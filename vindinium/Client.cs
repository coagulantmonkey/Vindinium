using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vindinium.Interfaces;

namespace vindinium
{
    class Client
    {
        static bool watchGameLive;
        static void Main(string[] args)
        {
            watchGameLive = bool.Parse(ConfigManager.GetConfigKey("WatchGame"));
            IAIManager bot = new AIManager();
            bot.ViewUrlChanged += bot_ViewUrlChanged;
            bot.Run();            
        }

        private static void bot_ViewUrlChanged(object sender, string e)
        {
            if (watchGameLive)
            {
                Task.Factory.StartNew(() =>
                    {
                        System.Diagnostics.Process.Start(e);
                    });
            }
        }
    }
}
