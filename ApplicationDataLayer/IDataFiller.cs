using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationDataLayer
{
    public interface IDataFiller
    {
        void FillData(ILibraryData data);
    }
}
