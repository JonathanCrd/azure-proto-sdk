﻿using Azure;
using Azure.ResourceManager.Network;
using Azure.ResourceManager.Network.Models;
using Azure.ResourceManager.Core;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System;

namespace azure_proto_network
{
    /// <summary>
    /// A class representing the operations that can be pefroemd over a specific <see cref="NetworkInterface"/>.
    /// </summary>
    public class NetworkInterfaceOperations : ResourceOperationsBase<NetworkInterface>, ITaggableResource<NetworkInterface>, IDeletableResource
    {
        internal NetworkInterfaceOperations(GenericResourceOperations genericOperations)
            : base(genericOperations)
        {
        }

        internal NetworkInterfaceOperations(ResourceGroupOperations resourceGroup, string nicName)
            : base(resourceGroup, $"{resourceGroup.Id}/providers/Microsoft.Network/networkInterfaces/{nicName}")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NetworkInterfaceOperations"/> class.
        /// </summary>
        /// <param name="options"> The client parameters to use in these operations. </param>
        /// <param name="id"> The identifier of the resource that is the target of operations. </param>
        protected NetworkInterfaceOperations(ResourceOperationsBase options, ResourceIdentifier id)
            : base(options, id)
        {
        }

        /// <summary>
        /// The resource type of a <see cref="NetworkInterface"/>.
        /// </summary>
        public static readonly ResourceType ResourceType = "Microsoft.Network/networkInterfaces";

        /// <inheritdoc/>
        protected override ResourceType ValidResourceType => ResourceType;

        internal NetworkInterfacesOperations Operations => new NetworkManagementClient(
            Id.Subscription,
            BaseUri,
            Credential,
            ClientOptions.Convert<NetworkManagementClientOptions>()).NetworkInterfaces;

        /// <summary>
        /// Deletes a <see cref="NetworkInterface"/>.
        /// </summary>
        /// <returns> An <see cref="ArmResponse"/> representing the service response to deletion. </returns>
        public ArmResponse<Response> Delete()
        {
            return new ArmResponse(Operations.StartDelete(Id.ResourceGroup, Id.Name).WaitForCompletionAsync().ConfigureAwait(false).GetAwaiter().GetResult());
        }

        /// <summary>
        /// Deletes a <see cref="NetworkInterface"/>.
        /// </summary>
        /// <param name="cancellationToken"> A token to allow the caller to cancel the call to the service. 
        /// The default value is <see cref="System.Threading.CancellationToken.None" />. </param>
        /// <returns> A <see cref="Task"/> that returns an <see cref="ArmResponse"/> when completed. </returns>
        public async Task<ArmResponse<Response>> DeleteAsync(CancellationToken cancellationToken = default)
        {
            return new ArmResponse((await Operations.StartDeleteAsync(Id.ResourceGroup, Id.Name, cancellationToken)).WaitForCompletionAsync().ConfigureAwait(false).GetAwaiter().GetResult());
        }

        /// <summary>
        /// Deletes a <see cref="NetworkInterface"/>.
        /// </summary>
        /// <param name="cancellationToken"> A token to allow the caller to cancel the call to the service. 
        /// The default value is <see cref="System.Threading.CancellationToken.None" />. </param>
        /// <returns> An <see cref="ArmOperation{Response}"/> that allows polling for completion of the operation. </returns>
        /// <remarks>
        /// <see href="https://azure.github.io/azure-sdk/dotnet_introduction.html#dotnet-longrunning"> Details on long running operation object. </see>
        /// </remarks>
        public ArmOperation<Response> StartDelete(CancellationToken cancellationToken = default)
        {
            return new ArmVoidOperation(Operations.StartDelete(Id.ResourceGroup, Id.Name, cancellationToken));
        }

        /// <summary>
        /// Deletes a <see cref="NetworkInterface"/>.
        /// </summary>
        /// <param name="cancellationToken"> A token to allow the caller to cancel the call to the service. 
        /// The default value is <see cref="System.Threading.CancellationToken.None" />. </param>
        /// <returns> A <see cref="Task"/> that on completion returns an <see cref="ArmOperation{Response}"/> that allows polling for completion of the operation. </returns>
        /// <remarks>
        /// <see href="https://azure.github.io/azure-sdk/dotnet_introduction.html#dotnet-longrunning"> Details on long running operation object. </see>
        /// </remarks>
        public async Task<ArmOperation<Response>> StartDeleteAsync(CancellationToken cancellationToken = default)
        {
            return new ArmVoidOperation(await Operations.StartDeleteAsync(Id.ResourceGroup, Id.Name, cancellationToken));
        }

        /// <summary>
        /// Gets details of the <see cref="NetworkInterface"/> from the service.
        /// </summary>
        /// <returns> An <see cref="ArmResponse{NetworkInterface}"/>. </returns>
        public override ArmResponse<NetworkInterface> Get()
        {
            return new PhArmResponse<NetworkInterface, Azure.ResourceManager.Network.Models.NetworkInterface>(
                Operations.Get(Id.ResourceGroup, Id.Name),
                n => new NetworkInterface(this, new NetworkInterfaceData(n)));
        }

        /// <inheritdoc/>
        public async override Task<ArmResponse<NetworkInterface>> GetAsync(CancellationToken cancellationToken = default)
        {
            return new PhArmResponse<NetworkInterface, Azure.ResourceManager.Network.Models.NetworkInterface>(
                await Operations.GetAsync(Id.ResourceGroup, Id.Name, null, cancellationToken),
                n => new NetworkInterface(this, new NetworkInterfaceData(n)));
        }

        /// <summary>
        /// Add the given tag key and tag value to the <see cref="NetworkInterface"/> resource.
        /// </summary>
        /// <param name="key" > The tag key. </param>
        /// <param name="value"> The Tag Value. </param>
        /// <returns> An <see cref="ArmOperation{NetworkInterface}"/> that allows polling for completion of the operation. </returns>
        /// <remarks>
        /// <see href="https://azure.github.io/azure-sdk/dotnet_introduction.html#dotnet-longrunning"> Details on long running operation object. </see>
        /// </remarks>
        public ArmOperation<NetworkInterface> StartAddTag(string key, string value)
        {
            var patchable = new TagsObject();
            patchable.Tags[key] = value;
            return new PhArmOperation<NetworkInterface, Azure.ResourceManager.Network.Models.NetworkInterface>(Operations.UpdateTags(Id.ResourceGroup, Id.Name, patchable),
                n => new NetworkInterface(this, new NetworkInterfaceData(n)));
        }

        /// <summary>
        /// Add the given tag key and tag value to the <see cref="NetworkInterface"/> resource.
        /// </summary>
        /// <param name="key" > The tag key. </param>
        /// <param name="value"> The Tag Value. </param>
        /// <param name="cancellationToken"> A token to allow the caller to cancel the call to the service. 
        /// The default value is <see cref="System.Threading.CancellationToken.None" />. </param>
        /// <returns> A <see cref="Task"/> that on completion returns a <see cref="ArmOperation{NetworkInterface}"/> that allows polling for completion of the operation. </returns>
        /// <remarks>
        /// <see href="https://azure.github.io/azure-sdk/dotnet_introduction.html#dotnet-longrunning"> Details on long running operation object. </see>
        /// </remarks>
        public async Task<ArmOperation<NetworkInterface>> StartAddTagAsync(string key, string value, CancellationToken cancellationToken = default)
        {
            var patchable = new TagsObject();
            patchable.Tags[key] = value;
            return new PhArmOperation<NetworkInterface, Azure.ResourceManager.Network.Models.NetworkInterface>(
                await Operations.UpdateTagsAsync(Id.ResourceGroup, Id.Name, patchable, cancellationToken),
                n => new NetworkInterface(this, new NetworkInterfaceData(n)));
        }

        /// <summary>
        /// Lists all available geo-locations.
        /// </summary>
        /// <returns> A collection of location that may take multiple service requests to iterate over. </returns>
        public IEnumerable<LocationData> ListAvailableLocations()
        {
            var pageableProvider = ResourcesClient.Providers.List(expand: "metadata");
            var networkInterfaceProvider = pageableProvider.FirstOrDefault(p => string.Equals(p.Namespace, ResourceType?.Namespace, StringComparison.InvariantCultureIgnoreCase));
            var networkInterfaceResource = networkInterfaceProvider.ResourceTypes.FirstOrDefault(r => ResourceType.Type.Equals(r.ResourceType)); 
            return networkInterfaceResource.Locations.Select(l => (LocationData)l);
        }

        /// <summary>
        /// Lists all available geo-locations.
        /// </summary>
        /// <param name="cancellationToken"> A token to allow the caller to cancel the call to the service. The default value is <see cref="P:System.Threading.CancellationToken.None" />. </param>
        /// <returns> An async collection of location that may take multiple service requests to iterate over. </returns>
        /// <exception cref="InvalidOperationException"> The default subscription id is null. </exception>
        public async Task<IEnumerable<LocationData>> ListAvailableLocationsAsync(CancellationToken cancellationToken = default)
        {
            var asyncpageableProvider = ResourcesClient.Providers.ListAsync(expand: "metadata", cancellationToken: cancellationToken);
            var networkInterfaceProvider = await asyncpageableProvider.FirstOrDefaultAsync(p => string.Equals(p.Namespace, ResourceType?.Namespace, StringComparison.InvariantCultureIgnoreCase));
            var networkInterfaceResource = networkInterfaceProvider.ResourceTypes.FirstOrDefault(r => ResourceType.Type.Equals(r.ResourceType));
            return networkInterfaceResource.Locations.Select(l => (LocationData)l);
        }
    }
}
