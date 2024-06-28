﻿using Application.Layer.Model.Base.Commands;
using Application.Layer.Model.Base.Events;
using Extensions.MessageBus.Abstractions;
using Extensions.MessageBus.MessageInbox.Options;
using Extensions.Serializers.Abstractions;
using Microsoft.Extensions.Options;
using System.Reflection;

namespace Extensions.MessageBus.MessageInbox;
public class InboxMessageConsumer : IMessageConsumer
{
    private readonly MessageInboxOptions _messageInboxOptions;
    private readonly IEventDispatcher _eventDispatcher;
    private readonly IJsonSerializer _jsonSerializer;
    private readonly ICommandDispatcher _commandDispatcher;
    private readonly IMessageInboxItemRepository _messageInboxItemRepository;
    private readonly List<Type> _domainEventTypes = [];
    private readonly List<Type> _commandTypes = [];
    public InboxMessageConsumer(IOptions<MessageInboxOptions> messageInboxOptions,
                                IJsonSerializer jsonSerializer,
                                IMessageInboxItemRepository messageInboxItemRepository,
                                ICommandDispatcher commandDispatcher,
                                IEventDispatcher eventDispatcher)
    {
        _messageInboxOptions = messageInboxOptions.Value;
        _eventDispatcher = eventDispatcher;
        _jsonSerializer = jsonSerializer;
        _commandDispatcher = commandDispatcher;
        _messageInboxItemRepository = messageInboxItemRepository;
        Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
        _domainEventTypes.AddRange(assemblies.SelectMany(assembly => assembly.GetTypes().Where(c => c.IsAssignableTo(typeof(IDomainEvent)) && c.IsClass).ToList()).ToList());
        _commandTypes.AddRange(assemblies.SelectMany(assembly => assembly.GetTypes().Where(c => c.IsAssignableTo(typeof(ICommand)) && c.IsClass).ToList()).ToList());
    }

    public Task<bool> ConsumeCommand(string sender, Parcel parcel)
    {
        throw new NotImplementedException();
        //if (_messageInboxItemRepository.AllowReceive(parcel.MessageId, sender))
        //{
        //    var mapToClass = _messageTypeMap[parcel.Route];
        //    var commandType = Type.GetType(mapToClass);
        //    dynamic command = _jsonSerializer.Deserialize(parcel.MessageBody, commandType);
        //    _commandDispatcher.Send(command);
        //    _messageInboxItemRepository.Receive(parcel.MessageId, sender);
        //}    }

    }
    public async Task<bool> ConsumeEvent(string sender, Parcel parcel)
    {
        if (_messageInboxItemRepository.AllowReceive(parcel.MessageId, sender))
        {
            var eventType = _domainEventTypes.FirstOrDefault(c => c.Name == parcel.MessageName);
            if (eventType != null)
            {
                dynamic @event = _jsonSerializer.Deserialize(parcel.MessageBody, eventType);
                await _eventDispatcher.PublishDomainEventAsync(@event);
                _messageInboxItemRepository.Receive(parcel.MessageId, sender, parcel.MessageBody);
            }
        }
        return true;
    }
}