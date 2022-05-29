using System;
using System.Collections.Generic;
using System.Text;

namespace PresentationLayer.LibraryModel
{
    public abstract class AbstractLibraryModel: IDisposable
    {
        public OverdueReporter OverdueWatcher;
        protected IDisposable ObserverStopper;
        public event EventHandler<bool> OnTransactionResult;
        public event EventHandler<int> OnDataUpdated;

        public abstract List<ReaderPresenter> GetReaders();
        public abstract List<LoanPresenter> GetLoansByReader(Guid readerId);
        public abstract LoanPresenter GetLoanById(Guid loanId);
        public abstract List<BookPresenter> GetAvailableBooks();
        public abstract void ReturnBook(Guid bookId);
        public abstract void BorrowBook(Guid readerId, Guid bookId, DateTime timeOffset);

        void IDisposable.Dispose()
        {
            ObserverStopper.Dispose();
        }

        public static AbstractLibraryModel GetModel()
        {
            return new LibraryModel();
        }

        protected void DataUpdatedHandler(object sender, int args)
        {
            OnDataUpdated?.Invoke(this, args);
        }

        protected void HandleTransactionResult(object sender, bool arg)
        {
            OnTransactionResult?.Invoke(this, arg);
        }
    }
}
