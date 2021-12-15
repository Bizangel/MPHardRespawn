using Terraria.ModLoader;
using Terraria;
using Terraria.ID;
using Terraria.Localization;

using Terraria.Chat;
using System.IO;

using System.Collections;

namespace MPHardRespawn
{
	public class MPHardRespawn : Mod
	{

        public override void Load()
        {
        }
        public override void HandlePacket(BinaryReader reader, int whoAmI)
        {

            byte packetID = reader.ReadByte();
            if (packetID == 0 && Main.netMode == NetmodeID.MultiplayerClient)
            {
                // Acknowledge Death.
                // Simulate a PVP death, so no drops.
                string playername = Main.player[Main.myPlayer].name;

                Main.player[Main.myPlayer].KillMe(Terraria.DataStructures.PlayerDeathReason.ByCustomReason($"{playername} attempted to join during boss fight..."), 9999f, 0, true);
            }
        }
    }
}