using System;
using System.Threading.Tasks;
using LibraryServer.BusinessLogicLayer;

namespace LibraryServer.ServerPresentation
{
    class Program
    {
        static async Task Main(string[] args)
        {
            CommandParser.LibraryLogic = LibraryLogic.Create(1);
            await WebServer.Server(8080, CommandParser.OnConnectionSocket);
        }
    }
}
