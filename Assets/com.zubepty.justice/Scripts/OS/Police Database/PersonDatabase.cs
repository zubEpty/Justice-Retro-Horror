using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PersonDatabase", menuName = "Database/Person Database")]
public class PersonDatabase : ScriptableObject
{
    public List<PersonProfile> people;

    public PersonProfile FindProfileByNameAndIdentifier(string name, string identifier)
    {
        foreach (var person in people)
        {
            if (person.fullName.Equals(name, StringComparison.OrdinalIgnoreCase) &&
                (
                    person.nationalID == identifier ||
                    person.dateOfBirth == identifier ||
                    person.licenseNumber == identifier
                ))
            {
                return person;
            }
        }
        return null;
    }

}