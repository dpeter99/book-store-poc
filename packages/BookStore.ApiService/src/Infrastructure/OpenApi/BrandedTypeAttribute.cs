namespace BookStore.ApiService;

public class BrandedTypeAttribute(string brand) : Attribute
{
	public string Brand { get => brand; }
}
