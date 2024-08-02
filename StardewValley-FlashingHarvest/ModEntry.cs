using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.TerrainFeatures;

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
            //ignore if no world is loaded yet
            if (!Context.IsWorldReady)
                return;

            //TODO currently set to toggle when pressing the Left Stick of controller in, or the O button on the keyboard is pressed
            if (e.Button == SButton.LeftStick || e.Button == SButton.O)
                isFlashingEnabled = !isFlashingEnabled;
        }

        /// <summary>
        /// Draw a box over the top of a specified tile
        /// See https://wiki.stardewvalley.net/Modding:Modder_Guide/Game_Fundamentals#Positions
        /// </summary>
        /// <param name="xTile">X Tile in world position we should draw over</param>
        /// <param name="yTile">Y Tile in world position that we should draw over</param>
        private void DrawTileOverlay(int xTile, int yTile) => Game1.DrawBox((xTile * Game1.tileSize) - Game1.viewport.X, (yTile * Game1.tileSize) - Game1.viewport.Y, Game1.tileSize, Game1.tileSize, Color.Red);

        private void Display_RenderedWorld(object? sender, RenderedWorldEventArgs e)
        {
            //don't draw if not enabled
            if (!isFlashingEnabled)
                return;

            Farmer player = Game1.player;
            //iterate over each terrain feature in the player's current location
            foreach (var feature in player.currentLocation.terrainFeatures.Values)
            {
                //check if the crop is ready for harvest
                if (feature is HoeDirt hd && hd.crop != null && hd.readyForHarvest())
                {
                    DrawTileOverlay((int)hd.crop.tilePosition.X, (int)hd.crop.tilePosition.Y);
                }
            }
        }
    }
}
