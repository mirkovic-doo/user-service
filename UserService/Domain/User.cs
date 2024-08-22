﻿using UserService.Domain.Base;

namespace UserService.Domain;

public class User : Entity, IEntity
{
    public User() : base() { }

    public User(
        string firstName,
        string lastName,
        string email,
        string country,
        string city,
        string postCode,
        string street,
        string houseNumber,
        bool isGuest) : base()
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Country = country;
        City = city;
        PostCode = postCode;
        Street = street;
        HouseNumber = houseNumber;
        IsGuest = isGuest;
    }

    public string FirebaseId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Country { get; set; }
    public string City { get; set; }
    public string PostCode { get; set; }
    public string Street { get; set; }
    public string HouseNumber { get; set; }
    public bool IsGuest { get; set; }
}
