using Framework.Basesource.Extensions;
using Framework.Basesource.RabbitMQServices;

const string RoutingKey = "MessageSystem";
const string QueueName = "MessageSystem";

MessageSystem service = new(ApplicationType.Receiver,RoutingKey,QueueName);
await service.RunAsync();