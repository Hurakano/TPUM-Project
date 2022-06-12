using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using LibraryServer.ServerPresentation;
using LibraryClient.LibraryClientData;
using System.Collections.Generic;
using System.Threading;

namespace IntegrationTest
{
    [TestClass]
    public class IntegrationTest
    {
        [TestMethod]
        public async Task ConnectionTest()
        {
            CancellationTokenSource serverToken = new CancellationTokenSource();
            Task serverTask = WebServer.Server(8080, null, serverToken.Token);
            Task connectionDone = null;
            IClientData client = new ClientData(new ClientWebSocketAdapter(), 8080, ref connectionDone);
            await connectionDone;
            int timeout = 100;
            while(!client.SocketConnected && timeout > 0)
            {
                await Task.Delay(10);
                timeout--;
            }

            Assert.IsTrue(client.SocketConnected);

            await client.Dissconnect();
            serverToken.Cancel();
            WebServer.StopServer();
            await serverTask;
            serverTask.Dispose();
            Assert.IsFalse(client.SocketConnected);
        }

        [TestMethod]
        public async Task DataSendTest()
        {
            List<string> messages = new List<string>();
            CancellationTokenSource serverToken = new CancellationTokenSource();
            Task serverTask = WebServer.Server(8081, x => x.OnMessage = (data) => messages.Add(data), serverToken.Token);
            Task connectionDone = null;
            IClientData client = new ClientData(new ClientWebSocketAdapter(), 8081, ref connectionDone);
            await connectionDone;
            int timeout = 100;
            while (!client.SocketConnected && timeout > 0)
            {
                await Task.Delay(10);
                timeout--;
            }
            Assert.IsTrue(client.SocketConnected);

            await client.Dissconnect();
            serverToken.Cancel();
            WebServer.StopServer();
            await serverTask;
            serverTask.Dispose();
            Assert.IsFalse(client.SocketConnected);
        }
    }
}
