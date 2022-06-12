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
using System.Xml;
using System.Runtime.Serialization;
using Opc.Ua;

namespace LibraryData
{
    #region BookState Class
    #if (!OPCUA_EXCLUDE_BookState)
    /// <summary>
    /// Stores an instance of the Book ObjectType.
    /// </summary>
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    public partial class BookState : BaseObjectState
    {
        #region Constructors
        /// <summary>
        /// Initializes the type with its default attribute values.
        /// </summary>
        public BookState(NodeState parent) : base(parent)
        {
        }

        /// <summary>
        /// Returns the id of the default type definition node for the instance.
        /// </summary>
        protected override NodeId GetDefaultTypeDefinitionId(NamespaceTable namespaceUris)
        {
            return Opc.Ua.NodeId.Create(LibraryData.ObjectTypes.Book, LibraryData.Namespaces.LibraryData, namespaceUris);
        }

        #if (!OPCUA_EXCLUDE_InitializationStrings)
        /// <summary>
        /// Initializes the instance.
        /// </summary>
        protected override void Initialize(ISystemContext context)
        {
            base.Initialize(context);
            Initialize(context, InitializationString);
            InitializeOptionalChildren(context);
        }

        /// <summary>
        /// Initializes the instance with a node.
        /// </summary>
        protected override void Initialize(ISystemContext context, NodeState source)
        {
            InitializeOptionalChildren(context);
            base.Initialize(context, source);
        }

        /// <summary>
        /// Initializes the any option children defined for the instance.
        /// </summary>
        protected override void InitializeOptionalChildren(ISystemContext context)
        {
            base.InitializeOptionalChildren(context);
        }

        #region Initialization String
        private const string InitializationString =
           "AgAAABwAAABodHRwOi8vY2FzLmV1L1VBL0NvbW1TZXJ2ZXIvJwAAAGh0dHA6Ly9saWJyYXJ5UHJvamVj" +
           "dC5jb20vVFBVTS1Qcm9qZWN0L/////8EYIACAQAAAAIADAAAAEJvb2tJbnN0YW5jZQECAQABAgEAAQAA" +
           "AP////8DAAAAFWCJCgIAAAACAAIAAABJZAECAgAALgBEAgAAAAAO/////wEB/////wAAAAAVYIkKAgAA" +
           "AAIABQAAAFRpdGxlAQIDAAAuAEQDAAAAAAz/////AQH/////AAAAABVgiQoCAAAAAgAGAAAAQXV0aG9y" +
           "AQIEAAAuAEQEAAAAAAz/////AQH/////AAAAAA==";
        #endregion
        #endif
        #endregion

        #region Public Properties
        /// <remarks />
        public PropertyState<Guid> Id
        {
            get
            {
                return m_id;
            }

            set
            {
                if (!Object.ReferenceEquals(m_id, value))
                {
                    ChangeMasks |= NodeStateChangeMasks.Children;
                }

                m_id = value;
            }
        }

        /// <remarks />
        public PropertyState<string> Title
        {
            get
            {
                return m_title;
            }

            set
            {
                if (!Object.ReferenceEquals(m_title, value))
                {
                    ChangeMasks |= NodeStateChangeMasks.Children;
                }

                m_title = value;
            }
        }

        /// <remarks />
        public PropertyState<string> Author
        {
            get
            {
                return m_author;
            }

            set
            {
                if (!Object.ReferenceEquals(m_author, value))
                {
                    ChangeMasks |= NodeStateChangeMasks.Children;
                }

                m_author = value;
            }
        }
        #endregion

        #region Overridden Methods
        /// <summary>
        /// Populates a list with the children that belong to the node.
        /// </summary>
        /// <param name="context">The context for the system being accessed.</param>
        /// <param name="children">The list of children to populate.</param>
        public override void GetChildren(
            ISystemContext context,
            IList<BaseInstanceState> children)
        {
            if (m_id != null)
            {
                children.Add(m_id);
            }

            if (m_title != null)
            {
                children.Add(m_title);
            }

            if (m_author != null)
            {
                children.Add(m_author);
            }

            base.GetChildren(context, children);
        }

        /// <summary>
        /// Finds the child with the specified browse name.
        /// </summary>
        protected override BaseInstanceState FindChild(
            ISystemContext context,
            QualifiedName browseName,
            bool createOrReplace,
            BaseInstanceState replacement)
        {
            if (QualifiedName.IsNull(browseName))
            {
                return null;
            }

            BaseInstanceState instance = null;

            switch (browseName.Name)
            {
                case LibraryData.BrowseNames.Id:
                {
                    if (createOrReplace)
                    {
                        if (Id == null)
                        {
                            if (replacement == null)
                            {
                                Id = new PropertyState<Guid>(this);
                            }
                            else
                            {
                                Id = (PropertyState<Guid>)replacement;
                            }
                        }
                    }

                    instance = Id;
                    break;
                }

                case LibraryData.BrowseNames.Title:
                {
                    if (createOrReplace)
                    {
                        if (Title == null)
                        {
                            if (replacement == null)
                            {
                                Title = new PropertyState<string>(this);
                            }
                            else
                            {
                                Title = (PropertyState<string>)replacement;
                            }
                        }
                    }

                    instance = Title;
                    break;
                }

                case LibraryData.BrowseNames.Author:
                {
                    if (createOrReplace)
                    {
                        if (Author == null)
                        {
                            if (replacement == null)
                            {
                                Author = new PropertyState<string>(this);
                            }
                            else
                            {
                                Author = (PropertyState<string>)replacement;
                            }
                        }
                    }

                    instance = Author;
                    break;
                }
            }

            if (instance != null)
            {
                return instance;
            }

            return base.FindChild(context, browseName, createOrReplace, replacement);
        }
        #endregion

        #region Private Fields
        private PropertyState<Guid> m_id;
        private PropertyState<string> m_title;
        private PropertyState<string> m_author;
        #endregion
    }
    #endif
    #endregion

    #region ReaderState Class
    #if (!OPCUA_EXCLUDE_ReaderState)
    /// <summary>
    /// Stores an instance of the Reader ObjectType.
    /// </summary>
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    public partial class ReaderState : BaseObjectState
    {
        #region Constructors
        /// <summary>
        /// Initializes the type with its default attribute values.
        /// </summary>
        public ReaderState(NodeState parent) : base(parent)
        {
        }

        /// <summary>
        /// Returns the id of the default type definition node for the instance.
        /// </summary>
        protected override NodeId GetDefaultTypeDefinitionId(NamespaceTable namespaceUris)
        {
            return Opc.Ua.NodeId.Create(LibraryData.ObjectTypes.Reader, LibraryData.Namespaces.LibraryData, namespaceUris);
        }

        #if (!OPCUA_EXCLUDE_InitializationStrings)
        /// <summary>
        /// Initializes the instance.
        /// </summary>
        protected override void Initialize(ISystemContext context)
        {
            base.Initialize(context);
            Initialize(context, InitializationString);
            InitializeOptionalChildren(context);
        }

        /// <summary>
        /// Initializes the instance with a node.
        /// </summary>
        protected override void Initialize(ISystemContext context, NodeState source)
        {
            InitializeOptionalChildren(context);
            base.Initialize(context, source);
        }

        /// <summary>
        /// Initializes the any option children defined for the instance.
        /// </summary>
        protected override void InitializeOptionalChildren(ISystemContext context)
        {
            base.InitializeOptionalChildren(context);
        }

        #region Initialization String
        private const string InitializationString =
           "AgAAABwAAABodHRwOi8vY2FzLmV1L1VBL0NvbW1TZXJ2ZXIvJwAAAGh0dHA6Ly9saWJyYXJ5UHJvamVj" +
           "dC5jb20vVFBVTS1Qcm9qZWN0L/////8EYIACAQAAAAIADgAAAFJlYWRlckluc3RhbmNlAQIFAAECBQAF" +
           "AAAA/////wIAAAAVYIkKAgAAAAIAAgAAAElkAQIGAAAuAEQGAAAAAA7/////AQH/////AAAAABVgiQoC" +
           "AAAAAgAEAAAATmFtZQECBwAALgBEBwAAAAAM/////wEB/////wAAAAA=";
        #endregion
        #endif
        #endregion

        #region Public Properties
        /// <remarks />
        public PropertyState<Guid> Id
        {
            get
            {
                return m_id;
            }

            set
            {
                if (!Object.ReferenceEquals(m_id, value))
                {
                    ChangeMasks |= NodeStateChangeMasks.Children;
                }

                m_id = value;
            }
        }

        /// <remarks />
        public PropertyState<string> Name
        {
            get
            {
                return m_name;
            }

            set
            {
                if (!Object.ReferenceEquals(m_name, value))
                {
                    ChangeMasks |= NodeStateChangeMasks.Children;
                }

                m_name = value;
            }
        }
        #endregion

        #region Overridden Methods
        /// <summary>
        /// Populates a list with the children that belong to the node.
        /// </summary>
        /// <param name="context">The context for the system being accessed.</param>
        /// <param name="children">The list of children to populate.</param>
        public override void GetChildren(
            ISystemContext context,
            IList<BaseInstanceState> children)
        {
            if (m_id != null)
            {
                children.Add(m_id);
            }

            if (m_name != null)
            {
                children.Add(m_name);
            }

            base.GetChildren(context, children);
        }

        /// <summary>
        /// Finds the child with the specified browse name.
        /// </summary>
        protected override BaseInstanceState FindChild(
            ISystemContext context,
            QualifiedName browseName,
            bool createOrReplace,
            BaseInstanceState replacement)
        {
            if (QualifiedName.IsNull(browseName))
            {
                return null;
            }

            BaseInstanceState instance = null;

            switch (browseName.Name)
            {
                case LibraryData.BrowseNames.Id:
                {
                    if (createOrReplace)
                    {
                        if (Id == null)
                        {
                            if (replacement == null)
                            {
                                Id = new PropertyState<Guid>(this);
                            }
                            else
                            {
                                Id = (PropertyState<Guid>)replacement;
                            }
                        }
                    }

                    instance = Id;
                    break;
                }

                case LibraryData.BrowseNames.Name:
                {
                    if (createOrReplace)
                    {
                        if (Name == null)
                        {
                            if (replacement == null)
                            {
                                Name = new PropertyState<string>(this);
                            }
                            else
                            {
                                Name = (PropertyState<string>)replacement;
                            }
                        }
                    }

                    instance = Name;
                    break;
                }
            }

            if (instance != null)
            {
                return instance;
            }

            return base.FindChild(context, browseName, createOrReplace, replacement);
        }
        #endregion

        #region Private Fields
        private PropertyState<Guid> m_id;
        private PropertyState<string> m_name;
        #endregion
    }
    #endif
    #endregion

    #region LoanState Class
    #if (!OPCUA_EXCLUDE_LoanState)
    /// <summary>
    /// Stores an instance of the Loan ObjectType.
    /// </summary>
    /// <exclude />
    [System.CodeDom.Compiler.GeneratedCodeAttribute("Opc.Ua.ModelCompiler", "1.0.0.0")]
    public partial class LoanState : BaseObjectState
    {
        #region Constructors
        /// <summary>
        /// Initializes the type with its default attribute values.
        /// </summary>
        public LoanState(NodeState parent) : base(parent)
        {
        }

        /// <summary>
        /// Returns the id of the default type definition node for the instance.
        /// </summary>
        protected override NodeId GetDefaultTypeDefinitionId(NamespaceTable namespaceUris)
        {
            return Opc.Ua.NodeId.Create(LibraryData.ObjectTypes.Loan, LibraryData.Namespaces.LibraryData, namespaceUris);
        }

        #if (!OPCUA_EXCLUDE_InitializationStrings)
        /// <summary>
        /// Initializes the instance.
        /// </summary>
        protected override void Initialize(ISystemContext context)
        {
            base.Initialize(context);
            Initialize(context, InitializationString);
            InitializeOptionalChildren(context);
        }

        /// <summary>
        /// Initializes the instance with a node.
        /// </summary>
        protected override void Initialize(ISystemContext context, NodeState source)
        {
            InitializeOptionalChildren(context);
            base.Initialize(context, source);
        }

        /// <summary>
        /// Initializes the any option children defined for the instance.
        /// </summary>
        protected override void InitializeOptionalChildren(ISystemContext context)
        {
            base.InitializeOptionalChildren(context);
        }

        #region Initialization String
        private const string InitializationString =
           "AgAAABwAAABodHRwOi8vY2FzLmV1L1VBL0NvbW1TZXJ2ZXIvJwAAAGh0dHA6Ly9saWJyYXJ5UHJvamVj" +
           "dC5jb20vVFBVTS1Qcm9qZWN0L/////8EYIACAQAAAAIADAAAAExvYW5JbnN0YW5jZQECCAABAggACAAA" +
           "AP////8FAAAAFWCJCgIAAAACAAIAAABJZAECCQAALgBECQAAAAAO/////wEB/////wAAAAAVYIkKAgAA" +
           "AAIABgAAAEJvb2tJZAECCgAALgBECgAAAAAO/////wEB/////wAAAAAVYIkKAgAAAAIACAAAAFJlYWRl" +
           "cklkAQILAAAuAEQLAAAAAA7/////AQH/////AAAAABVgiQoCAAAAAgAKAAAAQm9ycm93RGF0ZQECDAAA" +
           "LgBEDAAAAAAN/////wEB/////wAAAAAVYIkKAgAAAAIACgAAAFJldHVybkRhdGUBAg0AAC4ARA0AAAAA" +
           "Df////8BAf////8AAAAA";
        #endregion
        #endif
        #endregion

        #region Public Properties
        /// <remarks />
        public PropertyState<Guid> Id
        {
            get
            {
                return m_id;
            }

            set
            {
                if (!Object.ReferenceEquals(m_id, value))
                {
                    ChangeMasks |= NodeStateChangeMasks.Children;
                }

                m_id = value;
            }
        }

        /// <remarks />
        public PropertyState<Guid> BookId
        {
            get
            {
                return m_bookId;
            }

            set
            {
                if (!Object.ReferenceEquals(m_bookId, value))
                {
                    ChangeMasks |= NodeStateChangeMasks.Children;
                }

                m_bookId = value;
            }
        }

        /// <remarks />
        public PropertyState<Guid> ReaderId
        {
            get
            {
                return m_readerId;
            }

            set
            {
                if (!Object.ReferenceEquals(m_readerId, value))
                {
                    ChangeMasks |= NodeStateChangeMasks.Children;
                }

                m_readerId = value;
            }
        }

        /// <remarks />
        public PropertyState<DateTime> BorrowDate
        {
            get
            {
                return m_borrowDate;
            }

            set
            {
                if (!Object.ReferenceEquals(m_borrowDate, value))
                {
                    ChangeMasks |= NodeStateChangeMasks.Children;
                }

                m_borrowDate = value;
            }
        }

        /// <remarks />
        public PropertyState<DateTime> ReturnDate
        {
            get
            {
                return m_returnDate;
            }

            set
            {
                if (!Object.ReferenceEquals(m_returnDate, value))
                {
                    ChangeMasks |= NodeStateChangeMasks.Children;
                }

                m_returnDate = value;
            }
        }
        #endregion

        #region Overridden Methods
        /// <summary>
        /// Populates a list with the children that belong to the node.
        /// </summary>
        /// <param name="context">The context for the system being accessed.</param>
        /// <param name="children">The list of children to populate.</param>
        public override void GetChildren(
            ISystemContext context,
            IList<BaseInstanceState> children)
        {
            if (m_id != null)
            {
                children.Add(m_id);
            }

            if (m_bookId != null)
            {
                children.Add(m_bookId);
            }

            if (m_readerId != null)
            {
                children.Add(m_readerId);
            }

            if (m_borrowDate != null)
            {
                children.Add(m_borrowDate);
            }

            if (m_returnDate != null)
            {
                children.Add(m_returnDate);
            }

            base.GetChildren(context, children);
        }

        /// <summary>
        /// Finds the child with the specified browse name.
        /// </summary>
        protected override BaseInstanceState FindChild(
            ISystemContext context,
            QualifiedName browseName,
            bool createOrReplace,
            BaseInstanceState replacement)
        {
            if (QualifiedName.IsNull(browseName))
            {
                return null;
            }

            BaseInstanceState instance = null;

            switch (browseName.Name)
            {
                case LibraryData.BrowseNames.Id:
                {
                    if (createOrReplace)
                    {
                        if (Id == null)
                        {
                            if (replacement == null)
                            {
                                Id = new PropertyState<Guid>(this);
                            }
                            else
                            {
                                Id = (PropertyState<Guid>)replacement;
                            }
                        }
                    }

                    instance = Id;
                    break;
                }

                case LibraryData.BrowseNames.BookId:
                {
                    if (createOrReplace)
                    {
                        if (BookId == null)
                        {
                            if (replacement == null)
                            {
                                BookId = new PropertyState<Guid>(this);
                            }
                            else
                            {
                                BookId = (PropertyState<Guid>)replacement;
                            }
                        }
                    }

                    instance = BookId;
                    break;
                }

                case LibraryData.BrowseNames.ReaderId:
                {
                    if (createOrReplace)
                    {
                        if (ReaderId == null)
                        {
                            if (replacement == null)
                            {
                                ReaderId = new PropertyState<Guid>(this);
                            }
                            else
                            {
                                ReaderId = (PropertyState<Guid>)replacement;
                            }
                        }
                    }

                    instance = ReaderId;
                    break;
                }

                case LibraryData.BrowseNames.BorrowDate:
                {
                    if (createOrReplace)
                    {
                        if (BorrowDate == null)
                        {
                            if (replacement == null)
                            {
                                BorrowDate = new PropertyState<DateTime>(this);
                            }
                            else
                            {
                                BorrowDate = (PropertyState<DateTime>)replacement;
                            }
                        }
                    }

                    instance = BorrowDate;
                    break;
                }

                case LibraryData.BrowseNames.ReturnDate:
                {
                    if (createOrReplace)
                    {
                        if (ReturnDate == null)
                        {
                            if (replacement == null)
                            {
                                ReturnDate = new PropertyState<DateTime>(this);
                            }
                            else
                            {
                                ReturnDate = (PropertyState<DateTime>)replacement;
                            }
                        }
                    }

                    instance = ReturnDate;
                    break;
                }
            }

            if (instance != null)
            {
                return instance;
            }

            return base.FindChild(context, browseName, createOrReplace, replacement);
        }
        #endregion

        #region Private Fields
        private PropertyState<Guid> m_id;
        private PropertyState<Guid> m_bookId;
        private PropertyState<Guid> m_readerId;
        private PropertyState<DateTime> m_borrowDate;
        private PropertyState<DateTime> m_returnDate;
        #endregion
    }
    #endif
    #endregion
}