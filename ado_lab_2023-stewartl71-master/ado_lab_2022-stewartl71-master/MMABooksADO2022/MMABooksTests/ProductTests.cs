using MMABooksBusinessClasses;
using MMABooksDBClasses;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace MMABooksTests
{
    [TestFixture]
    public class ProductTests
    {
        private string _testCode = "TST-001";

        [Test, Order(1)]
        public void AddProduct_GetProduct_DeleteProduct_RoundTrip()
        {
            // Arrange
            var p = new Product
            {
                ProductCode = _testCode,
                Description = "Unit Test Product",
                OnHandQuantity = 5,
                UnitPrice = 9.99m
            };

            // Act - add
            bool added = ProductDB.AddProduct(p);
            Assert.IsTrue(added, "AddProduct failed");

            // Act - get
            var fromDb = ProductDB.GetProduct(_testCode);
            Assert.IsNotNull(fromDb);
            Assert.AreEqual(p.Description, fromDb.Description);

            // Act - delete (concurrency safe)
            bool deleted = ProductDB.DeleteProduct(fromDb);
            Assert.IsTrue(deleted, "DeleteProduct failed");

            // Verify deletion
            var shouldBeNull = ProductDB.GetProduct(_testCode);
            Assert.IsNull(shouldBeNull);
        }

        [Test, Order(2)]
        public void GetProducts_ReturnsList()
        {
            var list = ProductDB.GetProducts();
            Assert.IsNotNull(list);
            Assert.IsTrue(list.Count >= 0);
        }

        [Test, Order(3)]
        public void UpdateProduct_WhenOldMatches_Updates()
        {
            // Arrange - add a fresh record to update
            var p = new Product
            {
                ProductCode = "TST-UPD",
                Description = "Original",
                OnHandQuantity = 2,
                UnitPrice = 1.50m
            };

            Assert.IsTrue(ProductDB.AddProduct(p));
            var original = ProductDB.GetProduct(p.ProductCode);
            Assert.IsNotNull(original);

            var updated = new Product
            {
                ProductCode = original.ProductCode,
                Description = "Updated Desc",
                OnHandQuantity = 10,
                UnitPrice = 2.00m
            };

            // Act
            bool ok = ProductDB.UpdateProduct(original, updated);
            Assert.IsTrue(ok, "UpdateProduct failed");

            // Cleanup
            var fromDb = ProductDB.GetProduct(p.ProductCode);
            Assert.IsNotNull(fromDb);
            bool deleted = ProductDB.DeleteProduct(fromDb);
            Assert.IsTrue(deleted);
        }
    }
}
