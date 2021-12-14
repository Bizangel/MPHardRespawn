using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Input;
using Terraria.ID;
using Terraria.Localization;
using Microsoft.Xna.Framework;
using Terraria.UI;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace MPHardRespawn
{
	class MPHardRespawnPlayer : ModPlayer
	{
        //private MPHardRespawnModSystem modsystem;
        //public override void Initialize()
        //{
        //    modsystem = ModContent.GetInstance<MPHardRespawnModSystem>();
        //}

        public override void UpdateDead()
        {
            if (Main.GameUpdateCount % 59u == 0) // Every half of a second check boss active or not status.
            {
                if (MPHardRespawnModSystem.IsBossActive())
                {
                    Main.player[Main.myPlayer].respawnTimer += 60; // don't let it tick down
                    if (Main.player[Main.myPlayer].respawnTimer < 60) {
                        Main.player[Main.myPlayer].respawnTimer = 119;
                    }
                }
            }
        }

        public override void OnEnterWorld(Player player)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient) {
                ModPacket myPacket = ModContent.GetInstance<MPHardRespawn>().GetPacket();
                myPacket.Write((byte)0); // report packet
                myPacket.Send(); // send to server
            }
        }
        //public override void UpdateDead() { 
        //}
    }
    }
