using Conduit.Net.Attributes;

namespace Conduit.Net.Packets.Play
{
    public class UnlockRecipes : Packet
    {
        [VarInt] public int Action;
        
        public bool CraftingRecipeOpen;
        public bool CraftingRecipeFilterActive;
        public bool SmeltingRecipeOpen;
        public bool SmeltingRecipeFilterActive;
        public bool BlastRecipeOpen;
        public bool BlastRecipeFilterActive;
        public bool SmokerRecipeOpen;
        public bool SmokerRecipeFilterActive;

        public string[] RecipeIds1;
        public string[] RecipesIds2;

        public UnlockRecipes() => Id = 0x39;
    }
}