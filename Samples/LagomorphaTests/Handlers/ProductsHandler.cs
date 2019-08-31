using Lagomorpha;
using System;
using System.Diagnostics;

namespace LagomorphaTests.Handlers
{
    public class ProductsHandler
    {
        [QueueHandler("new-products")]
        public void HandleNewProduct(NewProduct p)
        {

        }

        [QueueHandler("remove-products")]
        public void HandleRemoveProduct(Guid codigo)
        {
            Debug.WriteLine("=================== MENSAGEM RECEBIDA =================");
            Debug.WriteLine(codigo);
            Debug.WriteLine("");
            Debug.WriteLine("");
        }
    }

    public class NewProduct
    {
        public string Name { get; set; }
    }
}
