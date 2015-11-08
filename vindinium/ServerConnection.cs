using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using vindinium.Interfaces;

namespace vindinium
{
    class ServerConnection
    {
        #region Members
        private string key;
        private string serverURL;
        public bool Errored { get; private set; }
        public string ErrorText { get; private set; }
        public event EventHandler<string> DataReceived = delegate { };
        #endregion

        public ServerConnection()
        {
            key = ConfigManager.GetConfigKey("ID");
            serverURL = ConfigManager.GetConfigKey("ServerURL");
        }

        public void CreateGame(bool trainingMode, int numberOfTurns)
        {
            Errored = false;

            if (trainingMode)
            {
                serverURL += "/api/training";
            }
            else
            {
                serverURL += "/api/arena";
            }

            string myParameters = "key=" + key;
            if (trainingMode) myParameters += "&turns=" + numberOfTurns;

            using (WebClient client = new WebClient())
            {
                client.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                try
                {
                    string result = client.UploadString(serverURL, myParameters);
                    DataReceived(this, result);
                }
                catch (WebException exception)
                {
                    Errored = true;
                    using (var reader = new StreamReader(exception.Response.GetResponseStream()))
                    {
                        ErrorText = reader.ReadToEnd();
                    }

                    Console.WriteLine(ErrorText);
                    Console.ReadLine();
                }
            }
        }

        public void SendCommand(string playUrl, string direction)
        {
            string myParameters = "key=" + key + "&dir=" + direction;

            using (WebClient client = new WebClient())
            {
                client.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";

                try
                {
                    string result = client.UploadString(playUrl, myParameters);
                    DataReceived(this, result);
                }
                catch (WebException exception)
                {
                    Errored = true;
                    using (var reader = new StreamReader(exception.Response.GetResponseStream()))
                    {
                        ErrorText = reader.ReadToEnd();
                    }

                    Console.WriteLine(ErrorText);
                    Console.ReadLine();
                }
            }
        }
    }
}