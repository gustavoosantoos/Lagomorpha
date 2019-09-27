using Lagomorpha;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace LagomorphaTests.Handlers
{
    public class ProductsHandler
    {
        [QueueHandler("new-products", ResponseQueue = "new-product-response")]
        public async Task<object> HandleNewProduct(NewProduct p)
        {
            await Task.Delay(2000);
            Console.WriteLine(p.Name);
            return new
            {
                FullName = $"Teste: {p.Name} inserido com sucesso"
            };
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
