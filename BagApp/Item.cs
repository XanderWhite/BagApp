namespace BagApp
{
    struct Item
    {
        public Item(int weight, int cost)
        {
            Weight = weight;
            Cost = cost;
        }

        public int Weight { get; set; }
        public int Cost { get; set; }
    }
}
