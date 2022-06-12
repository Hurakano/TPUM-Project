/* ========================================================================
 * Copyright (c) 2005-2021 The OPC Foundation, Inc. All rights reserved.
 *
 * OPC Foundation MIT License 1.00
 *
 * Permission is hereby granted, free of charge, to any person
 * obtaining a copy of this software and associated documentation
 * files (the "Software"), to deal in the Software without
 * restriction, including without limitation the rights to use,
 * copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the
 * Software is furnished to do so, subject to the following
 * conditions:
 *
 * The above copyright notice and this permission notice shall be
 * included in all copies or substantial portions of the Software.
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
 * EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
 * OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
 * NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
 * HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
 * WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
 * FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
 * OTHER DEALINGS IN THE SOFTWARE.
 *
 * The complete license agreement can be found here:
 * http://opcfoundation.org/License/MIT/1.00/
 * ======================================================================*/

using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Xml;
using System.Runtime.Serialization;
using Opc.Ua;

namespace LibraryData
{
    #region ObjectType Identifiers
    /// <summary>
    /// A class that declares constants for all ObjectTypes in the Model Design.
    /// </summary>
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    public static partial class ObjectTypes
    {
        /// <summary>
        /// The identifier for the Book ObjectType.
        /// </summary>
        public const uint Book = 1;

        /// <summary>
        /// The identifier for the Reader ObjectType.
        /// </summary>
        public const uint Reader = 5;

        /// <summary>
        /// The identifier for the Loan ObjectType.
        /// </summary>
        public const uint Loan = 8;
    }
    #endregion

    #region Variable Identifiers
    /// <summary>
    /// A class that declares constants for all Variables in the Model Design.
    /// </summary>
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    public static partial class Variables
    {
        /// <summary>
        /// The identifier for the Book_Id Variable.
        /// </summary>
        public const uint Book_Id = 2;

        /// <summary>
        /// The identifier for the Book_Title Variable.
        /// </summary>
        public const uint Book_Title = 3;

        /// <summary>
        /// The identifier for the Book_Author Variable.
        /// </summary>
        public const uint Book_Author = 4;

        /// <summary>
        /// The identifier for the Reader_Id Variable.
        /// </summary>
        public const uint Reader_Id = 6;

        /// <summary>
        /// The identifier for the Reader_Name Variable.
        /// </summary>
        public const uint Reader_Name = 7;

        /// <summary>
        /// The identifier for the Loan_Id Variable.
        /// </summary>
        public const uint Loan_Id = 9;

        /// <summary>
        /// The identifier for the Loan_BookId Variable.
        /// </summary>
        public const uint Loan_BookId = 10;

        /// <summary>
        /// The identifier for the Loan_ReaderId Variable.
        /// </summary>
        public const uint Loan_ReaderId = 11;

        /// <summary>
        /// The identifier for the Loan_BorrowDate Variable.
        /// </summary>
        public const uint Loan_BorrowDate = 12;

        /// <summary>
        /// The identifier for the Loan_ReturnDate Variable.
        /// </summary>
        public const uint Loan_ReturnDate = 13;
    }
    #endregion

    #region ObjectType Node Identifiers
    /// <summary>
    /// A class that declares constants for all ObjectTypes in the Model Design.
    /// </summary>
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    public static partial class ObjectTypeIds
    {
        /// <summary>
        /// The identifier for the Book ObjectType.
        /// </summary>
        public static readonly ExpandedNodeId Book = new ExpandedNodeId(LibraryData.ObjectTypes.Book, LibraryData.Namespaces.LibraryData);

        /// <summary>
        /// The identifier for the Reader ObjectType.
        /// </summary>
        public static readonly ExpandedNodeId Reader = new ExpandedNodeId(LibraryData.ObjectTypes.Reader, LibraryData.Namespaces.LibraryData);

        /// <summary>
        /// The identifier for the Loan ObjectType.
        /// </summary>
        public static readonly ExpandedNodeId Loan = new ExpandedNodeId(LibraryData.ObjectTypes.Loan, LibraryData.Namespaces.LibraryData);
    }
    #endregion

    #region Variable Node Identifiers
    /// <summary>
    /// A class that declares constants for all Variables in the Model Design.
    /// </summary>
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    public static partial class VariableIds
    {
        /// <summary>
        /// The identifier for the Book_Id Variable.
        /// </summary>
        public static readonly ExpandedNodeId Book_Id = new ExpandedNodeId(LibraryData.Variables.Book_Id, LibraryData.Namespaces.LibraryData);

        /// <summary>
        /// The identifier for the Book_Title Variable.
        /// </summary>
        public static readonly ExpandedNodeId Book_Title = new ExpandedNodeId(LibraryData.Variables.Book_Title, LibraryData.Namespaces.LibraryData);

        /// <summary>
        /// The identifier for the Book_Author Variable.
        /// </summary>
        public static readonly ExpandedNodeId Book_Author = new ExpandedNodeId(LibraryData.Variables.Book_Author, LibraryData.Namespaces.LibraryData);

        /// <summary>
        /// The identifier for the Reader_Id Variable.
        /// </summary>
        public static readonly ExpandedNodeId Reader_Id = new ExpandedNodeId(LibraryData.Variables.Reader_Id, LibraryData.Namespaces.LibraryData);

        /// <summary>
        /// The identifier for the Reader_Name Variable.
        /// </summary>
        public static readonly ExpandedNodeId Reader_Name = new ExpandedNodeId(LibraryData.Variables.Reader_Name, LibraryData.Namespaces.LibraryData);

        /// <summary>
        /// The identifier for the Loan_Id Variable.
        /// </summary>
        public static readonly ExpandedNodeId Loan_Id = new ExpandedNodeId(LibraryData.Variables.Loan_Id, LibraryData.Namespaces.LibraryData);

        /// <summary>
        /// The identifier for the Loan_BookId Variable.
        /// </summary>
        public static readonly ExpandedNodeId Loan_BookId = new ExpandedNodeId(LibraryData.Variables.Loan_BookId, LibraryData.Namespaces.LibraryData);

        /// <summary>
        /// The identifier for the Loan_ReaderId Variable.
        /// </summary>
        public static readonly ExpandedNodeId Loan_ReaderId = new ExpandedNodeId(LibraryData.Variables.Loan_ReaderId, LibraryData.Namespaces.LibraryData);

        /// <summary>
        /// The identifier for the Loan_BorrowDate Variable.
        /// </summary>
        public static readonly ExpandedNodeId Loan_BorrowDate = new ExpandedNodeId(LibraryData.Variables.Loan_BorrowDate, LibraryData.Namespaces.LibraryData);

        /// <summary>
        /// The identifier for the Loan_ReturnDate Variable.
        /// </summary>
        public static readonly ExpandedNodeId Loan_ReturnDate = new ExpandedNodeId(LibraryData.Variables.Loan_ReturnDate, LibraryData.Namespaces.LibraryData);
    }
    #endregion

    #region BrowseName Declarations
    /// <summary>
    /// Declares all of the BrowseNames used in the Model Design.
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    public static partial class BrowseNames
    {
        /// <summary>
        /// The BrowseName for the Author component.
        /// </summary>
        public const string Author = "Author";

        /// <summary>
        /// The BrowseName for the Book component.
        /// </summary>
        public const string Book = "Book";

        /// <summary>
        /// The BrowseName for the BookId component.
        /// </summary>
        public const string BookId = "BookId";

        /// <summary>
        /// The BrowseName for the BorrowDate component.
        /// </summary>
        public const string BorrowDate = "BorrowDate";

        /// <summary>
        /// The BrowseName for the Id component.
        /// </summary>
        public const string Id = "Id";

        /// <summary>
        /// The BrowseName for the Loan component.
        /// </summary>
        public const string Loan = "Loan";

        /// <summary>
        /// The BrowseName for the Name component.
        /// </summary>
        public const string Name = "Name";

        /// <summary>
        /// The BrowseName for the Reader component.
        /// </summary>
        public const string Reader = "Reader";

        /// <summary>
        /// The BrowseName for the ReaderId component.
        /// </summary>
        public const string ReaderId = "ReaderId";

        /// <summary>
        /// The BrowseName for the ReturnDate component.
        /// </summary>
        public const string ReturnDate = "ReturnDate";

        /// <summary>
        /// The BrowseName for the Title component.
        /// </summary>
        public const string Title = "Title";
    }
    #endregion

    #region Namespace Declarations
    /// <summary>
    /// Defines constants for all namespaces referenced by the model design.
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    public static partial class Namespaces
    {
        /// <summary>
        /// The URI for the cas namespace (.NET code namespace is '').
        /// </summary>
        public const string cas = "http://cas.eu/UA/CommServer/";

        /// <summary>
        /// The URI for the OpcUa namespace (.NET code namespace is 'Opc.Ua').
        /// </summary>
        public const string OpcUa = "http://opcfoundation.org/UA/";

        /// <summary>
        /// The URI for the OpcUaXsd namespace (.NET code namespace is 'Opc.Ua').
        /// </summary>
        public const string OpcUaXsd = "http://opcfoundation.org/UA/2008/02/Types.xsd";

        /// <summary>
        /// The URI for the LibraryData namespace (.NET code namespace is 'LibraryData').
        /// </summary>
        public const string LibraryData = "http://libraryProject.com/TPUM-Project/";
    }
    #endregion
}