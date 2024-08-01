using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.TerrainFeatures;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace returnzork.StardewValleyMod.FlashingHarvest
{
    internal class ModEntry : StardewModdingAPI.Mod
    {
        bool isFlashingEnabled = false;

        public override void Entry(IModHelper helper)
        {
            helper.Events.Input.ButtonPressed += OnButtonPressed;
            helper.Events.Display.RenderedWorld += Display_RenderedWorld;
        }

        private void OnButtonPressed(object? sender, ButtonPressedEventArgs e)
        {
            if (!Context.IsWorldReady)
                return;

            if (e.Button == SButton.LeftStick || e.Button == SButton.O)
                isFlashingEnabled = !isFlashingEnabled;
        }

        private void DrawTileOverlay(int xTile, int yTile)
        {
            // https://wiki.stardewvalley.net/Modding:Modder_Guide/Game_Fundamentals#Positions
            Game1.DrawBox((xTile * Game1.tileSize) - Game1.viewport.X, (yTile * Game1.tileSize) - Game1.viewport.Y, Game1.tileSize, Game1.tileSize, Color.Red);
        }

        private void Display_RenderedWorld(object? sender, RenderedWorldEventArgs e)
        {
            if (!isFlashingEnabled)
                return;
            Farmer player = Game1.player;
            foreach (var feature in player.currentLocation.terrainFeatures.Values)
            {
                if (feature is HoeDirt hd && hd.crop != null && hd.readyForHarvest())
                {
                    DrawTileOverlay((int)hd.crop.tilePosition.X, (int)hd.crop.tilePosition.Y);
                }
            }
        }
    }
}
