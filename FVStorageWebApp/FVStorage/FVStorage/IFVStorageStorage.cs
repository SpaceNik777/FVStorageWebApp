using FVStorage.Entities;

namespace FVStorage;

public interface IFVStorageStorage
{
    public int CountSupplies();
    public IEnumerable<Supply> ListSupplies();
    public IEnumerable<Supplier> ListSuppliers();
    public IEnumerable<Product> ListProducts();

    public Supply FindSupply(string id);
    public Product FindProduct(string code);
    public Supplier FindSupplier(string code);

    public void CreateSupply(Supply supply);
    public void UpdateSupply(Supply supply);
    public void DeleteSupply(Supply supply);

    public void UpdateProduct(Product product);
    public void CreateProduct(Product product);
    public void DeleteProduct(Product product);
    
    public void UpdateSupplier(Supplier supplier);
    public void CreateSupplier(Supplier supplier);
    public void DeleteSupplier(Supplier supplier);
    
}