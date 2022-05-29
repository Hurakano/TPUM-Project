using System;
using System.Collections.Generic;
using System.Text;
using LibraryClient.LibraryClientLogic;

namespace PresentationLayer.LibraryModel
{
    public class OverdueReporter : IObserver<List<LoanDTO>>
    {
        public event EventHandler OverdueEvent;

        public void OnCompleted()
        {

        }

        public void OnError(Exception error)
        {

        }

        public void OnNext(List<LoanDTO> value)
        {
            List<Guid> ids = new List<Guid>();
            foreach(LoanDTO loan in value)
            {
                ids.Add(loan.Id);
            }
            OverdueEvent?.Invoke(this, new OverdueLoansEventArgs { LoanIds = ids });
        }
    }

    public class OverdueLoansEventArgs : EventArgs
    {
        public List<Guid> LoanIds { get; set; }
    }
}

