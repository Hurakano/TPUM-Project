using System;
using System.Collections.Generic;
using System.Text.Json;

namespace LibraryClient.LibraryClientData
{
    public static class DataSerializer
    {
        public static string ListBookToJson(List<Book> books)
        {
            return JsonSerializer.Serialize(books);
        }

        public static List<Book> JsonToListBook(string json)
        {
            List<BookImpl> books = JsonSerializer.Deserialize<List<BookImpl>>(json);
            return new List<Book>(books!);
        }

        public static string ListReaderToJson(List<Reader> readers)
        {
            return JsonSerializer.Serialize(readers);
        }

        public static List<Reader> JsonToListReader(string json)
        {
            List<ReaderImpl> readers = JsonSerializer.Deserialize<List<ReaderImpl>>(json);
            return new List<Reader>(readers!);
        }

        public static string ListLoanToJson(List<Loan> loans)
        {
            return JsonSerializer.Serialize(loans);
        }

        public static List<Loan> JsonToListLoan(string json)
        {
            List<LoanImpl> loans = JsonSerializer.Deserialize<List<LoanImpl>>(json);
            return new List<Loan>(loans!);
        }
    }
}
