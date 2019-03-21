# Lagomorpha

Library to abstract the message consumption from RabbitMQ for .NET Core.

#### Do you want to know the name's origin?

The rabbit comes from the family Lagomorpha. So, like Lagomorpha is a higher
level of rabbits, this library is an abstraction for RabbitMQ. :)

#### Usage

- Include this package in your project.
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

~~~~

public class MessageHandler 
{
    [QueueHandler("NewProductQueue")]
    public void HandleNewProduct(Product p)
    {
        DoSomething(p);    
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
