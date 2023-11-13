using System.Reflection;
using FVStorage.Entities;
using static System.Int32;
using Microsoft.Extensions.Logging;

namespace FVStorage;

public class FvStorageStorageCsvFileStorageStorage : IFVStorageStorage {
        private static readonly IEqualityComparer<string> collation = StringComparer.OrdinalIgnoreCase;

        private readonly Dictionary<string, Supplier> suppliers = new Dictionary<string, Supplier>(collation);
        private readonly Dictionary<string, Product> products = new Dictionary<string, Product>(collation);
        private readonly Dictionary<string, Supply> supplies = new Dictionary<string, Supply>(collation);
        private readonly ILogger<FvStorageStorageCsvFileStorageStorage> logger;

        public FvStorageStorageCsvFileStorageStorage(ILogger<FvStorageStorageCsvFileStorageStorage> logger) {
            this.logger = logger;
            ReadSuppliersFromCsvFile("suppliers.csv");
            ReadProductsFromCsvFile("products.csv");
            ReadSuppliesFromCsvFile("supplies.csv");
            ResolveReferences();
        }

        private void ResolveReferences() {
            foreach (var supplier in suppliers.Values) {
                supplier.Products = products.Values.Where(m => m.SupplierCode == supplier.Code).ToList();
                foreach (var product in supplier.Products) product.Supplier = supplier;
            }

            foreach (var product in products.Values) {
                product.Supplies = supplies.Values.Where(v => v.ProductCode == product.Code).ToList();
                foreach (var supply in product.Supplies) supply.SupplyProduct = product;
            }
        }

        private string ResolveCsvFilePath(string filename) {
            var directory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var csvFilePath = "csv-data";
            return Path.Combine(csvFilePath, filename);
        }

        private void ReadSuppliesFromCsvFile(string filename) {
            var filePath = ResolveCsvFilePath(filename);
            foreach (var line in File.ReadAllLines(filePath)) {
                var tokens = line.Split(",");
                var supply = new Supply {
                    Id = tokens[0],
                    ProductCode = tokens[1],
                    Amount = Convert.ToInt32(tokens[2]),
                    Date = Convert.ToDateTime(tokens[3])
                };
                if (TryParse(tokens[3], out var date)) supply.Date = new DateTime(date);
                supplies[supply.Id] = supply;
            }
            logger.LogInformation($"Loaded {supplies.Count} models from {filePath}");
        }

        private void ReadProductsFromCsvFile(string filename) {
            var filePath = ResolveCsvFilePath(filename);
            foreach (var line in File.ReadAllLines(filePath)) {
                var tokens = line.Split(",");
                var product = new Product {
                    Code = tokens[0],
                    Name = tokens[1],
                    Weight = tokens[2],
                    Price = tokens[3],
                    SupplierCode = tokens[4]
                    
                };
                products.Add(product.Code, product);
            }
            logger.LogInformation($"Loaded {products.Count} models from {filePath}");
        }

        private void ReadSuppliersFromCsvFile(string filename) {
            var filePath = ResolveCsvFilePath(filename);
            foreach (var line in File.ReadAllLines(filePath)) {
                var tokens = line.Split(",");
                var supplier = new Supplier {
                    Code = tokens[0],
                    Name = tokens[1]
                };
                suppliers.Add(supplier.Code, supplier);
            }
            logger.LogInformation($"Loaded {suppliers.Count} manufacturers from {filePath}");
        }

        public int CountSupplies() => supplies.Count;

        public IEnumerable<Supply> ListSupplies() => supplies.Values;

        public IEnumerable<Supplier> ListSuppliers() => suppliers.Values;

        public IEnumerable<Product> ListProducts() => products.Values;

        public Supply FindSupply(string id) => supplies.GetValueOrDefault(id);

        public Product FindProduct(string code) => products.GetValueOrDefault(code);

        public Supplier FindSupplier(string code) => suppliers.GetValueOrDefault(code);

        public void CreateSupply(Supply supply) {
            supply.SupplyProduct.Supplies.Add(supply);
            supply.ProductCode = supply.SupplyProduct.Code;
            UpdateSupply(supply);
        }

        public void UpdateSupply(Supply supply) {
            supplies[supply.Id] = supply;
        }

        public void DeleteSupply(Supply supply) {
            var product = FindProduct(supply.ProductCode);
            product.Supplies.Remove(supply);
            supplies.Remove(supply.Id);
        }
        public void CreateProduct(Product product) {
            product.Supplier = FindSupplier(product.SupplierCode);
            foreach (Supply supply in supplies.Values)
            {
                if (supply.ProductCode == product.Code)
                {
                    product.Supplies.Add(supply);
                }
            }
            UpdateProduct(product);
        }
        public void UpdateProduct(Product product) {
            products[product.Code] = product;
        }
        public void DeleteProduct(Product product) {
            var suplier = FindSupplier(product.SupplierCode);
            suplier.Products.Remove(product);
            products.Remove(product.Code);
        }
        public void UpdateSupplier(Supplier supplier) {
            suppliers[supplier.Code] = supplier;
        }
        
        public void CreateSupplier(Supplier supplier) {
            foreach (Product product in products.Values)
            {
                if (product.SupplierCode == supplier.Code)
                {
                    supplier.Products.Add(product);
                }
            }
            suppliers[supplier.Code] = supplier;
        }
        public void DeleteSupplier(Supplier supplier) {
            foreach (Product product in products.Values)
            {
                if (product.SupplierCode == supplier.Code)
                {
                    DeleteProduct(product);
                }
            }
            suppliers.Remove(supplier.Code);
        }
        
    }