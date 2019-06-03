# Lagomorpha

Library to abstract the message consumption from RabbitMQ for .NET Core.

The rabbit comes from the family Lagomorpha.

#### Usage

- Include this package in your project, using this command in your Package Manager Console: <br />
`Install-Package Lagomorpha`

- In your .NET Core section to configuration of services, include, passing one type of the assembly where the handlers will be located:

~~~~
public void ConfigureServices(IServiceCollection services)
{
    ...
    services.AddLagomorpha(typeof(Startup));
    services.AddMvc();
}
~~~~

- Create some class to handle your messages:
- You can also declare dependencies in the constructor, Lagomorpha uses the default Dependency Injection provider to create the class, so, your depencency must be declared in your DI provider.

~~~~

public class MessageHandler 
{
    private readonly ISomeDependency _dep;
    
    public MessageHandler(ISomeDependency dep) 
    {
        _dep = dep;
    }

    [QueueHandler("NewProductQueue")]
    public void HandleNewProduct(Product p)
    {
        _dep.DoSomething(p);    
    }
}

~~~~

- Add this handler to de Dependency Injection service:

~~~~
public void ConfigureServices(IServiceCollection services)
{
    ...
    services.AddLagomorpha(typeof(Startup));
    services.AddScoped<MessageHandler>();
    services.AddMvc();
}
~~~~

- And that's all! :D

Lagomorpha will handle new messages in the queue called **NewProductQueue**, and
cast the incoming messages to the class **Product**, the first method's parameter.
