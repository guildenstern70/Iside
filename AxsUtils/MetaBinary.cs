using System;
using System.IO;

namespace AxsUtils
{
    /// <summary>
    ///     MetaBinary class is a metadata file handler.
    ///     The class gets as input an existing binary file (binaryFile).
    ///     It is capable to save the same file with some metadata prepended.
    ///     While writing, the class:
    ///     - prepends metadata to binaryFile
    ///     - saves output MetaBinary file (metadata+binaryFile.extension)
    ///     While reading, the class:
    ///     - reads metadata from MetaBinary file (stores it into a public property)
    ///     - saves the original binaryFile
    ///     - deletes the MetaBinary file
    /// </summary>
    public class MetaBinary
    {
        protected const int METADATA_SIZE = 255;

        private readonly string binaryFilePath;
        private string prvMetadata;

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="binaryFilePath">Path to file with metadata to add or remove</param>
        public MetaBinary(string binFilePath)
        {
            if (File.Exists(binFilePath))
            {
                this.binaryFilePath = binFilePath;
                System.Diagnostics.Debug.WriteLine(">>> File with metadata is: " + binFilePath);
            }
            else
            {
                throw new IOException("Binary metadata input file does not exist.");
            }
            this.prvMetadata = string.Empty;
        }

        /// <summary>
        ///     Get metadata read in file
        /// </summary>
        public string Metadata
        {
            get
            {
                if (File.Exists(this.binaryFilePath))
                {
                    FileStream fsR = null;
                    BinaryReader r = null;
                    char[] rChars;

                    try
                    {
                        fsR = new FileStream(this.binaryFilePath, FileMode.Open);
                        fsR.Seek(-METADATA_SIZE, SeekOrigin.End);
                        r = new BinaryReader(fsR);
                        rChars = r.ReadChars(METADATA_SIZE);
                    }
                    catch (IOException mex)
                    {
                        rChars = null;
                        System.Diagnostics.Debug.WriteLine("!!! MetaBinary.Metadata");
                        System.Diagnostics.Debug.WriteLine("!!! Error " + mex.Message);
                        System.Diagnostics.Debug.WriteLine("!!! Cannot read metadata");
                    }
                    finally
                    {
                        if (r != null)
                        {
                            r.Close();
                        }
                        if (fsR != null)
                        {
                            fsR.Close();
                        }
                    }

                    if (rChars != null)
                    {
                        string rString = new string(rChars);
                        this.prvMetadata = rString.TrimEnd(' ');
                    }
                    else
                    {
                        this.prvMetadata = null;
                    }
                }
                else
                {
                    throw new IOException("BinMetadata file does not exist.");
                }

                return this.prvMetadata;
            }
        }

        /// <summary>
        ///     Add (append) metadata to original binary file
        /// </summary>
        /// <param name="metadata"></param>
        public void AddMetaBinary(string metadata)
        {
            // Create new metabinary file
            try
            {
                using (FileStream fsW = new FileStream(this.binaryFilePath, FileMode.Append))
                {
                    using (BinaryWriter w = new BinaryWriter(fsW))
                    {
                        System.Diagnostics.Debug.WriteLine(">>> Created new file: " + fsW.Name);
                        // Append metadata as first thing
                        w.Write(FitMetadataIntoChars(metadata));
                        System.Diagnostics.Debug.WriteLine(">>> Metadata appended to: " + this.binaryFilePath);
                    }
                }
            }
            catch (IOException ioex)
            {
                System.Diagnostics.Debug.WriteLine(ioex.Source);
                System.Diagnostics.Debug.WriteLine(ioex.Message);
                System.Diagnostics.Debug.WriteLine(ioex.StackTrace);
            }
        }


        /// <summary>
        ///     Replace metadata in a file with other metadata.
        /// </summary>
        /// <param name="metadata">New metadata</param>
        public void ReplaceMetadata(string metadata)
        {
            DeleteMetadata(this.binaryFilePath);
            this.AddMetaBinary(metadata);
        }

        /// <summary>
        ///     Copies a file with metadata to a file without it.
        ///     The original file is the one passed to the constructor.
        ///     The file without metadata is saved into outdir dir.
        /// </summary>
        /// <param name="newExtension">Extension (without preceding dot)</param>
        /// <param name="outdir">The directory in which saving the file without metadata</param>
        /// <returns>The complete path (path+filename) of the file without metadata</returns>
        public string RestoreOriginal(string outdir, string newExtension)
        {
            // Output file name
            FileInfo fi = new FileInfo(this.binaryFilePath);
            string outname = fi.Name + "." + newExtension;
            string outfile = Path.Combine(outdir, outname);

            // Copy it
            if (File.Exists(outfile))
            {
                File.Delete(outfile);
            }

            File.Copy(this.binaryFilePath, outfile);

            DeleteMetadata(outfile);
            System.Diagnostics.Debug.WriteLine(">>> Metadata removed from: " + this.binaryFilePath);
            System.Diagnostics.Debug.WriteLine(">>> File without metadata is now: " + outfile);

            return outfile;
        }


        /// <summary>
        ///     Save original file without metadata
        /// </summary>
        public void RestoreOriginal()
        {
            DeleteMetadata(this.binaryFilePath);
        }


        protected static void DeleteMetadata(string outfile)
        {
            // Truncate existing metabinary file
            using (FileStream fs = new FileStream(outfile, FileMode.Open))
            {
                fs.Seek(-255, SeekOrigin.End);
                NativeMethods.SetEndOfFile(fs.SafeFileHandle.DangerousGetHandle());
            }

            System.Diagnostics.Debug.WriteLine(">>> Metadata wiped out");
        }

        /// <summary>
        ///     Get a metadata string and return a fixed length char array
        /// </summary>
        /// <param name="metadata"></param>
        /// <returns></returns>
        protected static char[] FitMetadataIntoChars(string metadata)
        {
            if (metadata == null)
            {
                throw new ArgumentNullException("metadata");
            }

            char[] metadataChars = new char[METADATA_SIZE];

            if (metadata.Length < METADATA_SIZE)
            {
                int count = 0;
                foreach (char c in metadata)
                {
                    metadataChars[count] = c;
                    count++;
                }
                for (int j = count; j < METADATA_SIZE; j++)
                {
                    metadataChars[j] = ' ';
                }
            }
            else
            {
                for (int j = 0; j < METADATA_SIZE; j++)
                {
                    metadataChars[j] = metadata[j];
                }
            }

            return metadataChars;
        }

        // Test methods
        public static void Test()
        {
            // 1. Create Metabinary file
            MetaBinary mb = new MetaBinary(@"F:\Documents and Settings\asaltar\Desktop\temp.zip");
            mb.AddMetaBinary("QUESTO E' UN MESSAGGIO METABINARIZZATO");

            // 2. Get metadata from Metabinary file
            Console.WriteLine("METADATA INSIDE: [{0}]", mb.Metadata);

            // 3. Restore original file
            mb.RestoreOriginal(".", "metabin");
            Console.WriteLine("Temp zip written");
        }
    }
}