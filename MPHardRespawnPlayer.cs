using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Input;
using Terraria.ID;
using Terraria.Localization;
using Microsoft.Xna.Framework;
using Terraria.UI;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria.DataStructures;
using Terraria.Chat;
using Terraria.Audio;

using System.Reflection;
namespace MPHardRespawn
{
	class MPHardRespawnPlayer : ModPlayer
	{
        public int bossDeathState = -1; // -1 is Not dead.
        public int prevDeathState = -1; // prev to check for changes
        public int switchStateCounter = 1;
        // 0 is initial dead during boss fight (YOU DIED).
        // 1 is dead during boss fight (No display)
        // 2 is Respawning... screen

        public LocalizedText GetLocalizedTextFromLiteral(string text)
        {
            LocalizedText mytext = Language.GetText("thrash.nonexistent.key");
            PropertyInfo property = typeof(LocalizedText).GetProperty("Value");
            property = property.DeclaringType.GetProperty("Value");
            property.SetValue(mytext, text, BindingFlags.NonPublic | BindingFlags.Instance, null, null, null);
            return mytext;
        }

        public override void UpdateDead()
        {

            if (bossDeathState == 0) {
                switchStateCounter--; // tick counter
            }
            

            if (Main.GameUpdateCount % 59u == 0) // Every half of a second check boss active or not status.
            {
                if (MPHardRespawnModSystem.IsBossActive())
                {
                    Main.player[Main.myPlayer].respawnTimer = 119;
                    Main.player[Main.myPlayer].respawnTimer += 60; // don't let it tick down
                    if (Main.player[Main.myPlayer].respawnTimer < 60)
                    {
                        Main.player[Main.myPlayer].respawnTimer = 119;
                    }
                }
                else {
                    // no longer active boss, display respawning screen
                    if (bossDeathState != -1) {
                        bossDeathState = 2;
                    }
                    
                }
            }

            if (switchStateCounter <= 0)
            {
                bossDeathState++; // switch to next state
            }

            if (prevDeathState != bossDeathState) { 
                prevDeathState = bossDeathState;
                switch (bossDeathState)
                {
                    case -1:
                        Lang.inter[38] = Language.GetText("LegacyInterface.38");
                        break;
                    case 0:
                        Lang.inter[38] = GetLocalizedTextFromLiteral("YOU DIED");
                        switchStateCounter = 180; // display for 3 secs 
                        break;
                    case 1:
                        Lang.inter[38] = GetLocalizedTextFromLiteral("");
                        Main.player[Main.myPlayer].lostCoins = 0; // hide coin display
                        switchStateCounter = 1; // display endlessly until changes.
                        break;
                    case 2:
                        Lang.inter[38] = GetLocalizedTextFromLiteral("Respawning...");
                        switchStateCounter = 1; // display endlessly until changes.
                        break;
                }
            } 
        }

        public override void OnRespawn(Player player)
        {
            bossDeathState = -1;
            prevDeathState = -1;
            switchStateCounter = 10;
            Lang.inter[38] = Language.GetText("LegacyInterface.38"); // restore original death message.
        }

        public override void Kill(double damage, int hitDirection, bool pvp, PlayerDeathReason damageSource)
        {
            if (MPHardRespawnModSystem.IsBossActive() && Main.netMode == NetmodeID.MultiplayerClient) {
                bossDeathState = 0;
                switchStateCounter = 10;
            }
        }
    }
}
