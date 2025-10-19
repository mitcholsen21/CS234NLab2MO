using NUnit.Framework;
using MMABooksBusinessClasses;
using MMABooksDBClasses;

namespace MMABooksTests
{
    [TestFixture]
    public class CustomerTests
    {
        [Test]
        public void CanCreateCustomer_DefaultConstructor()
        {
            var c = new Customer();
            Assert.NotNull(c);
        }

        [Test]
        public void CanSetProperties_WithValidation()
        {
            var c = new Customer
            {
                Name = "John Smith",
                Address = "123 Main St",
                City = "Eugene",
                State = "OR",
                ZipCode = "97401"
            };
            Assert.AreEqual("John Smith", c.Name);
        }

        [Test]
        public void AddGetDeleteCustomer_RoundTrip()
        {
            var c = new Customer
            {
                Name = "Unit Test",
                Address = "456 Pine St",
                City = "Portland",
                State = "OR",
                ZipCode = "97035"
            };

            int id = CustomerDB.AddCustomer(c);
            Assert.Greater(id, 0);

            var fromDb = CustomerDB.GetCustomer(id);
            Assert.AreEqual(c.Name, fromDb.Name);

            bool deleted = CustomerDB.DeleteCustomer(fromDb);
            Assert.IsTrue(deleted);
        }
    }
}
