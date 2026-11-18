namespace TestingLab.Core.Shop
{
    /// <summary>One row in the cart. Immutable: "changing" a line means replacing it.</summary>
    public readonly struct CartLine
    {
        public string Sku { get; }
        public int Price { get; }
        public int Quantity { get; }
        public int LineTotal => Price * Quantity;

        public CartLine(string sku, int price, int quantity)
        {
            Sku = sku;
            Price = price;
            Quantity = quantity;
        }
    }
}
