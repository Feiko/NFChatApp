using nanoFramework.WebServer;
using System;
using System.Text;
using System.Net;

namespace NFChatApp
{
    internal class ControllerChat
    {
        [Route("chat")]
        [Method("GET")]
        public void Get(WebServerEventArgs e)
        {
            if(e.Context.Request.Headers["Upgrade"] == "websocket") //is a websocket request
            {
                if (!ChatWebSocketServer.AddClient(e.Context)) WebServer.OutputHttpCode(e.Context.Response, HttpStatusCode.BadRequest);
            }

            CreateChatLoginPortal(e.Context);
        }


        [Route("chat")]
        [Method("POST")]
        public void Post(WebServerEventArgs e)
        {
            string[] content = GetRequestContent(e.Context).Split('=');
            if (content.Length > 1 && content[0].ToLower() == "name" && !string.IsNullOrEmpty(content[1]))
            {
                CreateChatSite(e.Context, content[1]);
            }
            else
            {
                WebServer.OutputHttpCode(e.Context.Response, HttpStatusCode.BadRequest);
            }

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
        {(friends.Length == 0 ? "<h3>Start the fun and be the first to enter the BOMBSHELTER Chat</h3>" : friendsHtml)}
        <br>
        <p>Please submit your name to joint the chat<p>
        <form action=""/chat"", method=""post"">
            <label for=""name"">Name:</label><br>
            <input type=""text"" id=""name"" name=""name""><br>
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
        <title>BOMBSHELTER Chat</title>
    </head>
    <body>
        <h1>WebSocket BOMBSHELTER Chat - User {name}</h1>
        <form action="""" onsubmit=""sendMessage(event)"">
            <input type=""text"" id=""messageText"" autocomplete=""off""/>
            <button>Send</button>
        </form>
        <ul id='messages'>
        </ul>
        <script>
            var ws = new WebSocket(""ws://"" + location.hostname + "":8080/chat"", ""{name}"");
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

        private static string GetRequestContent(HttpListenerContext context)
        {
            byte[] buff = new byte[context.Request.ContentLength64];
            context.Request.InputStream.Read(buff, 0, buff.Length);
            return Encoding.UTF8.GetString(buff, 0, buff.Length);
        }
    }
}
