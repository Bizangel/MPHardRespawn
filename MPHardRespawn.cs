using Terraria.ModLoader;
using Terraria;
using Terraria.ID;
using Terraria.Localization;

using Terraria.Chat;
using System.IO;
using TrustyPacket;

namespace MPHardRespawn
{
	public class MPHardRespawn : Mod
	{
		public override void HandlePacket(BinaryReader reader, int whoAmI)
		{
			// Leave the handling to TrustyPacket.
			TrustyPacketManager.HandlePacket(reader,whoAmI);


			//byte msg = reader.ReadByte();
			//if (Main.netMode == NetmodeID.Server)
			//{
			//	if (msg == 0) // Receives player report as connected and active player.
			//	{
			//		ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral("Recieved Report Request from:  " + whoAmI + " " + Main.player[whoAmI].name)
			//			, Colors.RarityGreen);
			//		ModContent.GetInstance<MPHardRespawnModSystem>().playerCalled = true; 
			//		//RemoteClient.CheckSection(whoAmI, Utils.ReadVector2(reader), 1);
			//		ModPacket myPacket = GetPacket();
			//		myPacket.Write((byte)1); // acknowledge packet
			//		myPacket.Send(whoAmI);
			//	}
			//}
			//else if (Main.netMode == NetmodeID.MultiplayerClient)
			//{
			//	if (msg == 1)
			//	{  // acknowledge message
			//		Main.NewText("Received acknowledge message!");
					
			//	}
			//}

		}
	}
}