using System;
using System.Collections.Generic;
using System.Text;
using BusinessLogicLayer;

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
            OverdueEvent?.Invoke(this, new OverdueLoansEventArgs { Loans = value });
        }
    }

    public class OverdueLoansEventArgs : EventArgs
    {
        public List<LoanDTO> Loans { get; set; }
    }
}

