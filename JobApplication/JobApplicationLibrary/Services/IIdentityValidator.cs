﻿namespace JobApplicationLibrary.Services
{
    public interface IIdentityValidator
    {
        bool IsValid(string identityNumber);
        //bool CheckConnectionToRemoteServer();

        string Country { get; }
    }
}