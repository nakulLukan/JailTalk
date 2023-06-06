﻿using JailTalk.Domain.Lookup;

namespace JailTalk.Domain.Prison;
public class Prisoner : DomainBase
{
    public Guid Id { get; set; }
    public string Pid { get; set; }
    public string FirstName { get; set; }
    public string MiddleName { get; set; }
    public string LastName { get; set; }
    public string FullName { get; set; }
    public long? AddressId { get; set; }
    public int JailId { get; set; }

    public AddressBook Address { get; set; }
    public Jail Jail { get; set; }
    public PhoneBalance PhoneBalance { get; set; }
    public List<PhoneDirectory> PhoneDirectory { get; set; }
}