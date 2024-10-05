using DataAccessLayer;
using DataAccessLayer.Entities;

namespace DataAccessLayerTests
{
    public class ProductDaoTests
    {
        private IDao<Product> _dao;

        [SetUp]
        public void Setup()
        {
            _dao = DaoFactory.Create<Product>(new SqlServerDataContext());
        }

        [Test]
        public void CreateProductShouldThrowExceptionTest()
        {
            Assert.Throws<NotImplementedException>(() => _dao.Create(new Product() { Name = "Test", Description = "Lorem Ipsum", Price = 4 }));
        }

        [Test]
        public void ReadAllProductsTest()
        {
            IEnumerable<Product> products = _dao.Read();
            Assert.That(products, Is.Not.Null);
        }

        [Test]
        public void UpdateProductShouldShowExceptionTest()
        {
            Assert.Throws<NotImplementedException>(() => _dao.Update(new Product() { Id = 1, Name = "Test", Description = "Lorem Ipsum", Price = 4 }));
        }

        [Test]
        public void DeleteProductShouldShowExceptionTest()
        {
            Assert.Throws<NotImplementedException>(() => _dao.Delete(new Product() { Id = 1, Name = "Test", Description = "Lorem Ipsum", Price = 4 }));

        }
    }
}