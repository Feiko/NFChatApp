using System;
using System.Diagnostics;
using System.Threading;
using System.Net;
using nanoFramework.WebServer;
using nanoFramework.Networking;

namespace NFChatApp
{
    public class Program
    {
        public static void Main()
        {
            Debug.WriteLine("Hello from nanoFramework!");

            const string Ssid = "testnetwork";
            const string Password = "securepassword1!";
            // Give 60 seconds to the wifi join to happen
            CancellationTokenSource cs = new(60000);


            var success = WiFiNetworkHelper.ScanAndConnectDhcp(Ssid, Password, token: cs.Token);
            if (!success)
            {
                // Something went wrong, you can get details with the ConnectionError property:
                Debug.WriteLine($"Can't connect to the network, error"); //TODO the right connection method wiht error 
            }
            else
            {

               Debug.WriteLine("connet to http://" + IPAddress.GetDefaultLocalAddress().ToString());
            }

            ChatWebSocketServer.Start();

            using(WebServer server = new WebServer(80, HttpProtocol.Http, new Type[] { typeof(ControllerChat) }))
            {
                server.Start();
                Thread.Sleep(Timeout.Infinite);
            }


            // Browse our samples repository: https://github.com/nanoframework/samples
            // Check our documentation online: https://docs.nanoframework.net/
            // Join our lively Discord community: https://discord.gg/gCyBu8T
        }
    }
}
