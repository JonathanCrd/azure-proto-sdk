﻿using Azure.ResourceManager.Network;
using Azure.ResourceManager.Network.Models;
using azure_proto_core;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace azure_proto_network
{
    public class PublicIpAddressCollection : AzureCollection<AzurePublicIpAddress>
    {
        public PublicIpAddressCollection(TrackedResource resourceGroup) : base(resourceGroup) { }

        private NetworkManagementClient Client => ClientFactory.Instance.GetNetworkClient(Parent.Id.Subscription);

        public AzurePublicIpAddress CreateOrUpdatePublicIpAddress(string name, AzurePublicIpAddress ipAddress)
        {
            var ipResult = Client.PublicIPAddresses.StartCreateOrUpdate(Parent.Name, name, ipAddress.Model).WaitForCompletionAsync().Result;
            ipAddress = new AzurePublicIpAddress(Parent, new PhPublicIPAddress(ipResult.Value));
            return ipAddress;
        }

        public async Task<AzurePublicIpAddress> CreateOrUpdatePublicIpAddressAsync(string name, AzurePublicIpAddress ipAddress, CancellationToken cancellationToken = default)
        {
            var ipResult = await Client.PublicIPAddresses.StartCreateOrUpdateAsync(Parent.Name, name, ipAddress.Model, cancellationToken);
            ipAddress = new AzurePublicIpAddress(Parent, new PhPublicIPAddress(ipResult.Value));
            return ipAddress;
        }

        protected override IEnumerable<AzurePublicIpAddress> GetItems()
        {
            throw new NotImplementedException();
        }

        protected override AzurePublicIpAddress Get(string ipName)
        {
            var ipResult = Client.PublicIPAddresses.Get(Parent.Name, ipName);
            return new AzurePublicIpAddress(Parent, new PhPublicIPAddress(ipResult.Value));
        }

        public AzurePublicIpAddress ConstructIPAddress()
        {
            var ipAddress = new PublicIPAddress()
            {
                PublicIPAddressVersion = Azure.ResourceManager.Network.Models.IPVersion.IPv4.ToString(),
                PublicIPAllocationMethod = IPAllocationMethod.Dynamic,
                Location = Parent.Location,
            };
            return new AzurePublicIpAddress(Parent, new PhPublicIPAddress(ipAddress));
        }
    }
}
