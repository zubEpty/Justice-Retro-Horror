using UnityEngine;
using System;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewPersonProfile", menuName = "Database/Person Profile")]
public class PersonProfile : ScriptableObject
{
    public string fullName;
    public string nationalID;
    public string dateOfBirth;
    public string contactNo;
    public string licenseNumber;
    public Sprite photo;
    public List<string> criminalRecords; // empty = no crimes


    public bool HasCriminalRecord => criminalRecords != null && criminalRecords.Count > 0;
}