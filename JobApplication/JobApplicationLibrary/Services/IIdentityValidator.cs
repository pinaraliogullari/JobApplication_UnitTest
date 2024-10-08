﻿using JobApplicationLibrary.Models;

namespace JobApplicationLibrary.Services
{
    public interface IIdentityValidator
    {
        bool IsValid(string identityNumber);
        //bool CheckConnectionToRemoteServer();

        ICountryDataProvider CountryDataProvider { get; }
        public ValidationMode ValidationMode { get; set; }
    }

    public interface ICountryData
    {
        string Country { get; }
    }

    public interface ICountryDataProvider
    {
        ICountryData CountryData { get; }
    }

    public enum ValidationMode
    {
        None,
        Quick,
        Detailed

    }
}