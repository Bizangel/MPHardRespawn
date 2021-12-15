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

using System.Reflection;

namespace MPHardRespawn
{
	class MPHardRespawnModSystem : ModSystem
	{
		public Player[] activePlayers = null;
		public bool accidentalDC = false;
		public bool SaveAndQuitDC = false;
		public List<string> bossFighters = null;

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
		}

		public override void Unload()
		{
			bossFighters = null;
		}

		public void PeriodicBossCheck(){
			bool isBossCurrentlyActive = IsBossActive();
			if (isBossCurrentlyActive && bossFighters.Count == 0)
			{ // switched to boss
				for (int j = 0; j < 256; j++)
				{
					if (Main.player[j].active)
					{
						bossFighters.Add(Main.player[j].name);
					}
				}
            }

			if (!isBossCurrentlyActive) {
				// boss has ended.
				if (bossFighters.Count != 0) {
					bossFighters.Clear();
				}
			}

			if (isBossCurrentlyActive) {  // if boss is active. check that all players. are valid, else kill them.
				for (int j = 0; j < 256; j++)
				{
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

            if (JustPressed(Keys.Z))
				TestMethod();
		}

		public LocalizedText GetLocalizedTextFromLiteral(string text) {
			LocalizedText mytext = LocalizedText.Empty;
			PropertyInfo property = typeof(LocalizedText).GetProperty("Value");
			property = property.DeclaringType.GetProperty("Value");
			property.SetValue(mytext, text, BindingFlags.NonPublic | BindingFlags.Instance, null, null, null);
			return mytext;
		}

		public void TestMethod() {
			LocalizedText text = GetLocalizedTextFromLiteral("YOU DIED");
            Lang.inter[38] = mytext;
			//string test3 = LanguageManager.Instance.GetTextValue("LegacyInterface.38");
			Main.NewText(Lang.inter[38]);
        }

        public static bool JustPressed(Keys key)
		{
			return Main.keyState.IsKeyDown(key) && !Main.oldKeyState.IsKeyDown(key);
		}
	}
}
