using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace WpfHuaWei.DataService
{
    internal class StateObject
    {
        public static int BufferSize = 1024 * 1024 * 10;

        public Socket socket;

        public byte[] buffer;

        public StateObject()
        {
            this.buffer = new byte[BufferSize];
        }
    }
}
