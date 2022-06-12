using System;
using System.Collections.Generic;
using LibraryServer.BusinessLogicLayer;
using System.Text.Json;

namespace LibraryServer.ServerPresentation
{
    static class DataSerializer
    {
        public static string BookToJson(BookDTO book)
        {
            return JsonSerializer.Serialize(book);
        }

        public static BookDTO JsonToBook(string json)
        {
            return JsonSerializer.Deserialize<BookDTOImpl>(json);
        }

        public static string ListBookToJson(List<BookDTO> books)
        {
            return JsonSerializer.Serialize(books);
        }

        public static List<BookDTO> JsonToListBooks(string json)
        {
            List<BookDTOImpl> books = JsonSerializer.Deserialize<List<BookDTOImpl>>(json);
            return new List<BookDTO>(books!);
        }

        public static string ListReadersToJson(List<ReaderDTO> readers)
        {
            return JsonSerializer.Serialize(readers);
        }

        public static string LoanToJson(LoanDTO loan)
        {
            return JsonSerializer.Serialize(loan);
        }

        public static string ListLoanToJson(List<LoanDTO> loans)
        {
            return JsonSerializer.Serialize(loans);
        }
    }
}
