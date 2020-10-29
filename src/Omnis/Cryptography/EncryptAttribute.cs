using System;

namespace Omnis.Cryptography {
    /// <summary>
    /// Mark a property as encryptable
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class EncryptAttribute : Attribute {
    }
}
