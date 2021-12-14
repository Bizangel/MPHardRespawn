using Terraria.ModLoader;
using Terraria;
using Terraria.ID;
using Terraria.Localization;

using Terraria.Net;
using Terraria.Chat;
using System.IO;

using MPHardRespawn;

namespace TrustyPacket {


    /// <summary>
    ///  A simple class that wraps around a modPacket. 
    ///  It ensures that modpackets are received succesfully by sending them until one is received.
    /// </summary>
    static public class TrustyPacketManager
    {

        public static void HandlePacket(BinaryReader reader, int whoAmI) {
            byte PacketID = reader.ReadByte();

            if (Main.netMode == NetmodeID.Server)
            {
                if (msg == 0) // Receives player report as connected and active player.
                {
                    ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral("Recieved Report Request from:  " + whoAmI + " " + Main.player[whoAmI].name)
                        , Colors.RarityGreen);
                    ModContent.GetInstance<MPHardRespawnModSystem>().playerCalled = true;
                    //RemoteClient.CheckSection(whoAmI, Utils.ReadVector2(reader), 1);
                    ModPacket myPacket = ModContent.GetInstance<MPHardRespawn.MPHardRespawn>().GetPacket();
                    myPacket.Write((byte)1); // acknowledge packet
                    myPacket.Send(whoAmI);
                }
            }
        }

        static private void OnServerReceive(BinaryReader reader, int whoAmI) { 
           
        }

        static private void OnClientReceive(BinaryReader reader, int whoAmI) { 
        }

        static public void SendTrustyPacket(BinaryReader reader, int whoAmI)
        {
            
        }

    }
}
