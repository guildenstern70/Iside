using System;
using System.Collections;

namespace LLCryptoLib.Security.Certificates
{
    /// <summary>
    ///     Defines a collection of certificate stores.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming",
         "CA1711:IdentifiersShouldNotHaveIncorrectSuffix")]
    public class CertificateStoreCollection : CertificateStore
    {
        /// <summary>
        ///     Holds the references to the CertificateStore instances in the collection. This is to avoid CertificateStores
        ///     finalizing and destroying their handles.
        /// </summary>
        private readonly ArrayList m_Stores;

        /// <summary>
        ///     Initializes a new instance of the <see cref="CertificateStoreCollection" /> class.
        /// </summary>
        /// <param name="stores">An array of stores that should be added to the collection.</param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="stores" /> is a null reference (<b>Nothing</b> in Visual
        ///     Basic).
        /// </exception>
        /// <exception cref="ArgumentException">
        ///     One of the <see cref="CertificateStore" /> objects in the array is a
        ///     <see cref="CertificateStoreCollection" /> instance. This is not allowed to avoid circular dependencies.
        /// </exception>
        /// <exception cref="CertificateException">An error occurs while adding a certificate to the collection.</exception>
        public CertificateStoreCollection(CertificateStore[] stores)
            : base(
                SspiProvider.CertOpenStore(new IntPtr(SecurityConstants.CERT_STORE_PROV_COLLECTION), 0, 0, 0, null),
                false)
        {
            if (stores == null)
                throw new ArgumentNullException();
            for (int i = 0; i < stores.Length; i++)
            {
                if (stores[i].ToString() == this.ToString())
                {
                    // used in order to avoid circular dependencies
                    throw new ArgumentException(
                        "A certificate store collection cannot hold other certificate store collections.");
                }
            }
            for (int i = 0; i < stores.Length; i++)
            {
                if (SspiProvider.CertAddStoreToCollection(this.Handle, stores[i].Handle, 0, 0) == 0)
                    throw new CertificateException("Could not add the store to the collection.");
            }
            this.m_Stores = new ArrayList();
                // used to hold references to the certificate stores so they cannot be finalized
            this.m_Stores.AddRange(stores);
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="CertificateStoreCollection" /> class.
        /// </summary>
        /// <param name="collection">
        ///     The CertificateStoreCollection whose elements are copied to the new certificate store
        ///     collection.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="collection" /> is a null reference (<b>Nothing</b> in Visual
        ///     Basic).
        /// </exception>
        /// <exception cref="CertificateException">An error occurs while adding a certificate to the collection.</exception>
        public CertificateStoreCollection(CertificateStoreCollection collection)
            : base(
                SspiProvider.CertOpenStore(new IntPtr(SecurityConstants.CERT_STORE_PROV_COLLECTION), 0, 0, 0, null),
                false)
        {
            if (collection == null)
                throw new ArgumentNullException();
            this.m_Stores = new ArrayList(collection.m_Stores);
                // used to hold references to the certificate stores so they cannot be finalized
            for (int i = 0; i < this.m_Stores.Count; i++)
            {
                if (
                    SspiProvider.CertAddStoreToCollection(this.Handle, ((CertificateStore) this.m_Stores[i]).Handle, 0,
                        0) == 0)
                    throw new CertificateException("Could not add the store to the collection.");
            }
        }

        /// <summary>
        ///     Adds a certificate store to the collection.
        /// </summary>
        /// <param name="store">An instance of the <see cref="CertificateStore" /> class.</param>
        /// <exception cref="ArgumentNullException"><paramref name="store" /> is a null reference (<b>Nothing</b> in Visual Basic).</exception>
        /// <exception cref="ArgumentException">
        ///     The specified certificate store is a <see cref="CertificateStoreCollection" />
        ///     instance. This is not allowed to avoid circular dependencies.
        /// </exception>
        /// <exception cref="CertificateException">An error occurs while adding the certificate to the collection.</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage",
             "CA2208:InstantiateArgumentExceptionsCorrectly")]
        public void AddStore(CertificateStore store)
        {
            if (store == null)
                throw new ArgumentNullException();
            if (store.ToString() == this.ToString()) // avoid circular dependencies
                throw new ArgumentException(
                    "A certificate store collection cannot hold other certificate store collections.");
            if (SspiProvider.CertAddStoreToCollection(this.Handle, store.Handle, 0, 0) == 0)
                throw new CertificateException("Could not add the store to the collection.");
            this.m_Stores.Add(store);
        }

        /// <summary>
        ///     Removes a certificate store from the collection.
        /// </summary>
        /// <param name="store">An instance of the <see cref="CertificateStore" /> class.</param>
        /// <exception cref="ArgumentNullException"><paramref name="store" /> is a null reference (<b>Nothing</b> in Visual Basic).</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage",
             "CA2208:InstantiateArgumentExceptionsCorrectly")]
        public void RemoveStore(CertificateStore store)
        {
            if (store == null)
                throw new ArgumentNullException();
            SspiProvider.CertRemoveStoreFromCollection(this.Handle, store.Handle);
            this.m_Stores.Remove(store);
        }
    }
}