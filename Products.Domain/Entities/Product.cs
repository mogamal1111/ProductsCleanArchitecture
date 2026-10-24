namespace Products.Domain.Entities;

public sealed class Product
{
    // Required by EF Core to materialize Product from the database.
    private Product() { }

    public Product(string name, decimal price)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Product name is required.", nameof(name));

        var trimmedName = name.Trim();
        if (trimmedName.Length is < 2 or > 100)
            throw new ArgumentException("Product name must have 2 to 100 characters.", nameof(name));

        if (price is < 0.01m or > 999999999.99m)
            throw new ArgumentOutOfRangeException(nameof(price), "Price must be between 0.01 and 999999999.99.");

        Name = trimmedName;
        Price = price;
    }

    public int Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public decimal Price { get; private set; }
}
