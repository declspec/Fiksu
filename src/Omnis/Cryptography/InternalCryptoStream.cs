using System.IO;
using System.Security.Cryptography;

namespace Omnis.Cryptography {
    /// <summary>
    /// An extended version of <see cref="CryptoStream"/> which allows greater control over the Transform and underlying Stream.
    /// </summary>
    public class ExtendedCryptoStream : CryptoStream {
        // An internal _leaveOpen is only needed until a CryptoStream constructor exists in .NET standard that accepts a
        //  'leaveOpen' parameter which allows you to leave the underlying stream open.
        // This constructor exists in .NET Framework >= 4.7.2 and .NET Core >= 2.0 but is not yet available
        //  in .NET Standard as of .NET Standard 2.0.
        // Future versions should support the API (hopefully) and will allow us to do away with this variable and all related code.
        private readonly bool _leaveOpen;
        private readonly bool _disposeTransform;

        protected ICryptoTransform Transform { get; }

        public ExtendedCryptoStream(Stream stream, ICryptoTransform transform, CryptoStreamMode mode, bool leaveOpen = false, bool disposeTransform = false)
            : base(stream, transform, mode) {
            Transform = transform;
            _leaveOpen = leaveOpen;
            _disposeTransform = disposeTransform;
        }

        protected override void Dispose(bool disposing) {
            if (disposing) {
                if (_leaveOpen) {
                    // Re-implement the core parts of CryptoStream.Dispose(disposing)
                    if (!HasFlushedFinalBlock)
                        FlushFinalBlock();

                    // Switching to 'false' here will do all cleanup except for closing the underlying stream.
                    disposing = false;
                }

                if (_disposeTransform)
                    Transform.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}