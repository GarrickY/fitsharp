﻿// Copyright © 2016 Syterra Software Inc. All rights reserved.
// The use and distribution terms for this software are covered by the Common Public License 1.0 (http://opensource.org/licenses/cpl.php)
// which can be found in the file license.txt at the root of this distribution. By using this software in any fashion, you are agreeing
// to be bound by the terms of this license. You must not remove this notice, or any other, from this software.

using System.Text;

namespace fitSharp.IO {
    public class SocketSession: Session {
        public SocketSession(SocketModel socket) {
            this.socket = socket;
        }

        public string Read(int bytesToRead) {
            var bytes = new byte[bytesToRead];
			var bytesReceived = 0;
            while (bytesReceived < bytesToRead) {
			    bytesReceived += socket.Receive(bytes, bytesReceived, bytesToRead - bytesReceived);
            }
			var characters = new char[bytesToRead];
			var charCount = Encoding.UTF8.GetDecoder().GetChars(bytes, 0, bytesToRead, characters, 0);
			return new StringBuilder(charCount).Append(characters, 0, charCount).ToString();
        }

        public void Write(string message) {
			var messageBytes = Encoding.UTF8.GetBytes(message);
			socket.Send(messageBytes);
        }

        public void Write(string message, string prefixFormat) {
			var messageBytes = Encoding.UTF8.GetBytes(message);
            Write(string.Format(prefixFormat, messageBytes.Length));
			socket.Send(messageBytes);
        }

        public void Close() {
            socket.Close();
        }

        readonly SocketModel socket;
    }
}
