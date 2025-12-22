// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Connections;

namespace Yarp.Kubernetes.Controller.Certificates;

internal class ServerCertificateSelector : IServerCertificateSelector
{
    private readonly Dictionary<string, (NamespacedName, X509Certificate2)> _certificates = new Dictionary<string, (NamespacedName, X509Certificate2)>();

    public void AddCertificate(string domainName, NamespacedName certificateName, X509Certificate2 certificate)
    {
        _certificates[domainName] = (certificateName, certificate);
    }

    public X509Certificate2 GetCertificate(ConnectionContext connectionContext, string domainName)
    {
        return _certificates.GetValueOrDefault(domainName).Item2;
    }

    public void RemoveCertificate(NamespacedName certificateName)
    {
        foreach (var kvp in _certificates)
        {
            if (kvp.Value.Item1 == certificateName)
            {
                _certificates.Remove(kvp.Key);
                break;
            }
        }
    }
}
