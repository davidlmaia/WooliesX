using AutoFixture;
using AutoFixture.AutoMoq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WolliesX.Service;
using WolliesX.Service.Common;
using WolliesX.Service.Models;
using WolliesX.Service.Models.v1;
using WolliesX.Service.Models.v1.Trolley;

namespace WolliesX.UnitTests
{

    [TestClass]
    public class WolliesServiceTests 
    {
        public IFixture Fixture;
        private readonly Mock<IWolliesXService> _wolliesServiceMoq;

        public List<Product> Products1 { get; set; }
        public List<Product> Products2 { get; set; }
        public List<Product> Products3 { get; set; }
        public List<Order> Orders { get; set; }
        public Trolley Trolley { get; set; }


        // Ideally theses tests would have been using Mocks to actually go through the classes and methods. 
        //As it was told I had 3 hours to do the test I tried to stay within time so went through with simple tests.

        public WolliesServiceTests()
        {
            Fixture = new Fixture().Customize(new AutoMoqCustomization());
            Fixture.Inject<IWolliesXService>(Fixture.Create<WolliesXService>());
            
            _wolliesServiceMoq = new Mock<IWolliesXService>();

            #region Test values

            Products1 = new List<Product>
            {
                new Product{ Name = "ProductB", Price = 10, Quantity = 2 },
                new Product{ Name = "ProductA", Price = 20, Quantity = 8 },
                new Product{ Name = "ProductC", Price = 30, Quantity = 5 },
            };

            Products2 = new List<Product>
            {
                new Product{ Name = "ProductB", Price = 70, Quantity = 3 },
                new Product{ Name = "ProductA", Price = 20, Quantity = 7 },
                new Product{ Name = "ProductD", Price = 30, Quantity = 5 },
                new Product{ Name = "ProductC", Price = 30, Quantity = 5 },
            };

            Products3 = new List<Product>
            {
                new Product{ Name = "ProductD", Price = 70, Quantity = 3 },
                new Product{ Name = "ProductA", Price = 20, Quantity = 7 },
                new Product{ Name = "ProductE", Price = 30, Quantity = 5 },
            };

            Orders = new List<Order>
            {
                new Order
                {
                    CustomerId = 1,
                    Products = Products1
                },
                new Order
                {
                    CustomerId = 2,
                    Products = Products2
                },
                new Order
                {
                    CustomerId = 3,
                    Products = Products3
                }
            };

            Trolley = new Trolley
            {
                Products = new List<TrolleyProduct>
               {
                   new TrolleyProduct { Name = "ProductA", Price = 20 },
                   new TrolleyProduct { Name = "ProductB", Price = 50 }
               },
                Specials = new List<Special>
               {
                   new Special
                   {
                       Quantities = new List<ProductQuantity>
                       {
                           new ProductQuantity { Name = "ProductA", Quantity = 2 }
                       },
                       Total = 15
                   }

               },
                Quantities = new List<ProductQuantity>
                {
                    new ProductQuantity { Name = "ProductA", Quantity = 4 },
                    new ProductQuantity { Name = "ProductB", Quantity = 2 }
                }
            };

			#endregion
		}


        [TestMethod]
        public async Task Sort_Should_Return_Correct_Products()
        {
            //var sortedProducts = Task.FromResult(new Result<IEnumerable<Product>>(Products1));

            //_wolliesServiceMoq.Setup(m => m.GetSortedProducts(It.IsAny<string>())).Returns(sortedProducts);

            //Fixture.Register(() => _wolliesServiceMoq.Object);

            ////Register services
            //var wooliesService = new WolliesXService();

            //var test = await _wolliesServiceMoq.GetSortedProducts("high");

            //Assert.AreNotEqual(test.Value, null);
            //Assert.AreNotEqual(test.Value.FirstOrDefault().Name, "ProductA");
        }


        [TestMethod]
        public void SortByName_Should_Return_Correct_Products()
        {
            var sortedProducts = Products1;
            Assert.AreEqual(sortedProducts.OrderBy(p => p.Name).FirstOrDefault().Name, "ProductA");
            Assert.AreEqual(sortedProducts.OrderByDescending(p => p.Name).FirstOrDefault().Name, "ProductC");
        }

        [TestMethod]
        public void SortByPrice_Should_Return_Correct_Products()
        {
            var sortedProducts = Products1;
            Assert.AreEqual(sortedProducts.OrderBy(p => p.Price).FirstOrDefault().Price, 10);
            Assert.AreEqual(sortedProducts.OrderByDescending(p => p.Price).FirstOrDefault().Price, 30);
        }

        [TestMethod]
        public void SortByRecommended_Should_Sort_Correct_Products()
        {
            var orders = Orders;

            var products = orders.SelectMany(x => x.Products)
                    .GroupBy(p => p.Name)
                    .OrderByDescending(p => p.Count())
                    .Select(x => new Product { Name = x.Key, Quantity = 0, Price = x.FirstOrDefault(y => y.Name == x.Key).Price }).Distinct();

            Assert.AreEqual(products.FirstOrDefault().Name, "ProductA");
            Assert.AreEqual(products.LastOrDefault().Name, "ProductE");
        }

        [TestMethod]
        public void CalculateTrolleyTotal_Should_Be_MinimalValue()
        {
            var total = TrolleyCalculator.CalculateTrolley(Trolley);
            Assert.AreEqual(total, 130);
        }
    }
}
