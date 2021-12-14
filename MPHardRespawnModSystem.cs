using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Input;
using Terraria.ID;
using Terraria.Chat;

using Terraria.Localization;
using Terraria.Net;

using Microsoft.Xna.Framework;
using Terraria.UI;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace MPHardRespawn
{
	class MPHardRespawnModSystem : ModSystem
	{
		public Player[] activePlayers = null;
		public bool playerCalled = false;

		public static bool IsBossActive() {
			// Returns True if a boss is active. Does not include mini-bosses. Does not include event-bossess.
			foreach (NPC mob in Main.npc)
			{
				if (mob.active && mob.boss) {
					return true;
				}
			}
			if (NPC.AnyNPCs(NPCID.EaterofWorldsBody) || NPC.AnyNPCs(NPCID.EaterofWorldsHead) || NPC.AnyNPCs(NPCID.EaterofWorldsTail)) {
				return true;
			}

			return false;
		}

		public void onClientConnect() { // Called when a new client reports as connected.
			
		}

        public override void PostUpdateEverything()
        {
			if (Main.netMode == NetmodeID.Server) {
				
				if (Main.GameUpdateCount % 30u == 0) 
				{

                    string activeplayers = "";
                    for (int j = 0; j < 256; j++)
                    {
                        if (Main.player[j].active)
                        {
                            activeplayers += Main.player[j].name + " | ";
                        }
                    }
                    //ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral("Called: " + playerCalled), Color.White);
                    ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral("Active players: " + activeplayers), Color.White);
                }

				
			}


			if (JustPressed(Keys.Z))
				TestMethod();

		}

		public void TestMethod() {
			Main.NewText("Is boss active?: " + IsBossActive());
			
		}

        public static bool JustPressed(Keys key)
		{
			return Main.keyState.IsKeyDown(key) && !Main.oldKeyState.IsKeyDown(key);
		}
	}
}
