namespace Lagomorpha
{
    public class FooBarHandler : RabbitQueueWorker
    {
        [QueueHandler("ProductQueue")]
        public void Handle(Product product)
        {

        }

        [QueueHandler("SellQueue")]
        public void Handle(Sell sell)
        {

        }
    }

    public class Sell
    {
    }

    public class Product
    {
    }
}
