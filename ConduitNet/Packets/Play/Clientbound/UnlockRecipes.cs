using Conduit.Net.Attributes;
using Conduit.Net.Data.Play;

namespace Conduit.Net.Packets.Play.Clientbound
{
    public sealed class UnlockRecipes : Packet
    {
        [VarInt] public UnlockRecipesMode Action;
        
        public bool CraftingRecipeBookOpen;
        public bool CraftingRecipeBookFilterActive;
        public bool SmeltingRecipeBookOpen;
        public bool SmeltingRecipeBookFilterActive;
        public bool BlastFurnaceRecipeBookOpen;
        public bool BlastFurnaceRecipeBookFilterActive;
        public bool SmokerRecipeBookOpen;
        public bool SmokerRecipeBookFilterActive;

        public string[] RecipeIds;
        // Only if init mode (optional)
        public string[] NewRecipeIds;
    }
}