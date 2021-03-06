
#if !MONO

[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters",
        Scope = "member",
        Target =
            "LLCryptoLib.Crypto.AsymmetricCrypter.Decrypt(System.IO.FileInfo,System.IO.FileInfo,System.Security.Cryptography.RSACryptoServiceProvider):System.Void"
    )]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters",
        Scope = "member",
        Target =
            "LLCryptoLib.Crypto.AsymmetricCrypter.Encrypt(System.IO.FileInfo,System.IO.FileInfo,System.Security.Cryptography.RSACryptoServiceProvider):System.Void"
    )]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1805:DoNotInitializeUnnecessarily",
        Scope = "member",
        Target =
            "LLCryptoLib.Crypto.BlowfishManagedTransform..ctor(LLCryptoLib.Crypto.TransformMode,System.Security.Cryptography.CipherMode,System.Security.Cryptography.PaddingMode,System.Byte[],System.Byte[])"
    )]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage",
        "CA2214:DoNotCallOverridableMethodsInConstructors", Scope = "member",
        Target = "LLCryptoLib.Hash.Adler32..ctor()")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage",
        "CA2214:DoNotCallOverridableMethodsInConstructors", Scope = "member", Target = "LLCryptoLib.Hash.Cksum..ctor()")
]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
        "CA1810:InitializeReferenceTypeStaticFieldsInline", Scope = "member", Target = "LLCryptoLib.Hash.CRC..cctor()")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage",
        "CA2214:DoNotCallOverridableMethodsInConstructors", Scope = "member",
        Target = "LLCryptoLib.Hash.CRC..ctor(LLCryptoLib.Hash.CRCParameters)")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
        "CA1810:InitializeReferenceTypeStaticFieldsInline", Scope = "member", Target = "LLCryptoLib.Hash.CRC32..cctor()"
    )]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage",
        "CA2214:DoNotCallOverridableMethodsInConstructors", Scope = "member",
        Target = "LLCryptoLib.Hash.CRC32..ctor(System.UInt32,System.Boolean)")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage",
        "CA2214:DoNotCallOverridableMethodsInConstructors", Scope = "member",
        Target = "LLCryptoLib.Hash.ElfHash..ctor()")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage",
        "CA2214:DoNotCallOverridableMethodsInConstructors", Scope = "member", Target = "LLCryptoLib.Hash.FCS16..ctor()")
]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage",
        "CA2214:DoNotCallOverridableMethodsInConstructors", Scope = "member", Target = "LLCryptoLib.Hash.FCS32..ctor()")
]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage",
        "CA2214:DoNotCallOverridableMethodsInConstructors", Scope = "member",
        Target = "LLCryptoLib.Hash.FNV..ctor(LLCryptoLib.Hash.FNVParameters)")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage",
        "CA2214:DoNotCallOverridableMethodsInConstructors", Scope = "member",
        Target = "LLCryptoLib.Hash.GHash..ctor(LLCryptoLib.Hash.GHashParameters)")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
        "CA1810:InitializeReferenceTypeStaticFieldsInline", Scope = "member",
        Target = "LLCryptoLib.Hash.GOSTHash..cctor()")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage",
        "CA2214:DoNotCallOverridableMethodsInConstructors", Scope = "member",
        Target = "LLCryptoLib.Hash.GOSTHash..ctor()")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage",
        "CA2214:DoNotCallOverridableMethodsInConstructors", Scope = "member",
        Target = "LLCryptoLib.Hash.HAVAL..ctor(LLCryptoLib.Hash.HAVALParameters)")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage",
        "CA2214:DoNotCallOverridableMethodsInConstructors", Scope = "member", Target = "LLCryptoLib.Hash.JHash..ctor()")
]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage",
        "CA2214:DoNotCallOverridableMethodsInConstructors", Scope = "member", Target = "LLCryptoLib.Hash.MD2..ctor()")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage",
        "CA2214:DoNotCallOverridableMethodsInConstructors", Scope = "member", Target = "LLCryptoLib.Hash.MD4..ctor()")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage",
        "CA2214:DoNotCallOverridableMethodsInConstructors", Scope = "member", Target = "LLCryptoLib.Hash.MD5..ctor()")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage",
        "CA2214:DoNotCallOverridableMethodsInConstructors", Scope = "member",
        Target = "LLCryptoLib.Hash.RIPEMD128..ctor()")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage",
        "CA2214:DoNotCallOverridableMethodsInConstructors", Scope = "member",
        Target = "LLCryptoLib.Hash.RIPEMD160..ctor()")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage",
        "CA2214:DoNotCallOverridableMethodsInConstructors", Scope = "member",
        Target = "LLCryptoLib.Hash.RIPEMD256..ctor()")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage",
        "CA2214:DoNotCallOverridableMethodsInConstructors", Scope = "member",
        Target = "LLCryptoLib.Hash.RIPEMD320..ctor()")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage",
        "CA2214:DoNotCallOverridableMethodsInConstructors", Scope = "member", Target = "LLCryptoLib.Hash.SHA0..ctor()")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage",
        "CA2214:DoNotCallOverridableMethodsInConstructors", Scope = "member", Target = "LLCryptoLib.Hash.SHA1..ctor()")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage",
        "CA2214:DoNotCallOverridableMethodsInConstructors", Scope = "member", Target = "LLCryptoLib.Hash.SHA224..ctor()"
    )]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage",
        "CA2214:DoNotCallOverridableMethodsInConstructors", Scope = "member", Target = "LLCryptoLib.Hash.SHA256..ctor()"
    )]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage",
        "CA2214:DoNotCallOverridableMethodsInConstructors", Scope = "member", Target = "LLCryptoLib.Hash.SHA384..ctor()"
    )]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage",
        "CA2214:DoNotCallOverridableMethodsInConstructors", Scope = "member", Target = "LLCryptoLib.Hash.SHA512..ctor()"
    )]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage",
        "CA2214:DoNotCallOverridableMethodsInConstructors", Scope = "member",
        Target = "LLCryptoLib.Hash.Snefru2..ctor(LLCryptoLib.Hash.Snefru2Parameters)")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods",
        Scope = "member", Target = "LLCryptoLib.Hash.Snefru2..ctor(LLCryptoLib.Hash.Snefru2Parameters)")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage",
        "CA2214:DoNotCallOverridableMethodsInConstructors", Scope = "member", Target = "LLCryptoLib.Hash.SumBSD..ctor()"
    )]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage",
        "CA2214:DoNotCallOverridableMethodsInConstructors", Scope = "member",
        Target = "LLCryptoLib.Hash.SumSysV..ctor()")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage",
        "CA2214:DoNotCallOverridableMethodsInConstructors", Scope = "member", Target = "LLCryptoLib.Hash.Tiger..ctor()")
]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2201:DoNotRaiseReservedExceptionTypes",
        Scope = "member",
        Target = "LLCryptoLib.Hash.Utilities.ByteToHexadecimal(System.Byte[],System.Int32,System.Int32):System.String")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2233:OperationsShouldNotOverflow",
        Scope = "member",
        Target = "LLCryptoLib.Hash.Utilities.ByteToHexadecimal(System.Byte[],System.Int32,System.Int32):System.String",
        MessageId = "2*length")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2201:DoNotRaiseReservedExceptionTypes",
        Scope = "member",
        Target =
            "LLCryptoLib.Hash.Utilities.ByteToUInt(System.Byte[],System.Int32,System.Int32,LLCryptoLib.Hash.EndianType):System.UInt32[]"
    )]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2201:DoNotRaiseReservedExceptionTypes",
        Scope = "member",
        Target =
            "LLCryptoLib.Hash.Utilities.ByteToULong(System.Byte[],System.Int32,System.Int32,LLCryptoLib.Hash.EndianType):System.UInt64[]"
    )]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2201:DoNotRaiseReservedExceptionTypes",
        Scope = "member",
        Target =
            "LLCryptoLib.Hash.Utilities.ByteToUShort(System.Byte[],System.Int32,System.Int32,LLCryptoLib.Hash.EndianType):System.UInt16[]"
    )]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2233:OperationsShouldNotOverflow",
        Scope = "member", Target = "LLCryptoLib.Hash.Utilities.RotateLeft(System.UInt16,System.Int32):System.UInt16",
        MessageId = "16-shift")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2233:OperationsShouldNotOverflow",
        Scope = "member", Target = "LLCryptoLib.Hash.Utilities.RotateLeft(System.UInt32,System.Int32):System.UInt32",
        MessageId = "32-shift")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2233:OperationsShouldNotOverflow",
        Scope = "member", Target = "LLCryptoLib.Hash.Utilities.RotateLeft(System.UInt64,System.Int32):System.UInt64",
        MessageId = "64-shift")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2233:OperationsShouldNotOverflow",
        Scope = "member", Target = "LLCryptoLib.Hash.Utilities.RotateRight(System.UInt16,System.Int32):System.UInt16",
        MessageId = "16-shift")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2233:OperationsShouldNotOverflow",
        Scope = "member", Target = "LLCryptoLib.Hash.Utilities.RotateRight(System.UInt32,System.Int32):System.UInt32",
        MessageId = "32-shift")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2233:OperationsShouldNotOverflow",
        Scope = "member", Target = "LLCryptoLib.Hash.Utilities.RotateRight(System.UInt64,System.Int32):System.UInt64",
        MessageId = "64-shift")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2201:DoNotRaiseReservedExceptionTypes",
        Scope = "member",
        Target =
            "LLCryptoLib.Hash.Utilities.UIntToByte(System.UInt32[],System.Int32,System.Int32,LLCryptoLib.Hash.EndianType):System.Byte[]"
    )]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2233:OperationsShouldNotOverflow",
        Scope = "member",
        Target =
            "LLCryptoLib.Hash.Utilities.UIntToByte(System.UInt32[],System.Int32,System.Int32,LLCryptoLib.Hash.EndianType):System.Byte[]",
        MessageId = "length*4")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2201:DoNotRaiseReservedExceptionTypes",
        Scope = "member",
        Target =
            "LLCryptoLib.Hash.Utilities.ULongToByte(System.UInt64[],System.Int32,System.Int32,LLCryptoLib.Hash.EndianType):System.Byte[]"
    )]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2233:OperationsShouldNotOverflow",
        Scope = "member",
        Target =
            "LLCryptoLib.Hash.Utilities.ULongToByte(System.UInt64[],System.Int32,System.Int32,LLCryptoLib.Hash.EndianType):System.Byte[]",
        MessageId = "length*8")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2233:OperationsShouldNotOverflow",
        Scope = "member",
        Target =
            "LLCryptoLib.Hash.Utilities.UShortToByte(System.UInt16[],System.Int32,System.Int32,LLCryptoLib.Hash.EndianType):System.Byte[]",
        MessageId = "length*2")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
        "CA1810:InitializeReferenceTypeStaticFieldsInline", Scope = "member",
        Target = "LLCryptoLib.Hash.Whirlpool..cctor()")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage",
        "CA2214:DoNotCallOverridableMethodsInConstructors", Scope = "member",
        Target = "LLCryptoLib.Hash.Whirlpool..ctor()")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage",
        "CA2214:DoNotCallOverridableMethodsInConstructors", Scope = "member", Target = "LLCryptoLib.Hash.XUM32..ctor()")
]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1060:MovePInvokesToNativeMethodsClass",
        Scope = "member",
        Target =
            "LLCryptoLib.Security.SspiProvider.CertGetCertificateContextProperty(System.IntPtr,System.Int32,System.Byte[],System.Int32&):System.Int32"
    )]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1060:MovePInvokesToNativeMethodsClass",
        Scope = "member",
        Target =
            "LLCryptoLib.Security.SspiProvider.CertGetCertificateContextProperty(System.IntPtr,System.Int32,System.IntPtr,System.Int32&):System.Int32"
    )]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode",
        Scope = "member",
        Target =
            "LLCryptoLib.Security.SspiProvider.CryptAcquireContext(System.Int32&,System.IntPtr,System.String,System.Int32,System.Int32):System.Int32"
    )]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1060:MovePInvokesToNativeMethodsClass",
        Scope = "member",
        Target =
            "LLCryptoLib.Security.SspiProvider.CryptAcquireContext(System.Int32&,System.IntPtr,System.String,System.Int32,System.Int32):System.Int32"
    )]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security",
        "CA2101:SpecifyMarshalingForPInvokeStringArguments", Scope = "member",
        Target =
            "LLCryptoLib.Security.SspiProvider.CryptAcquireContext(System.Int32&,System.IntPtr,System.String,System.Int32,System.Int32):System.Int32"
    )]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1060:MovePInvokesToNativeMethodsClass",
        Scope = "member",
        Target =
            "LLCryptoLib.Security.SspiProvider.CryptAcquireContext(System.Int32&,System.String,System.String,System.Int32,System.Int32):System.Int32"
    )]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security",
        "CA2101:SpecifyMarshalingForPInvokeStringArguments", Scope = "member",
        Target =
            "LLCryptoLib.Security.SspiProvider.CryptAcquireContext(System.Int32&,System.String,System.String,System.Int32,System.Int32):System.Int32"
    )]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1060:MovePInvokesToNativeMethodsClass",
        Scope = "member",
        Target =
            "LLCryptoLib.Security.SspiProvider.CryptDecodeObject(System.Int32,System.IntPtr,System.Byte[],System.Int32,System.Int32,System.IntPtr,System.Int32&):System.Int32"
    )]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1060:MovePInvokesToNativeMethodsClass",
        Scope = "member",
        Target =
            "LLCryptoLib.Security.SspiProvider.CryptDecodeObject(System.Int32,System.IntPtr,System.IntPtr,System.Int32,System.Int32,System.IntPtr,System.Int32&):System.Int32"
    )]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode",
        Scope = "member",
        Target =
            "LLCryptoLib.Security.SspiProvider.CryptDecrypt(System.Int32,System.Int32,System.Int32,System.Int32,System.Byte[],System.Int32&):System.Int32"
    )]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1060:MovePInvokesToNativeMethodsClass",
        Scope = "member",
        Target =
            "LLCryptoLib.Security.SspiProvider.CryptDecrypt(System.Int32,System.Int32,System.Int32,System.Int32,System.Byte[],System.Int32&):System.Int32"
    )]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Portability",
        "CA1901:PInvokeDeclarationsShouldBePortable", Scope = "member",
        Target =
            "LLCryptoLib.Security.SspiProvider.CryptDecrypt(System.Int32,System.Int32,System.Int32,System.Int32,System.Byte[],System.Int32&):System.Int32",
        MessageId = "0")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Portability",
        "CA1901:PInvokeDeclarationsShouldBePortable", Scope = "member",
        Target =
            "LLCryptoLib.Security.SspiProvider.CryptDecrypt(System.Int32,System.Int32,System.Int32,System.Int32,System.Byte[],System.Int32&):System.Int32",
        MessageId = "1")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode",
        Scope = "member",
        Target =
            "LLCryptoLib.Security.SspiProvider.CryptDecrypt(System.IntPtr,System.Int32,System.Int32,System.Int32,System.Byte[],System.Int32&):System.Int32"
    )]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1060:MovePInvokesToNativeMethodsClass",
        Scope = "member",
        Target =
            "LLCryptoLib.Security.SspiProvider.CryptDecrypt(System.IntPtr,System.Int32,System.Int32,System.Int32,System.Byte[],System.Int32&):System.Int32"
    )]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Portability",
        "CA1901:PInvokeDeclarationsShouldBePortable", Scope = "member",
        Target =
            "LLCryptoLib.Security.SspiProvider.CryptDecrypt(System.IntPtr,System.Int32,System.Int32,System.Int32,System.Byte[],System.Int32&):System.Int32",
        MessageId = "1")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode",
        Scope = "member",
        Target =
            "LLCryptoLib.Security.SspiProvider.CryptEncrypt(System.Int32,System.Int32,System.Int32,System.Int32,System.Byte[],System.Int32&,System.Int32):System.Int32"
    )]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1060:MovePInvokesToNativeMethodsClass",
        Scope = "member",
        Target =
            "LLCryptoLib.Security.SspiProvider.CryptEncrypt(System.Int32,System.Int32,System.Int32,System.Int32,System.Byte[],System.Int32&,System.Int32):System.Int32"
    )]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Portability",
        "CA1901:PInvokeDeclarationsShouldBePortable", Scope = "member",
        Target =
            "LLCryptoLib.Security.SspiProvider.CryptEncrypt(System.Int32,System.Int32,System.Int32,System.Int32,System.Byte[],System.Int32&,System.Int32):System.Int32",
        MessageId = "0")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Portability",
        "CA1901:PInvokeDeclarationsShouldBePortable", Scope = "member",
        Target =
            "LLCryptoLib.Security.SspiProvider.CryptEncrypt(System.Int32,System.Int32,System.Int32,System.Int32,System.Byte[],System.Int32&,System.Int32):System.Int32",
        MessageId = "1")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode",
        Scope = "member",
        Target =
            "LLCryptoLib.Security.SspiProvider.CryptEncrypt(System.Int32,System.Int32,System.Int32,System.Int32,System.IntPtr,System.Int32&,System.Int32):System.Int32"
    )]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1060:MovePInvokesToNativeMethodsClass",
        Scope = "member",
        Target =
            "LLCryptoLib.Security.SspiProvider.CryptEncrypt(System.Int32,System.Int32,System.Int32,System.Int32,System.IntPtr,System.Int32&,System.Int32):System.Int32"
    )]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Portability",
        "CA1901:PInvokeDeclarationsShouldBePortable", Scope = "member",
        Target =
            "LLCryptoLib.Security.SspiProvider.CryptEncrypt(System.Int32,System.Int32,System.Int32,System.Int32,System.IntPtr,System.Int32&,System.Int32):System.Int32",
        MessageId = "0")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Portability",
        "CA1901:PInvokeDeclarationsShouldBePortable", Scope = "member",
        Target =
            "LLCryptoLib.Security.SspiProvider.CryptEncrypt(System.Int32,System.Int32,System.Int32,System.Int32,System.IntPtr,System.Int32&,System.Int32):System.Int32",
        MessageId = "1")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode",
        Scope = "member",
        Target =
            "LLCryptoLib.Security.SspiProvider.CryptEncrypt(System.IntPtr,System.Int32,System.Int32,System.Int32,System.Byte[],System.Int32&,System.Int32):System.Int32"
    )]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1060:MovePInvokesToNativeMethodsClass",
        Scope = "member",
        Target =
            "LLCryptoLib.Security.SspiProvider.CryptEncrypt(System.IntPtr,System.Int32,System.Int32,System.Int32,System.Byte[],System.Int32&,System.Int32):System.Int32"
    )]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Portability",
        "CA1901:PInvokeDeclarationsShouldBePortable", Scope = "member",
        Target =
            "LLCryptoLib.Security.SspiProvider.CryptEncrypt(System.IntPtr,System.Int32,System.Int32,System.Int32,System.Byte[],System.Int32&,System.Int32):System.Int32",
        MessageId = "1")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1060:MovePInvokesToNativeMethodsClass",
        Scope = "member",
        Target =
            "LLCryptoLib.Security.SspiProvider.CryptExportKey(System.Int32,System.Int32,System.Int32,System.Int32,System.Byte[],System.Int32&):System.Int32"
    )]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Portability",
        "CA1901:PInvokeDeclarationsShouldBePortable", Scope = "member",
        Target =
            "LLCryptoLib.Security.SspiProvider.CryptExportKey(System.Int32,System.Int32,System.Int32,System.Int32,System.Byte[],System.Int32&):System.Int32",
        MessageId = "0")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Portability",
        "CA1901:PInvokeDeclarationsShouldBePortable", Scope = "member",
        Target =
            "LLCryptoLib.Security.SspiProvider.CryptExportKey(System.Int32,System.Int32,System.Int32,System.Int32,System.Byte[],System.Int32&):System.Int32",
        MessageId = "1")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1060:MovePInvokesToNativeMethodsClass",
        Scope = "member",
        Target =
            "LLCryptoLib.Security.SspiProvider.CryptExportKey(System.Int32,System.Int32,System.Int32,System.Int32,System.IntPtr,System.Int32&):System.Int32"
    )]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Portability",
        "CA1901:PInvokeDeclarationsShouldBePortable", Scope = "member",
        Target =
            "LLCryptoLib.Security.SspiProvider.CryptExportKey(System.Int32,System.Int32,System.Int32,System.Int32,System.IntPtr,System.Int32&):System.Int32",
        MessageId = "0")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Portability",
        "CA1901:PInvokeDeclarationsShouldBePortable", Scope = "member",
        Target =
            "LLCryptoLib.Security.SspiProvider.CryptExportKey(System.Int32,System.Int32,System.Int32,System.Int32,System.IntPtr,System.Int32&):System.Int32",
        MessageId = "1")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode",
        Scope = "member",
        Target =
            "LLCryptoLib.Security.SspiProvider.CryptGetKeyParam(System.Int32,System.Int32,System.Byte[],System.Int32&,System.Int32):System.Int32"
    )]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1060:MovePInvokesToNativeMethodsClass",
        Scope = "member",
        Target =
            "LLCryptoLib.Security.SspiProvider.CryptGetKeyParam(System.Int32,System.Int32,System.Byte[],System.Int32&,System.Int32):System.Int32"
    )]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Portability",
        "CA1901:PInvokeDeclarationsShouldBePortable", Scope = "member",
        Target =
            "LLCryptoLib.Security.SspiProvider.CryptGetKeyParam(System.Int32,System.Int32,System.Byte[],System.Int32&,System.Int32):System.Int32",
        MessageId = "0")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode",
        Scope = "member",
        Target =
            "LLCryptoLib.Security.SspiProvider.CryptGetKeyParam(System.Int32,System.Int32,System.Int32&,System.Int32&,System.Int32):System.Int32"
    )]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1060:MovePInvokesToNativeMethodsClass",
        Scope = "member",
        Target =
            "LLCryptoLib.Security.SspiProvider.CryptGetKeyParam(System.Int32,System.Int32,System.Int32&,System.Int32&,System.Int32):System.Int32"
    )]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Portability",
        "CA1901:PInvokeDeclarationsShouldBePortable", Scope = "member",
        Target =
            "LLCryptoLib.Security.SspiProvider.CryptGetKeyParam(System.Int32,System.Int32,System.Int32&,System.Int32&,System.Int32):System.Int32",
        MessageId = "0")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode",
        Scope = "member",
        Target =
            "LLCryptoLib.Security.SspiProvider.CryptGetKeyParam(System.Int32,System.Int32,System.IntPtr&,System.Int32&,System.Int32):System.Int32"
    )]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1060:MovePInvokesToNativeMethodsClass",
        Scope = "member",
        Target =
            "LLCryptoLib.Security.SspiProvider.CryptGetKeyParam(System.Int32,System.Int32,System.IntPtr&,System.Int32&,System.Int32):System.Int32"
    )]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Portability",
        "CA1901:PInvokeDeclarationsShouldBePortable", Scope = "member",
        Target =
            "LLCryptoLib.Security.SspiProvider.CryptGetKeyParam(System.Int32,System.Int32,System.IntPtr&,System.Int32&,System.Int32):System.Int32",
        MessageId = "0")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode",
        Scope = "member",
        Target =
            "LLCryptoLib.Security.SspiProvider.CryptHashData(System.Int32,System.Byte[],System.Int32,System.Int32):System.Int32"
    )]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1060:MovePInvokesToNativeMethodsClass",
        Scope = "member",
        Target =
            "LLCryptoLib.Security.SspiProvider.CryptHashData(System.Int32,System.Byte[],System.Int32,System.Int32):System.Int32"
    )]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Portability",
        "CA1901:PInvokeDeclarationsShouldBePortable", Scope = "member",
        Target =
            "LLCryptoLib.Security.SspiProvider.CryptHashData(System.Int32,System.Byte[],System.Int32,System.Int32):System.Int32",
        MessageId = "0")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode",
        Scope = "member",
        Target =
            "LLCryptoLib.Security.SspiProvider.CryptHashData(System.Int32,System.IntPtr,System.Int32,System.Int32):System.Int32"
    )]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1060:MovePInvokesToNativeMethodsClass",
        Scope = "member",
        Target =
            "LLCryptoLib.Security.SspiProvider.CryptHashData(System.Int32,System.IntPtr,System.Int32,System.Int32):System.Int32"
    )]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Portability",
        "CA1901:PInvokeDeclarationsShouldBePortable", Scope = "member",
        Target =
            "LLCryptoLib.Security.SspiProvider.CryptHashData(System.Int32,System.IntPtr,System.Int32,System.Int32):System.Int32",
        MessageId = "0")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1060:MovePInvokesToNativeMethodsClass",
        Scope = "member",
        Target =
            "LLCryptoLib.Security.SspiProvider.CryptImportKey(System.Int32,System.Byte[],System.Int32,System.Int32,System.Int32,System.Int32&):System.Int32"
    )]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Portability",
        "CA1901:PInvokeDeclarationsShouldBePortable", Scope = "member",
        Target =
            "LLCryptoLib.Security.SspiProvider.CryptImportKey(System.Int32,System.Byte[],System.Int32,System.Int32,System.Int32,System.Int32&):System.Int32",
        MessageId = "0")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Portability",
        "CA1901:PInvokeDeclarationsShouldBePortable", Scope = "member",
        Target =
            "LLCryptoLib.Security.SspiProvider.CryptImportKey(System.Int32,System.Byte[],System.Int32,System.Int32,System.Int32,System.Int32&):System.Int32",
        MessageId = "3")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode",
        Scope = "member",
        Target =
            "LLCryptoLib.Security.SspiProvider.CryptImportKey(System.Int32,System.IntPtr,System.Int32,System.Int32,System.Int32,System.Int32&):System.Int32"
    )]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1060:MovePInvokesToNativeMethodsClass",
        Scope = "member",
        Target =
            "LLCryptoLib.Security.SspiProvider.CryptImportKey(System.Int32,System.IntPtr,System.Int32,System.Int32,System.Int32,System.Int32&):System.Int32"
    )]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Portability",
        "CA1901:PInvokeDeclarationsShouldBePortable", Scope = "member",
        Target =
            "LLCryptoLib.Security.SspiProvider.CryptImportKey(System.Int32,System.IntPtr,System.Int32,System.Int32,System.Int32,System.Int32&):System.Int32",
        MessageId = "0")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Portability",
        "CA1901:PInvokeDeclarationsShouldBePortable", Scope = "member",
        Target =
            "LLCryptoLib.Security.SspiProvider.CryptImportKey(System.Int32,System.IntPtr,System.Int32,System.Int32,System.Int32,System.Int32&):System.Int32",
        MessageId = "3")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode",
        Scope = "member",
        Target =
            "LLCryptoLib.Security.SspiProvider.CryptSetKeyParam(System.Int32,System.Int32,LLCryptoLib.Security.DataBlob&,System.Int32):System.Int32"
    )]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1060:MovePInvokesToNativeMethodsClass",
        Scope = "member",
        Target =
            "LLCryptoLib.Security.SspiProvider.CryptSetKeyParam(System.Int32,System.Int32,LLCryptoLib.Security.DataBlob&,System.Int32):System.Int32"
    )]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Portability",
        "CA1901:PInvokeDeclarationsShouldBePortable", Scope = "member",
        Target =
            "LLCryptoLib.Security.SspiProvider.CryptSetKeyParam(System.Int32,System.Int32,LLCryptoLib.Security.DataBlob&,System.Int32):System.Int32",
        MessageId = "0")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode",
        Scope = "member",
        Target =
            "LLCryptoLib.Security.SspiProvider.CryptSetKeyParam(System.Int32,System.Int32,System.Byte[],System.Int32):System.Int32"
    )]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1060:MovePInvokesToNativeMethodsClass",
        Scope = "member",
        Target =
            "LLCryptoLib.Security.SspiProvider.CryptSetKeyParam(System.Int32,System.Int32,System.Byte[],System.Int32):System.Int32"
    )]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Portability",
        "CA1901:PInvokeDeclarationsShouldBePortable", Scope = "member",
        Target =
            "LLCryptoLib.Security.SspiProvider.CryptSetKeyParam(System.Int32,System.Int32,System.Byte[],System.Int32):System.Int32",
        MessageId = "0")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode",
        Scope = "member",
        Target =
            "LLCryptoLib.Security.SspiProvider.CryptSetKeyParam(System.Int32,System.Int32,System.Int32&,System.Int32):System.Int32"
    )]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1060:MovePInvokesToNativeMethodsClass",
        Scope = "member",
        Target =
            "LLCryptoLib.Security.SspiProvider.CryptSetKeyParam(System.Int32,System.Int32,System.Int32&,System.Int32):System.Int32"
    )]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Portability",
        "CA1901:PInvokeDeclarationsShouldBePortable", Scope = "member",
        Target =
            "LLCryptoLib.Security.SspiProvider.CryptSetKeyParam(System.Int32,System.Int32,System.Int32&,System.Int32):System.Int32",
        MessageId = "0")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security",
        "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands", Scope = "member",
        Target = "LLCryptoLib.Security.Authentication.CharEnumerator..ctor(System.Security.SecureString)")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security",
        "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands", Scope = "member",
        Target = "LLCryptoLib.Security.Authentication.CredentialCollection..ctor(System.String)")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1805:DoNotInitializeUnnecessarily",
        Scope = "member",
        Target = "LLCryptoLib.Security.Certificates.Certificate..ctor(LLCryptoLib.Security.Certificates.Certificate)")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2208:InstantiateArgumentExceptionsCorrectly",
        Scope = "member",
        Target = "LLCryptoLib.Security.Certificates.Certificate..ctor(LLCryptoLib.Security.Certificates.Certificate)")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1805:DoNotInitializeUnnecessarily",
        Scope = "member",
        Target =
            "LLCryptoLib.Security.Certificates.Certificate..ctor(System.IntPtr,LLCryptoLib.Security.Certificates.CertificateStore)"
    )]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1805:DoNotInitializeUnnecessarily",
        Scope = "member", Target = "LLCryptoLib.Security.Certificates.Certificate..ctor(System.IntPtr,System.Boolean)")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2208:InstantiateArgumentExceptionsCorrectly",
        Scope = "member",
        Target =
            "LLCryptoLib.Security.Certificates.Certificate.AssociateWithPrivateKey(System.String,System.String,System.Boolean):System.Void"
    )]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2200:RethrowToPreserveStackDetails",
        Scope = "member",
        Target =
            "LLCryptoLib.Security.Certificates.Certificate.AssociateWithPrivateKey(System.String,System.String,System.Boolean):System.Void"
    )]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2208:InstantiateArgumentExceptionsCorrectly",
        Scope = "member",
        Target =
            "LLCryptoLib.Security.Certificates.Certificate.CreateFromCerFile(System.Byte[]):LLCryptoLib.Security.Certificates.Certificate"
    )]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security",
        "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands", Scope = "member",
        Target =
            "LLCryptoLib.Security.Certificates.Certificate.CreateFromCerFile(System.Byte[],System.Int32,System.Int32):LLCryptoLib.Security.Certificates.Certificate"
    )]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2208:InstantiateArgumentExceptionsCorrectly",
        Scope = "member",
        Target =
            "LLCryptoLib.Security.Certificates.Certificate.CreateFromCerFile(System.Byte[],System.Int32,System.Int32):LLCryptoLib.Security.Certificates.Certificate"
    )]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1807:AvoidUnnecessaryStringCreation",
        Scope = "member", Target = "LLCryptoLib.HexStyler.Format(System.String,LLCryptoLib.HexStyle):System.String",
        MessageId = "output")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2208:InstantiateArgumentExceptionsCorrectly",
        Scope = "member",
        Target =
            "LLCryptoLib.Security.Certificates.Certificate.CreateFromPemFile(System.Byte[]):LLCryptoLib.Security.Certificates.Certificate"
    )]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security",
        "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands", Scope = "member",
        Target =
            "LLCryptoLib.Security.Certificates.Certificate.DecodeExtension(LLCryptoLib.Security.Certificates.Extension,System.IntPtr,System.Type):System.Object"
    )]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2208:InstantiateArgumentExceptionsCorrectly",
        Scope = "member",
        Target =
            "LLCryptoLib.Security.Certificates.Certificate.DecodeExtension(LLCryptoLib.Security.Certificates.Extension,System.IntPtr,System.Type):System.Object"
    )]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2200:RethrowToPreserveStackDetails",
        Scope = "member",
        Target =
            "LLCryptoLib.Security.Certificates.Certificate.DecodeExtension(LLCryptoLib.Security.Certificates.Extension,System.IntPtr,System.Type):System.Object"
    )]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security",
        "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands", Scope = "member",
        Target =
            "LLCryptoLib.Security.Certificates.Certificate.DecodeExtension(LLCryptoLib.Security.Certificates.Extension,System.String,System.Type):System.Object"
    )]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security",
        "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands", Scope = "member",
        Target =
            "LLCryptoLib.Security.Certificates.CertificateChain..ctor(LLCryptoLib.Security.Certificates.Certificate,LLCryptoLib.Security.Certificates.CertificateStore,LLCryptoLib.Security.Certificates.CertificateChainOptions)"
    )]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2208:InstantiateArgumentExceptionsCorrectly",
        Scope = "member",
        Target =
            "LLCryptoLib.Security.Certificates.CertificateChain..ctor(LLCryptoLib.Security.Certificates.Certificate,LLCryptoLib.Security.Certificates.CertificateStore,LLCryptoLib.Security.Certificates.CertificateChainOptions)"
    )]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security",
        "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands", Scope = "member",
        Target =
            "LLCryptoLib.Security.Certificates.CertificateChain.VerifyChain(System.String,LLCryptoLib.Security.Certificates.AuthType,LLCryptoLib.Security.Certificates.VerificationFlags):LLCryptoLib.Security.Certificates.CertificateStatus"
    )]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2208:InstantiateArgumentExceptionsCorrectly",
        Scope = "member",
        Target =
            "LLCryptoLib.Security.Certificates.CertificateStore..ctor(LLCryptoLib.Security.Certificates.CertificateStore)"
    )]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2208:InstantiateArgumentExceptionsCorrectly",
        Scope = "member",
        Target =
            "LLCryptoLib.Security.Certificates.CertificateStore..ctor(LLCryptoLib.Security.Certificates.StoreLocation,System.String)"
    )]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security",
        "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands", Scope = "member",
        Target =
            "LLCryptoLib.Security.Certificates.CertificateStore..ctor(System.Byte[],LLCryptoLib.Security.Certificates.CertificateStoreType)"
    )]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2208:InstantiateArgumentExceptionsCorrectly",
        Scope = "member",
        Target =
            "LLCryptoLib.Security.Certificates.CertificateStore..ctor(System.Byte[],LLCryptoLib.Security.Certificates.CertificateStoreType)"
    )]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods",
        Scope = "member",
        Target = "LLCryptoLib.Security.Certificates.CertificateStore..ctor(System.Collections.IEnumerable)")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Interoperability",
        "CA1404:CallGetLastErrorImmediatelyAfterPInvoke", Scope = "member",
        Target =
            "LLCryptoLib.Security.Certificates.CertificateStore.CreateFromPfxFile(System.Byte[],System.String,System.Boolean,LLCryptoLib.Security.Certificates.KeysetLocation):LLCryptoLib.Security.Certificates.CertificateStore"
    )]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security",
        "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands", Scope = "member",
        Target =
            "LLCryptoLib.Security.Certificates.CertificateStore.CreateFromPfxFile(System.Byte[],System.String,System.Boolean,LLCryptoLib.Security.Certificates.KeysetLocation):LLCryptoLib.Security.Certificates.CertificateStore"
    )]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2208:InstantiateArgumentExceptionsCorrectly",
        Scope = "member",
        Target =
            "LLCryptoLib.Security.Certificates.CertificateStore.CreateFromPfxFile(System.Byte[],System.String,System.Boolean,LLCryptoLib.Security.Certificates.KeysetLocation):LLCryptoLib.Security.Certificates.CertificateStore"
    )]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security",
        "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands", Scope = "member",
        Target =
            "LLCryptoLib.Security.Certificates.CertificateStore.FindCertificateByHash(System.Byte[],LLCryptoLib.Security.Certificates.HashType):LLCryptoLib.Security.Certificates.Certificate"
    )]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2208:InstantiateArgumentExceptionsCorrectly",
        Scope = "member",
        Target =
            "LLCryptoLib.Security.Certificates.CertificateStore.FindCertificateByHash(System.Byte[],LLCryptoLib.Security.Certificates.HashType):LLCryptoLib.Security.Certificates.Certificate"
    )]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security",
        "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands", Scope = "member",
        Target =
            "LLCryptoLib.Security.Certificates.CertificateStore.FindCertificateBySubjectName(System.String,LLCryptoLib.Security.Certificates.Certificate):LLCryptoLib.Security.Certificates.Certificate"
    )]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2208:InstantiateArgumentExceptionsCorrectly",
        Scope = "member",
        Target =
            "LLCryptoLib.Security.Certificates.CertificateStore.FindCertificateBySubjectName(System.String,LLCryptoLib.Security.Certificates.Certificate):LLCryptoLib.Security.Certificates.Certificate"
    )]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2208:InstantiateArgumentExceptionsCorrectly",
        Scope = "member",
        Target =
            "LLCryptoLib.Security.Certificates.CertificateStore.FindCertificateBySubjectString(System.String,LLCryptoLib.Security.Certificates.Certificate):LLCryptoLib.Security.Certificates.Certificate"
    )]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security",
        "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands", Scope = "member",
        Target =
            "LLCryptoLib.Security.Certificates.CertificateStore.FindCertificateByUsage(System.String[],LLCryptoLib.Security.Certificates.Certificate):LLCryptoLib.Security.Certificates.Certificate"
    )]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2208:InstantiateArgumentExceptionsCorrectly",
        Scope = "member",
        Target =
            "LLCryptoLib.Security.Certificates.CertificateStore.FindCertificateByUsage(System.String[],LLCryptoLib.Security.Certificates.Certificate):LLCryptoLib.Security.Certificates.Certificate"
    )]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage",
        "CA2214:DoNotCallOverridableMethodsInConstructors", Scope = "member",
        Target =
            "LLCryptoLib.Security.Certificates.CertificateStoreCollection..ctor(LLCryptoLib.Security.Certificates.CertificateStore[])"
    )]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2208:InstantiateArgumentExceptionsCorrectly",
        Scope = "member",
        Target =
            "LLCryptoLib.Security.Certificates.CertificateStoreCollection..ctor(LLCryptoLib.Security.Certificates.CertificateStore[])"
    )]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2208:InstantiateArgumentExceptionsCorrectly",
        Scope = "member",
        Target =
            "LLCryptoLib.Security.Certificates.CertificateStoreCollection..ctor(LLCryptoLib.Security.Certificates.CertificateStoreCollection)"
    )]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1805:DoNotInitializeUnnecessarily",
        Scope = "member",
        Target =
            "LLCryptoLib.Security.Certificates.CertificateVerificationResult..ctor(LLCryptoLib.Security.Certificates.CertificateChain,System.String,LLCryptoLib.Security.Certificates.AuthType,LLCryptoLib.Security.Certificates.VerificationFlags,System.AsyncCallback,System.Object)"
    )]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode",
        Scope = "member",
        Target = "LLCryptoLib.Security.Certificates.DistinguishedName..ctor(LLCryptoLib.Security.CertificateNameInfo)")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security",
        "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands", Scope = "member",
        Target = "LLCryptoLib.Security.Certificates.DistinguishedName..ctor(System.IntPtr,System.Int32)")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2200:RethrowToPreserveStackDetails",
        Scope = "member",
        Target = "LLCryptoLib.Security.Certificates.DistinguishedName..ctor(System.IntPtr,System.Int32)")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2208:InstantiateArgumentExceptionsCorrectly",
        Scope = "member", Target = "LLCryptoLib.Security.Certificates.DistinguishedNameList.Item[System.Int32]")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1805:DoNotInitializeUnnecessarily",
        Scope = "member", Target = "LLCryptoLib.Security.Cryptography.CAPIProvider..ctor()")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security",
        "CA2122:DoNotIndirectlyExposeMethodsWithLinkDemands", Scope = "member",
        Target = "LLCryptoLib.Security.Cryptography.RC4CryptoServiceProvider..ctor()")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:DisposeObjectsBeforeLosingScope",
        Scope = "member",
        Target = "LLCryptoLib.Security.Cryptography.StringEncryption.Encrypt(System.Byte[]):System.String")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security",
        "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target =
            "LLCryptoLib.Security.Win32.NativeMethods.CryptGetKeyParam(System.IntPtr,System.Int32,System.Int32&,System.Int32&,System.Int32):System.Int32"
    )]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security",
        "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target =
            "LLCryptoLib.Security.Win32.NativeMethods.CryptGetKeyParam(System.IntPtr,System.Int32,System.IntPtr&,System.Int32&,System.Int32):System.Int32"
    )]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security",
        "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target =
            "LLCryptoLib.Security.Win32.NativeMethods.CryptImportKey(System.IntPtr,System.Byte[],System.Int32,System.IntPtr,System.Int32,System.IntPtr&):System.Int32"
    )]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security",
        "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target =
            "LLCryptoLib.Security.Win32.NativeMethods.CryptImportKey(System.IntPtr,System.IntPtr,System.Int32,System.IntPtr,System.Int32,System.IntPtr&):System.Int32"
    )]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security",
        "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target =
            "LLCryptoLib.Security.Win32.NativeMethods.CryptSetKeyParam(System.IntPtr,System.Int32,System.Byte[],System.Int32):System.Int32"
    )]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security",
        "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target =
            "LLCryptoLib.Security.Win32.NativeMethods.CryptSetKeyParam(System.IntPtr,System.Int32,System.Int32&,System.Int32):System.Int32"
    )]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security",
        "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage", Scope = "member",
        Target =
            "LLCryptoLib.Security.Win32.NativeMethods.FormatMessage(System.Int32,System.IntPtr,System.UInt32,System.Int32,System.IntPtr&,System.Int32,System.IntPtr):System.Int32"
    )]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode",
        Scope = "member",
        Target = "LLCryptoLib.Security.Win32.NativeMethods.SCardFreeMemory(System.IntPtr,System.Byte*):System.Int32")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode",
        Scope = "member",
        Target = "LLCryptoLib.Security.Win32.NativeMethods.SCardFreeMemory(System.IntPtr,System.IntPtr):System.Int32")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode",
        Scope = "member",
        Target =
            "LLCryptoLib.Security.Win32.NativeMethods.SCardGetAttrib(System.IntPtr,System.Int32,System.IntPtr&,System.Int32&):System.Int32"
    )]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode",
        Scope = "member",
        Target =
            "LLCryptoLib.Security.Win32.NativeMethods.SCardGetAttrib(System.IntPtr,System.Int32,System.UInt32&,System.Int32&):System.Int32"
    )]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode",
        Scope = "member", Target = "LLCryptoLib.Security.Win32.SCARD_IO_REQUEST..ctor(System.Int32,System.Int32)")]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:DisposeObjectsBeforeLosingScope",
        Scope = "member",
        Target =
            "LLCryptoLib.Shred.Shredder.WipeFile(System.String,LLCryptoLib.Shred.IShredMethod,LLCryptoLib.CallbackEntry,System.Threading.AutoResetEvent):System.Boolean"
    )]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1706:ShortAcronymsShouldBeUppercase",
        Scope = "member", Target = "LLCryptoLib.Utils.RandomString.GetEx(System.Int32,System.Boolean):System.String",
        MessageId = "Member")]
#endif

[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters",
        Scope = "member",
        Target =
            "LLCryptoLib.Crypto.AsymmetricCrypter.Decrypt(System.IO.FileInfo,System.IO.FileInfo,System.Security.Cryptography.RSA):System.Void"
    )]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters",
        Scope = "member",
        Target =
            "LLCryptoLib.Crypto.AsymmetricCrypter.Encrypt(System.IO.FileInfo,System.IO.FileInfo,System.Security.Cryptography.RSA):System.Void"
    )]
[assembly:
    System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode",
        Scope = "member",
        Target =
            "LLCryptoLib.Security.Win32.NativeMethods.FormatMessage(System.Int32,System.IntPtr,System.UInt32,System.Int32,System.IntPtr&,System.Int32,System.IntPtr):System.Int32"
    )]