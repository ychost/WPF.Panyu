using Microsoft.VisualStudio.TestTools.UnitTesting;
using HuaWeiUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HuaWeiUtils.Tests {
    [TestClass()]
    public class EncryptionDecryptionTests {
        [TestMethod()]
        public void EncryptTest() {
            Console.WriteLine(EncryptionDecryption.Encrypt("PY-123456"));
            Console.WriteLine(EncryptionDecryption.Decrypt("Jb0JxAiP41QAKVPMlagmtQ=="));
        }
    }
}