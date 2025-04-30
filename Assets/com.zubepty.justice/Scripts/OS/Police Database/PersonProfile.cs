using UnityEngine;
using System;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewPersonProfile", menuName = "Database/Person Profile")]
public class PersonProfile : ScriptableObject
{
    public string fullName;
    public string nationalID;
    public string dateOfBirth; // or use DateTime if not displaying in inspector
    public string licenseNumber;
    public Sprite photo;
    public List<string> criminalRecords; // empty = no crimes

    public bool HasCriminalRecord => criminalRecords != null && criminalRecords.Count > 0;
}