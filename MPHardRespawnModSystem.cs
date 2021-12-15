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
		public bool accidentalDC = false;
		public bool SaveAndQuitDC = false;
		public List<string> bossFighters = null;
		public List<int> bossFighters_Indexes = null;

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

		public override void Load()
		{
			bossFighters = new List<string>();
			bossFighters_Indexes = new List<int>();
		}

		public override void Unload()
		{
			bossFighters = null;
			bossFighters_Indexes = null;
		}

		public void PeriodicBossCheck() {
            //string text = "BossFighters: ";
            //foreach (string player in bossFighters)
            //{
            //    text += player + " | ";
            //}
            //ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral(text), Color.White);

            bool isBossCurrentlyActive = IsBossActive();
			if (isBossCurrentlyActive && bossFighters.Count == 0)
			{ // switched to boss
				for (int j = 0; j < 256; j++)
				{
					if (Main.player[j].active)
					{
						bossFighters.Add(Main.player[j].name);
						bossFighters_Indexes.Add(j);
					}
				}
			}

			if (!isBossCurrentlyActive) {
				// boss has ended.
				if (bossFighters.Count != 0) {
					bossFighters.Clear();
					bossFighters_Indexes.Clear();
				}
			}

			if (isBossCurrentlyActive) {  // if boss is active. check that all players. are valid, else kill them.
				// check for dced players
				foreach (int playerIndex in bossFighters_Indexes)
				{
					if (!Main.player[playerIndex].active)
					{
						int foundIndex = bossFighters_Indexes.IndexOf(playerIndex);
						bossFighters.RemoveAt(foundIndex); // he dced remove from list.
						bossFighters_Indexes.RemoveAt(foundIndex); // he dced remove from list.
					}
				}

				for (int j = 0; j < 256; j++)
				{
					// check that each player is active.
					if (Main.player[j].active)
					{
						if (!bossFighters.Contains(Main.player[j].name)) {  // if not there, kill the player.
							ModPacket myPacket = ModContent.GetInstance<MPHardRespawn>().GetPacket();
							myPacket.Write((byte)0); //kill packet
							myPacket.Send(j);
						};
					}
				}
			}
		}
		public override void PostUpdateEverything()
		{
			if (Main.netMode == NetmodeID.Server)
			{

				if (Main.GameUpdateCount % 30u == 0)
				{
					PeriodicBossCheck();
				}
			}
		}

		public void TestMethod() {
			
		}

        public static bool JustPressed(Keys key)
		{
			return Main.keyState.IsKeyDown(key) && !Main.oldKeyState.IsKeyDown(key);
		}
	}
}
