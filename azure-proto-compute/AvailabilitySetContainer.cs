﻿using Azure.ResourceManager.Compute;
using Azure.ResourceManager.Compute.Models;
using azure_proto_core;
using System.Threading;
using System.Threading.Tasks;

namespace azure_proto_compute
{
    /// <summary>
    /// Operatiosn class for Availability Set Contaienrs (resource groups)
    /// </summary>
    public class AvailabilitySetContainer : ResourceContainerOperations<XAvailabilitySet, PhAvailabilitySet>
    {
        public AvailabilitySetContainer(ArmClientContext context, PhResourceGroup resourceGroup) : base(context, resourceGroup) { }

        public override ResourceType ResourceType => "Microsoft.Compute/availabilitySets";

        public override ArmResponse<XAvailabilitySet> Create(string name, PhAvailabilitySet resourceDetails, CancellationToken cancellationToken = default)
        {
            var response = Operations.CreateOrUpdate(Id.ResourceGroup, name, resourceDetails.Model, cancellationToken);
            return new PhArmResponse<XAvailabilitySet, AvailabilitySet>(
                response,
                a => new XAvailabilitySet(ClientContext, new PhAvailabilitySet(a)));
        }

        public async override Task<ArmResponse<XAvailabilitySet>> CreateAsync(string name, PhAvailabilitySet resourceDetails, CancellationToken cancellationToken = default)
        {
            var response = await Operations.CreateOrUpdateAsync(Id.ResourceGroup, name, resourceDetails.Model, cancellationToken).ConfigureAwait(false);
            return new PhArmResponse<XAvailabilitySet, AvailabilitySet>(
                response,
                a => new XAvailabilitySet(ClientContext, new PhAvailabilitySet(a)));
        }

        public override ArmOperation<XAvailabilitySet> StartCreate(string name, PhAvailabilitySet resourceDetails, CancellationToken cancellationToken = default)
        {
            return new PhArmOperation<XAvailabilitySet, AvailabilitySet>(
                Operations.CreateOrUpdate(Id.ResourceGroup, name, resourceDetails.Model, cancellationToken),
                a => new XAvailabilitySet(ClientContext, new PhAvailabilitySet(a)));
        }

        public async override Task<ArmOperation<XAvailabilitySet>> StartCreateAsync(string name, PhAvailabilitySet resourceDetails, CancellationToken cancellationToken = default)
        {
            return new PhArmOperation<XAvailabilitySet, AvailabilitySet>(
                await Operations.CreateOrUpdateAsync(Id.ResourceGroup, name, resourceDetails.Model, cancellationToken).ConfigureAwait(false),
                a => new XAvailabilitySet(ClientContext, new PhAvailabilitySet(a)));
        }

        public ArmBuilder<XAvailabilitySet, PhAvailabilitySet> Construct(string skuName, Location location = null)
        {
            var availabilitySet = new AvailabilitySet(location ?? DefaultLocation)
            {
                PlatformUpdateDomainCount = 5,
                PlatformFaultDomainCount = 2,
                Sku = new Azure.ResourceManager.Compute.Models.Sku() { Name = skuName }
            };

            return new ArmBuilder<XAvailabilitySet, PhAvailabilitySet>(this, new PhAvailabilitySet(availabilitySet));
        }

        internal AvailabilitySetsOperations Operations => GetClient((uri, cred) => new ComputeManagementClient(uri, Id.Subscription, cred)).AvailabilitySets;
    }
}
