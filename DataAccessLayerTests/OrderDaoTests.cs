using DataAccessLayer;
using DataAccessLayer.Entities;
using NUnit.Framework.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayerTests
{
    internal class OrderDaoTests
    {
        IDao<Order> _dao;

        [SetUp]
        public void Setup()
        {
            _dao = DaoFactory.Create<Order>(new SqlServerDataContext());
        }

        [Test]
        public void CreateOrderSuccessTest()
        {
            Order order = _dao.Create(new()
            {
                CustomerName = "Hans",
                Status = "New",
                Discount = 0,
                Orderlines = [new() { Item = new Product() { Id = 1, Name = "Americano", Description = "Espresso shots topped with hot water create a light layer of crema culminating in this wonderfully rich cup with depth and nuance." }, Quantity = 2 }]
            });

            Assert.That(order, Is.Not.Null);
        }

        [Test]
        public void CreateOrderFailTest()
        {
            Assert.Throws<DaoException>(() =>
            {
                Order order = _dao.Create(new()
                {
                    CustomerName = "Bound To Fail",
                    Status = "New",
                    Discount = 0,
                    Orderlines = [new() { Item = new Product() { Id = 20, Name = "Imaginary", Description = "This beverage don't exist" }, Quantity = 2 }]
                });
            });
        }

        [Test]
        public void ReadOrdersSuccessTest()
        {
            var orders = _dao.Read();

            Assert.That(orders, Is.Not.Null);
            Assert.That(orders.Any(), Is.True);

            foreach (var order in orders)
            {
                Assert.That(order.Orderlines, Is.Not.Null);
                Assert.That(order.Orderlines.Any(), Is.True);

                foreach (Orderline orderline in order.Orderlines)
                {
                    Assert.That(orderline.Item, Is.Not.Null);
                }
            }
        }

        [Test]
        public void UpdateOrderStatusSuccessTest()
        {
            Order test = _dao.Read().First();
            int id = test.Id;

            test.Status = "Finished";

            bool result = _dao.Update(test);

            Assert.That(result, Is.True);
        }

        [Test]
        public void DeleteShouldThrowNotImplementedException()
        {
            Assert.Throws<NotImplementedException>(() => _dao.Delete(new Order() { CustomerName = "", Status = "" }));
        }
    }
}
