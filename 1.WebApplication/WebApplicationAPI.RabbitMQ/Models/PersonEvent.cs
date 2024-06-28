﻿using Application.Layer.Model.Base.Events;

namespace WebApplicationAPI.RabbitMQ.Models;
public class PersonEvent : IDomainEvent
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
}

public class BlogCreated : IDomainEvent
{
    public string BusinessId { get; private set; }
    public string Title { get; private set; }
    public string Description { get; private set; }
    public BlogCreated(
        string businessId, 
        string title, 
        string description)
    {
        BusinessId = businessId;
        Title = title;
        Description = description;
    }
}