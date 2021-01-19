﻿using Azure;
using Azure.ResourceManager.Network;
using Azure.ResourceManager.Network.Models;
using Azure.ResourceManager.Core;
using Azure.ResourceManager.Core.Adapters;
using Azure.ResourceManager.Core.Resources;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System;

namespace azure_proto_network
{
    /// <summary>
    /// A class representing collection of <see cref="NetworkInterface"/> and their operations over a <see cref="ResourceGroup"/>.
    /// </summary>
    public class NetworkInterfaceContainer : ResourceContainerBase<NetworkInterface, NetworkInterfaceData>
    {
        internal NetworkInterfaceContainer(AzureResourceManagerClientOptions options, ResourceGroupData resourceGroup) : base(options, resourceGroup) { }

        internal NetworkInterfaceContainer(AzureResourceManagerClientOptions options, ResourceIdentifier id) : base(options, id) { }

        internal NetworkInterfacesOperations Operations => GetClient<NetworkManagementClient>((uri, cred) => new NetworkManagementClient(Id.Subscription, uri, cred, 
            ClientOptions.Convert<NetworkManagementClientOptions>())).NetworkInterfaces;

        /// <inheritdoc/>
        protected override ResourceType ValidResourceType => ResourceGroupOperations.ResourceType;

        /// <inheritdoc/>
        public override ArmResponse<NetworkInterface> Create(string name, NetworkInterfaceData resourceDetails)
        {
            var operation = Operations.StartCreateOrUpdate(Id.ResourceGroup, name, resourceDetails);
            return new PhArmResponse<NetworkInterface, Azure.ResourceManager.Network.Models.NetworkInterface>(
                operation.WaitForCompletionAsync().ConfigureAwait(false).GetAwaiter().GetResult(),
                n => new NetworkInterface(ClientOptions, new NetworkInterfaceData(n)));
        }

        /// <inheritdoc/>
        public async override Task<ArmResponse<NetworkInterface>> CreateAsync(string name, NetworkInterfaceData resourceDetails, CancellationToken cancellationToken = default)
        {
            var operation = await Operations.StartCreateOrUpdateAsync(Id.ResourceGroup, name, resourceDetails, cancellationToken).ConfigureAwait(false);
            return new PhArmResponse<NetworkInterface, Azure.ResourceManager.Network.Models.NetworkInterface>(
                await operation.WaitForCompletionAsync(cancellationToken).ConfigureAwait(false),
                n => new NetworkInterface(ClientOptions, new NetworkInterfaceData(n)));
        }

        /// <inheritdoc/>
        public override ArmOperation<NetworkInterface> StartCreate(string name, NetworkInterfaceData resourceDetails, CancellationToken cancellationToken = default)
        {
            return new PhArmOperation<NetworkInterface, Azure.ResourceManager.Network.Models.NetworkInterface>(
                Operations.StartCreateOrUpdate(Id.ResourceGroup, name, resourceDetails, cancellationToken),
                n => new NetworkInterface(ClientOptions, new NetworkInterfaceData(n)));
        }

        /// <inheritdoc/>
        public async override Task<ArmOperation<NetworkInterface>> StartCreateAsync(string name, NetworkInterfaceData resourceDetails, CancellationToken cancellationToken = default)
        {
            return new PhArmOperation<NetworkInterface, Azure.ResourceManager.Network.Models.NetworkInterface>(
                await Operations.StartCreateOrUpdateAsync(Id.ResourceGroup, name, resourceDetails, cancellationToken).ConfigureAwait(false),
                n => new NetworkInterface(ClientOptions, new NetworkInterfaceData(n)));
        }

        /// <summary>
        /// Constructs an object used to create a <see cref="NetworkInterface"/>.
        /// </summary>
        /// <param name="ip"> The public IP address of the <see cref="NetworkInterface"/>. </param>
        /// <param name="subnetId"> The resource identifier of the subnet attached to this <see cref="NetworkInterface"/>. </param>
        /// <param name="location"> The <see cref="Location"/> that will contain the <see cref="NetworkInterface"/>. </param>
        /// <returns>An object used to create a <see cref="NetworkInterface"/>. </returns>
        public ArmBuilder<NetworkInterface, NetworkInterfaceData> Construct(PublicIPAddressData ip, string subnetId, Location location = null)
        {
            var nic = new Azure.ResourceManager.Network.Models.NetworkInterface()
            {
                Location = location ?? DefaultLocation,
                IpConfigurations = new List<NetworkInterfaceIPConfiguration>()
                {
                    new NetworkInterfaceIPConfiguration()
                    {
                        Name = "Primary",
                        Primary = true,
                        Subnet = new Azure.ResourceManager.Network.Models.Subnet() { Id = subnetId },
                        PrivateIPAllocationMethod = IPAllocationMethod.Dynamic,
                        PublicIPAddress = new PublicIPAddress() { Id = ip.Id }
                    }
                }
            };

            return new ArmBuilder<NetworkInterface, NetworkInterfaceData>(this, new NetworkInterfaceData(nic));
        }

        /// <summary>
        /// Lists the <see cref="NetworkInterface"/> for this <see cref="ResourceGroup"/>.
        /// </summary>
        /// <param name="cancellationToken"> A token to allow the caller to cancel the call to the service. 
        /// The default value is <see cref=System.Threading.CancellationToken.None" />. </param>
        /// <returns> A collection of <see cref="NetworkInterface"/> that may take multiple service requests to iterate over. </returns>
        public Pageable<NetworkInterfaceOperations> List(CancellationToken cancellationToken = default)
        {
            return new PhWrappingPageable<Azure.ResourceManager.Network.Models.NetworkInterface, NetworkInterfaceOperations>(
                Operations.List(Id.Name, cancellationToken),
                this.convertor());
        }

        /// <summary>
        /// Lists the <see cref="NetworkInterface"/> for this <see cref="ResourceGroup"/>.
        /// </summary>
        /// <param name="cancellationToken"> A token to allow the caller to cancel the call to the service. 
        /// The default value is <see cref=System.Threading.CancellationToken.None" />. </param>
        /// <returns> An async collection of <see cref="NetworkInterface"/> that may take multiple service requests to iterate over. </returns>
        public AsyncPageable<NetworkInterfaceOperations> ListAsync(CancellationToken cancellationToken = default)
        {
            var result = Operations.ListAsync(Id.Name, cancellationToken);
            return new PhWrappingAsyncPageable<Azure.ResourceManager.Network.Models.NetworkInterface, NetworkInterfaceOperations>(
                result,
                this.convertor());
        }

        /// <summary>
        /// Filters the list of <see cref="NetworkInterface"/> resources for this <see cref="ResourceGroup"/> represented as generic resources.
        /// <param name="filter"> A string to filter the <see cref="NetworkInterface"/> resources by name. </param>
        /// <param name="top"> The number of results to return per page of data. </param>
        /// <param name="cancellationToken"> A token to allow the caller to cancel the call to the service. 
        /// The default value is <see cref=System.Threading.CancellationToken.None" />. </param>
        /// <returns> A collection of resource operations that may take multiple service requests to iterate over. </returns>
        public Pageable<ArmResource> ListByName(ArmSubstringFilter filter, int? top = null, CancellationToken cancellationToken = default)
        {
            ArmFilterCollection filters = new ArmFilterCollection(NetworkInterfaceData.ResourceType);
            filters.SubstringFilter = filter;
            return ResourceListOperations.ListAtContext(ClientOptions, Id, filters, top, cancellationToken);
        }

        /// <summary>
        /// Filters the list of <see cref="NetworkInterface"/> resources for this <see cref="ResourceGroup"/> represented as generic resources.
        /// <param name="filter"> A string to filter the <see cref="NetworkInterface"/> resources by name. </param>
        /// <param name="top"> The number of results to return per page of data. </param>
        /// <param name="cancellationToken"> A token to allow the caller to cancel the call to the service. 
        /// The default value is <see cref=System.Threading.CancellationToken.None" />. </param>
        /// <returns> An async collection of resource operations that may take multiple service requests to iterate over. </returns>
        public AsyncPageable<ArmResource> ListByNameAsync(ArmSubstringFilter filter, int? top = null, CancellationToken cancellationToken = default)
        {
            ArmFilterCollection filters = new ArmFilterCollection(NetworkInterfaceData.ResourceType);
            filters.SubstringFilter = filter;
            return ResourceListOperations.ListAtContextAsync(ClientOptions, Id, filters, top, cancellationToken);
        }

        /// <summary>
        /// Filters the list of <see cref="NetworkInterface"/> resources for this <see cref="ResourceGroup"/>. 
        /// Makes an additional network call to retrieve the full data model for each <see cref="NetworkInterface"/>.
        /// <param name="filter"> A string to filter the <see cref="NetworkInterface"/> resources by name. </param>
        /// <param name="top"> The number of results to return per page of data. </param>
        /// <param name="cancellationToken"> A token to allow the caller to cancel the call to the service. 
        /// The default value is <see cref=System.Threading.CancellationToken.None" />. </param>
        /// <returns> A collection of resource operations that may take multiple service requests to iterate over. </returns>
        public Pageable<NetworkInterface> ListByNameExpanded(ArmSubstringFilter filter, int? top = null, CancellationToken cancellationToken = default)
        {
            var results = ListByName(filter, top, cancellationToken);
            return new PhWrappingPageable<ArmResource, NetworkInterface>(results, s => new NetworkInterfaceOperations(s).Get().Value);
        }

        /// <summary>
        /// Filters the list of <see cref="NetworkInterface"/> resources for this <see cref="ResourceGroup"/>. 
        /// Makes an additional network call to retrieve the full data model for each <see cref="NetworkInterface"/>.
        /// <param name="filter"> A string to filter the <see cref="NetworkInterface"/> resources by name. </param>
        /// <param name="top"> The number of results to return per page of data. </param>
        /// <param name="cancellationToken"> A token to allow the caller to cancel the call to the service. 
        /// The default value is <see cref=System.Threading.CancellationToken.None" />. </param>
        /// <returns> An async collection of resource operations that may take multiple service requests to iterate over. </returns>
        public AsyncPageable<NetworkInterface> ListByNameExpandedAsync(ArmSubstringFilter filter, int? top = null, CancellationToken cancellationToken = default)
        {
            var results = ListByNameAsync(filter, top, cancellationToken);
            return new PhWrappingAsyncPageable<ArmResource, NetworkInterface>(results, s => new NetworkInterfaceOperations(s).Get().Value);
        }
        
        private Func<Azure.ResourceManager.Network.Models.NetworkInterface, NetworkInterface> convertor()
        {
            return s => new NetworkInterface(ClientOptions, new NetworkInterfaceData(s));
        }
    }
}
