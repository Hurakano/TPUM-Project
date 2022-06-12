using System;
using System.Threading.Tasks;
using System.Threading;
using LibraryServer.BusinessLogicLayer;

namespace LibraryServer.ServerPresentation
{
    class Program
    {
        static async Task Main(string[] args)
        {
            CommandHandler.LibraryLogic = LibraryLogicFactory.Create(1);
            await WebServer.Server(8080, CommandHandler.OnConnectionSocket, new CancellationToken());
        }
    }
}
