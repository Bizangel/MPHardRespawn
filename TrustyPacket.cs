//using Terraria.ModLoader;
//using Terraria;
//using Terraria.ID;
//using Terraria.Localization;

//using Terraria.Net;
//using Terraria.Chat;
//using System.IO;

//using System.Collections.Generic;

//using MPHardRespawn;
//using System;



//namespace TrustyPacket {

    

//    /// <summary>
//    /// Helper class, Allows for easy binary read and write via generics.
//    /// </summary>
//    static public class BinaryHelper {
//        private static readonly Dictionary<Type, IWriteRead> typeWriteReads = new Dictionary<Type, IWriteRead>() {
//            { typeof(int), new IntWriteRead() },
//            { typeof(float), new FloatWriteRead() },
//            { typeof(bool), new IntWriteRead() },
//            { typeof(string), new StringWriteRead() }
//        };

//        static public void Write<T>(ModPacket packet, T val){
//            ((IWriteRead<T>) typeWriteReads[typeof(T)]).Write(packet, val);
//        }

//        static public T Read<T>(BinaryReader reader)
//        {
//            return ((IWriteRead<T>)typeWriteReads[typeof(T)]).Read(reader);
//        }

//        private interface IWriteRead { }

//        private interface IWriteRead<T> : IWriteRead
//        {
//            void Write(ModPacket packet, T val);
//            T Read(BinaryReader reader);
//        }

//        class IntWriteRead : IWriteRead<int> {
//            public void Write(ModPacket packet, int val) {
//                packet.Write(val);
//            }
//            public int Read(BinaryReader reader)
//            {
//                return reader.ReadInt32();
//            }
//        }

//        class StringWriteRead : IWriteRead<string>
//        {
//            public void Write(ModPacket packet, string val)
//            {
//                packet.Write(val);
//            }
//            public string Read(BinaryReader reader)
//            {
//                return reader.ReadString();
//            }
//        }

//        class BoolWriteRead : IWriteRead<bool>
//        {
//            public void Write(ModPacket packet, bool val)
//            {
//                packet.Write(val);
//            }

//            public bool Read(BinaryReader reader)
//            {
//                return reader.ReadBoolean();
//            }
//        }

//        class FloatWriteRead : IWriteRead<float>
//        {
//            public void Write(ModPacket packet, float val)
//            {
//                packet.Write(val);
//            }

//            public float Read(BinaryReader reader)
//            {
//                return reader.ReadSingle();
//            }
//        }
//    }

//    /// <summary>
//    ///  A simple class that wraps around a modPacket. 
//    ///  It ensures that modpackets are received succesfully by sending them until one is received.
//    /// </summary>
//    public class TrustyPacketManager
//    {
//        public static readonly Dictionary<Type, byte> typeToID = new Dictionary<Type, byte>() {
//            { typeof(int), 0 },
//            { typeof(float), 1 },
//            { typeof(bool), 2 },
//            { typeof(string), 3 }
//        };

//        /// <summary>
//        /// Helper Packet Class. Stores packet to be sent.
//        /// </summary>
//        private class Packet<T> {
//            private byte _packetID;
//            private T _content;
//            private int _toWho;
//            private float _trustyTimeout;
//            private byte _typenameID;

//            public Packet(byte packetID, T content, int toWho = -1, float trustyTimeout = -1f) {
//                _packetID = packetID;
//                _content = content;
//                _toWho = toWho;
//                _trustyTimeout = trustyTimeout;
//                _typenameID = typeToID[typeof(T)];
//            }

//            /// <summary>
//            /// Sends stored packet information. Needs a newly created modPacket.
//            /// </summary>
//            /// <param name="packet">A newly created modPacket.</param>
//            public void WriteAndSend(ModPacket packet) {
//                bool requestEcho = _trustyTimeout > 0;
//                packet.Write(_packetID); // id
//                packet.Write(requestEcho); // echo trustyflag
//                packet.Write(_typenameID); // typename id
//                BinaryHelper.Write(packet, _content);
//                packet.Send(_toWho);
//            }
//        }

//        private Mod _mod;

//        private Dictionary<byte, Action<int,int>> _intHandles;
//        private Dictionary<byte, Action<float,int>> _floatHandles;
//        private Dictionary<byte, Action<bool,int>> _boolHandles;
//        private Dictionary<byte, Action<string,int>> _stringHandles;
//        private Dictionary<byte, bool> _clientSide;

//        //private Dictionary<byte, System.Delegate> _serverHandles;


//        public TrustyPacketManager(Mod m) {
//            _mod = m;
//            _intHandles = new Dictionary<byte, Action<int,int>>();
//            _floatHandles = new Dictionary<byte, Action<float,int>>();
//            _boolHandles = new Dictionary<byte, Action<bool,int>>();
//            _stringHandles = new Dictionary<byte, Action<string,int>>();
//            _clientSide = new Dictionary<byte,bool>();
//        }


//        public void HandlePacket(BinaryReader reader, int whoAmI) {
//            byte PacketID = reader.ReadByte();
//            bool echoRequested = reader.ReadBoolean();
//            byte typeID = reader.ReadByte();
//            if (Main.netMode == NetmodeID.SinglePlayer) {
//                return;
//            }

//            if (Main.netMode == NetmodeID.MultiplayerClient && _clientSide[PacketID]) {
//                return;
//            }

//            if (Main.netMode == NetmodeID.MultiplayerClient && !_clientSide[PacketID])
//            {
//                return;
//            }

//            switch (typeID) {
//                case 0:
//                    _intHandles[PacketID].Invoke(reader.ReadInt32(), whoAmI);
//                    break;
//                case 1:
//                    _floatHandles[PacketID].Invoke(reader.ReadSingle(), whoAmI);
//                    break;
//                case 2:
//                    _boolHandles[PacketID].Invoke(reader.ReadBoolean(), whoAmI);
//                    break;
//                case 3:
//                    _stringHandles[PacketID].Invoke(reader.ReadString(), whoAmI);
//                    break;
//            }

//            //packet.Write(_packetID); // id
//            //packet.Write(requestEcho); // echo trustyflag
//            //packet.Write((byte)0); // typename id
//            //BinaryHelper.Write(packet, _content);
//            //packet.Send(_toWho);


//            //throw new ArgumentException("Index is out of range", nameof(index), ex);

//            //if (Main.netMode == NetmodeID.Server)
//            //{
//            //    if (msg == 0) // Receives player report as connected and active player.
//            //    {
//            //        ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral("Recieved Report Request from:  " + whoAmI + " " + Main.player[whoAmI].name)
//            //            , Colors.RarityGreen);
//            //        ModContent.GetInstance<MPHardRespawnModSystem>().playerCalled = true;
//            //        //RemoteClient.CheckSection(whoAmI, Utils.ReadVector2(reader), 1);
//            //        ModPacket myPacket = ModContent.GetInstance<MPHardRespawn.MPHardRespawn>().GetPacket();
//            //        myPacket.Write((byte)1); // acknowledge packet
//            //        myPacket.Send(whoAmI);
//            //    }
//            //    OnServerReceive(byte)
//            //}
//        }

//        /// <summary>
//        /// Called everyframe. Must be called inside PostUpdateEverything. It handles Packet resend logic and such.
//        /// </summary>
//        public void PacketManagerUpdate() { 
            
//        }

//        /*
//         *  Typename IDS: 
//         *  0 int
//         *  1 float
//         *  2 bool
//         *  3 string
//         */

//        /// <summary>
//        /// Sends a new TrustyPacket message. Must be handled by a trustypacket registered handler.
//        /// </summary>
//        /// <typeparam name="T">Type of Message, Either Bool, String or Int.</typeparam>
//        /// <param name="packetID">The ID of the packet.</param>
//        /// <param name="content">The content of the packet. Must match typename</param>
//        /// <param name="toWho">PlayerID to send the message. Leave -1 for server.</param>
//        /// <param name="trustyTimeout">Time in seconds to wait, if no packet acknowledge is received. Leave negative for non-packet acknowledge.  </param>
//        public void Send<T>(byte packetID, T content, int toWho = -1, float trustyTimeout=-1f) 
//        {
//            var packet = new Packet<T>(packetID,content,toWho,trustyTimeout);
//            packet.WriteAndSend(_mod.GetPacket());
//        }


//        public void RegisterIntPacketHandler(byte packetID, Action<int, int> del, bool clientSide = false) {
//            _intHandles.Add(packetID, del);
//            _clientSide.Add(packetID, clientSide);
//        }

//        public void RegisterBoolPacketHandler(byte packetID, Action<bool, int> del, bool clientSide = false)
//        {
//            _boolHandles.Add(packetID, del);
//            _clientSide.Add(packetID, clientSide);
//        }

//        public void RegisterStringPacketHandler(byte packetID, Action<string, int> del, bool clientSide = false)
//        {
//            _stringHandles.Add(packetID, del);
//            _clientSide.Add(packetID, clientSide);
//        }

//        public void RegisterFloatPacketHandler(byte packetID, Action<float,int> del, bool clientSide = false)
//        {
//            _floatHandles.Add(packetID, del);
//            _clientSide.Add(packetID, clientSide);
//        }
//    }
//}
