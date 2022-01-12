using nanoFramework.WebServer;
using System;
using System.Text;
using System.Collections;
using nanoFramework.Json;
using System.Net;

namespace NFChatApp
{
    internal class ControllerChat
    {
        [Route("Chat")]
        [Method("GET")]
        public void Get(WebServerEventArgs e)
        {
            CreateChatLoginPortal(e.Context);
        }


        [Route("Chat")]
        [Method("POST")]
        public void Post(WebServerEventArgs e)
        {
            byte[] buff = new byte[e.Context.Request.ContentLength64];
            e.Context.Request.InputStream.Read(buff, 0, buff.Length);
            string rawData = Encoding.UTF8.GetString(buff, 0, buff.Length);
            CreateChatSite(e.Context, rawData);

        }

        private static void CreateChatLoginPortal(HttpListenerContext context)
        {
            var friends = ChatWebSocketServer.WebSocketUsers.Values;
            string friendsHtml = "<h3>Want to have some fun with friends in the chat? The following friends are already logged in:<h3><ul>";
            foreach (string friend in friends)
            {
                friendsHtml += $"<li>{friend}</li>";
            }
            friendsHtml += "</ul>";

            string html = $@"
<!DOCTYPE html>
<html>
    <head>
        <title>BOMBSHETLER Chat</title>
    </head>
    <body>
        <h1>Welcome to BOMBSHELTER Chat,</h1> 
        {(friends.Length == 0 ? "<h3>Want to have some fun with friends in the chat? The following friends are already logged in:</h3>" : friendsHtml)}
        <br>
        <p>Please submit your name to joint the chat<p>
        <form action=""/chat"">
            <label for=""name"">Name:</label><br>
            <input type=""text"" id=""fname"" name=""fname""><br>
            <input type=""submit"" value=""Submit"">
        </form>
    </body>
</html>
              
";

            context.Response.ContentType = "text/html";
            context.Response.ContentLength64 = html.Length;
            WebServer.OutPutStream(context.Response, html);
        }



        private static void CreateChatSite(HttpListenerContext context, string name)
        {
            string html = ($@"
<!DOCTYPE html>
<html>
    <head>
        <title>Chat</title>
    </head>
    <body>
        <h1>WebSocket Chat</h1>
        <form action="""" onsubmit=""sendMessage(event)"">
            <input type=""text"" id=""messageText"" autocomplete=""off""/>
            <button>Send</button>
        </form>
        <ul id='messages'>
        </ul>
        <script>
            var ws = new WebSocket(""ws://"" + location.hostname + ""/chat"", ""{name}"");
        ws.onmessage = function(event) {{
            var messages = document.getElementById('messages')
                var message = document.createElement('li')
                var content = document.createTextNode(event.data)
                message.appendChild(content)
                messages.appendChild(message)
            }};
    function sendMessage(event)
    {{
        var input = document.getElementById(""messageText"")
                ws.send(input.value)
                input.value = ''
                event.preventDefault()
            }}
        </script>
    </body>
</html>

");
            context.Response.ContentType = "text/html";
            context.Response.ContentLength64 = html.Length;
            WebServer.OutPutStream(context.Response, html);
        }
    }
}
