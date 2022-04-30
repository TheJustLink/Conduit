using Conduit.Net.Attributes;

namespace Conduit.Net.Packets.Play
{
    public class UnlockRecipes : Packet
    {
        [VarInt]
        public int Action;//value between 0 and 2
        public bool CraftingRecipeOpen;
        public bool CraftingRecipeFilterActive;
        public bool SmeltingRecipeOpen;
        public bool SmeltingRecipeFilterActive;
        public bool BlastRecipeOpen;
        public bool BlastRecipeFilterActive;
        public bool SmokerRecipeOpen;
        public bool SmokerRecipeFilterActive;
        [VarInt]
        public int ArraySize1;
        public string[] RecipeIds1;
        [VarInt]
        public int ArraySize2;
        public string[] RecipesIds2;

        public UnlockRecipes() => Id = 0x39;
    }
}
