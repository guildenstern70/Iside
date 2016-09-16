using System;
using System.Collections;

namespace LLCryptoLib.Security.Certificates
{
    /// <summary>
    ///     Implements a collection of <see cref="DistinguishedName" /> instances.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1710:IdentifiersShouldHaveCorrectSuffix")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design",
         "CA1010:CollectionsShouldImplementGenericInterface")]
    public class DistinguishedNameList : IEnumerable, ICloneable
    {
        /// <summary>
        ///     Holds the internal list.
        /// </summary>
        private readonly ArrayList m_List;

        /// <summary>
        ///     Initializes a new <see cref="DistinguishedNameList" /> instance.
        /// </summary>
        public DistinguishedNameList()
        {
            this.m_List = new ArrayList();
        }

        /// <summary>
        ///     Initializes a new <see cref="DistinguishedNameList" /> instance.
        /// </summary>
        /// <param name="state">The initial state of the collection.</param>
        /// <exception cref="ArgumentNullException"><paramref name="state" /> is a null reference (<b>Nothing</b> in Visual Basic).</exception>
        internal DistinguishedNameList(ArrayList state)
        {
            if (state == null)
                throw new ArgumentNullException();
            this.m_List = (ArrayList) state.Clone();
        }

        /// <summary>
        ///     Gets a value indicating whether the <see cref="DistinguishedNameList" /> has a fixed size.
        /// </summary>
        /// <value><b>true</b> if the ArrayList has a fixed size; otherwise, <b>false</b>.</value>
        public bool IsFixedSize
        {
            get { return this.m_List.IsFixedSize; }
        }

        /// <summary>
        ///     Gets a value indicating whether the <see cref="DistinguishedNameList" /> is read-only.
        /// </summary>
        /// <value><b>true</b> if the ArrayList is read-only; otherwise, <b>false</b>.</value>
        public bool IsReadOnly
        {
            get { return this.m_List.IsReadOnly; }
        }

        /// <summary>
        ///     Gets or sets the element at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the element to get or set.</param>
        /// <value>The element at the specified index.</value>
        /// <exception cref="ArgumentNullException"><paramref name="value" /> is a null reference (<b>Nothing</b> in Visual Basic).</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="index" /> is less than zero -or-
        ///     <paramref name="index" /> is equal to or greater than <see cref="Count" />.
        /// </exception>
        public DistinguishedName this[int index]
        {
            get { return (DistinguishedName) this.m_List[index]; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException();
                this.m_List[index] = value;
            }
        }

        /// <summary>
        ///     Gets the number of elements actually contained in the <see cref="DistinguishedNameList" />.
        /// </summary>
        /// <value>The number of elements actually contained in the DistinguishedNameList.</value>
        public int Count
        {
            get { return this.m_List.Count; }
        }

        /// <summary>
        ///     Gets a value indicating whether access to the <see cref="DistinguishedNameList" /> is synchronized (thread-safe).
        /// </summary>
        /// <value><b>true</b> if access to the DistinguishedNameList is synchronized (thread-safe); otherwise, <b>false</b>.</value>
        public bool IsSynchronized
        {
            get { return this.m_List.IsSynchronized; }
        }

        /// <summary>
        ///     Gets an object that can be used to synchronize access to the <see cref="DistinguishedNameList" />.
        /// </summary>
        /// <value>An object that can be used to synchronize access to the DistinguishedNameList.</value>
        public object SyncRoot
        {
            get { return this.m_List.SyncRoot; }
        }

        /// <summary>
        ///     Creates a shallow copy of the <see cref="DistinguishedNameList" />.
        /// </summary>
        /// <returns>A shallow copy of the DistinguishedNameList.</returns>
        public object Clone()
        {
            return new DistinguishedNameList(this.m_List);
        }

        /// <summary>
        ///     Returns an enumerator for the entire <see cref="DistinguishedNameList" />.
        /// </summary>
        /// <returns>An IEnumerator for the entire ArrayList.</returns>
        public IEnumerator GetEnumerator()
        {
            return this.m_List.GetEnumerator();
        }

        /// <summary>
        ///     Adds a <see cref="DistinguishedName" /> to the end of the <see cref="DistinguishedNameList" />.
        /// </summary>
        /// <param name="value">The <see cref="DistinguishedName" /> to be added to the end of the DistinguishedNameList.</param>
        /// <returns>The list index at which the value has been added.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value" /> is a null reference (<b>Nothing</b> in Visual Basic).</exception>
        /// <exception cref="NotSupportedException">The list is read-only -or- the list has a fixed size.</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage",
             "CA2208:InstantiateArgumentExceptionsCorrectly")]
        public int Add(DistinguishedName value)
        {
            if (value == null)
                throw new ArgumentNullException();
            return this.m_List.Add(value);
        }

        /// <summary>
        ///     Removes all elements from the <see cref="DistinguishedNameList" />.
        /// </summary>
        /// <exception cref="NotSupportedException">The list is read-only -or- the list has a fixed size.</exception>
        public void Clear()
        {
            this.m_List.Clear();
        }

        /// <summary>
        ///     Determines whether an element is in the <see cref="DistinguishedNameList" />.
        /// </summary>
        /// <param name="value">
        ///     The Object to locate in the DistinguishedNameList. The element to locate cannot be a null reference
        ///     (<b>Nothing</b> in Visual Basic).
        /// </param>
        /// <returns><b>true</b> if item is found in the DistinguishedNameList; otherwise, <b>false</b>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="value" /> is a null reference (<b>Nothing</b> in Visual Basic).</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage",
             "CA2208:InstantiateArgumentExceptionsCorrectly")]
        public bool Contains(DistinguishedName value)
        {
            if (value == null)
                throw new ArgumentNullException();
            return this.m_List.Contains(value);
        }

        /// <summary>
        ///     Searches for the specified <see cref="DistinguishedName" /> and returns the zero-based index of the first
        ///     occurrence within the entire <see cref="DistinguishedNameList" />.
        /// </summary>
        /// <param name="value">The DistinguishedName to locate in the DistinguishedNameList.</param>
        /// <returns>
        ///     The zero-based index of the first occurrence of value within the entire DistinguishedNameList, if found;
        ///     otherwise, -1.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="value" /> is a null reference (<b>Nothing</b> in Visual Basic).</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage",
             "CA2208:InstantiateArgumentExceptionsCorrectly")]
        public int IndexOf(DistinguishedName value)
        {
            if (value == null)
                throw new ArgumentNullException();
            return this.m_List.IndexOf(value);
        }

        /// <summary>
        ///     Inserts an element into the <see cref="DistinguishedNameList" /> at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which <paramref name="value" /> should be inserted.</param>
        /// <param name="value">The <see cref="DistinguishedName" /> to insert. </param>
        /// <exception cref="ArgumentNullException"><paramref name="value" /> is a null reference (<b>Nothing</b> in Visual Basic).</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="index" /> is less than zero -or-
        ///     <paramref name="index" /> is greater than <see cref="Count" />.
        /// </exception>
        /// <exception cref="NotSupportedException">
        ///     The DistinguishedNameList is read-only -or- the DistinguishedNameList has a
        ///     fixed size.
        /// </exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage",
             "CA2208:InstantiateArgumentExceptionsCorrectly")]
        public void Insert(int index, DistinguishedName value)
        {
            if (value == null)
                throw new ArgumentNullException();
            this.m_List.Insert(index, value);
        }

        /// <summary>
        ///     Removes the first occurrence of a specific object from the <see cref="DistinguishedNameList" />.
        /// </summary>
        /// <param name="value">The <see cref="DistinguishedName" /> to remove from the DistinguishedNameList.</param>
        /// <exception cref="ArgumentNullException"><paramref name="value" /> is a null reference (<b>Nothing</b> in Visual Basic).</exception>
        /// <exception cref="NotSupportedException">
        ///     The DistinguishedNameList is read-only -or- the DistinguishedNameList has a
        ///     fixed size.
        /// </exception>
        public void Remove(DistinguishedName value)
        {
            this.m_List.Remove(value);
        }

        /// <summary>
        ///     Removes the element at the specified index of the <see cref="DistinguishedNameList" />.
        /// </summary>
        /// <param name="index">The zero-based index of the element to remove.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="index" /> is less than zero -or-
        ///     <paramref name="index" /> is greater than <see cref="Count" />.
        /// </exception>
        /// <exception cref="NotSupportedException">
        ///     The DistinguishedNameList is read-only -or- the DistinguishedNameList has a
        ///     fixed size.
        /// </exception>
        public void RemoveAt(int index)
        {
            this.m_List.RemoveAt(index);
        }

        /// <summary>
        ///     Copies the entire <see cref="DistinguishedNameList" /> to a compatible one-dimensional <see cref="Array" />,
        ///     starting at the specified index of the target array.
        /// </summary>
        /// <param name="array">
        ///     The one-dimensional Array that is the destination of the elements copied from
        ///     DistinguishedNameList. The Array must have zero-based indexing.
        /// </param>
        /// <param name="index">The zero-based index in <paramref name="array" /> at which copying begins.</param>
        /// <exception cref="ArgumentNullException"><paramref name="array" /> is a null reference (<b>Nothing</b> in Visual Basic).</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="array" /> is less than zero.</exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="array" /> is multidimensional -or- <paramref name="array" /> is
        ///     equal to or greater than the length of <paramref name="array" /> -or- the number of elements in the source
        ///     DistinguishedNameList is greater than the available space to the end of the destination array.
        /// </exception>
        /// <exception cref="InvalidCastException">
        ///     The type of the source DistinguishedNameList cannot be cast automatically to the
        ///     type of the destination array.
        /// </exception>
        public void CopyTo(Array array, int index)
        {
            this.m_List.CopyTo(array, index);
        }
    }
}