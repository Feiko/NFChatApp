using System;
using System.Diagnostics;
using System.Threading;
using System.Net;
using nanoFramework.WebServer;
using nanoFramework.Networking;
using System.Device.WiFi;
using System.Net.NetworkInformation;
using nanoFramework.Runtime.Native;


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
            Debug.WriteLine(IPAddress.GetDefaultLocalAddress().ToString());

            //Wireless80211.Disable();
            //if (!WirelessAP.Setup())
            //{
            //    Debug.WriteLine($"Setup Soft AP, Rebooting device");
            //    Power.RebootDevice();
            //}

            //Debug.WriteLine($"Running Soft AP, waiting for client to connect");
            //Debug.WriteLine($"Soft AP IP address :{WirelessAP.GetIP()}");


            ChatWebSocketServer.Start();

            using (WebServer server = new WebServer(80, HttpProtocol.Http, new Type[] { typeof(ControllerChat) }))
            {
                server.Start();
                Debug.WriteLine("connet to http://" + IPAddress.GetDefaultLocalAddress().ToString() + "/chat");
                Thread.Sleep(Timeout.Infinite);
            }

            Thread.Sleep(Timeout.Infinite);

            // Browse our samples repository: https://github.com/nanoframework/samples
            // Check our documentation online: https://docs.nanoframework.net/
            // Join our lively Discord community: https://discord.gg/gCyBu8T
        }
    }
}
