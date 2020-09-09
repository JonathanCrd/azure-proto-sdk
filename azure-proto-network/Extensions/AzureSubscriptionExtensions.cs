﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Azure;
using azure_proto_core;
using azure_proto_core.Adapters;
using azure_proto_core.Resources;

namespace azure_proto_network
{
    public static class AzureSubscriptionExtensions
    {
        #region Virtual Network Operations

        public static Pageable<VirtualNetworkOperations> ListVnets(this SubscriptionOperations subscription, ArmSubstringFilter filter = null, int? top = null, CancellationToken cancellationToken = default)
        {
            var collection = new VirtualNetworkCollection( subscription, subscription.Id);
            return new PhWrappingPageable<ResourceOperationsBase<PhVirtualNetwork>, VirtualNetworkOperations>(collection.List(filter, top, cancellationToken), vnet => new VirtualNetworkOperations(vnet, vnet.Context));
        }

        public static AsyncPageable<VirtualNetworkOperations> ListVnetsAsync(this SubscriptionOperations subscription, ArmSubstringFilter filter = null, int? top = null, CancellationToken cancellationToken = default)
        {
            var collection = new VirtualNetworkCollection(subscription, subscription.Id);
            return new PhWrappingAsyncPageable<ResourceOperationsBase<PhVirtualNetwork>, VirtualNetworkOperations>(collection.ListAsync(filter, top, cancellationToken), vnet => new VirtualNetworkOperations(vnet, vnet.Context));
        }


        #endregion

        #region Public IP Address Operations

        public static Pageable<ResourceOperationsBase<PhPublicIPAddress>> ListPublicIps(this SubscriptionOperations subscription, ArmSubstringFilter filter = null, int? top = null, CancellationToken cancellationToken = default)
        {
            var collection = new PublicIpAddressCollection(subscription, subscription.Id);
            return collection.List(filter, top, cancellationToken);
        }

        public static AsyncPageable<ResourceOperationsBase<PhPublicIPAddress>> ListPublicIpsAsync(this SubscriptionOperations subscription, ArmSubstringFilter filter = null, int? top = null, CancellationToken cancellationToken = default)
        {
            var collection = new PublicIpAddressCollection(subscription, subscription.Id);
            return collection.ListAsync(filter, top, cancellationToken);
        }

        #endregion

        #region Network Interface (NIC) operations

        public static Pageable<ResourceOperationsBase<PhNetworkInterface>> ListNics(this SubscriptionOperations subscription, ArmSubstringFilter filter = null, int? top = null, CancellationToken cancellationToken = default)
        {
            var collection = new NetworkInterfaceCollection(subscription, subscription.Id);
            return collection.List(filter, top, cancellationToken);
        }

        public static AsyncPageable<ResourceOperationsBase<PhNetworkInterface>> ListNicsAsync(this SubscriptionOperations subscription, ArmSubstringFilter filter = null, int? top = null, CancellationToken cancellationToken = default)
        {
            var collection = new NetworkInterfaceCollection(subscription, subscription.Id);
            return collection.ListAsync(filter, top, cancellationToken);
        }


        #endregion

        #region Network Security Group operations
        public static Pageable<ResourceOperationsBase<PhNetworkSecurityGroup>> ListNsgs(this SubscriptionOperations subscription, ArmSubstringFilter filter = null, int? top = null, CancellationToken cancellationToken = default)
        {
            var collection = new NetworkSecurityGroupCollection(subscription, subscription.Id);
            return collection.List(filter, top, cancellationToken);
        }

        public static AsyncPageable<ResourceOperationsBase<PhNetworkSecurityGroup>> ListNsgsAsync(this SubscriptionOperations subscription, ArmSubstringFilter filter = null, int? top = null, CancellationToken cancellationToken = default)
        {
            var collection = new NetworkSecurityGroupCollection(subscription, subscription.Id);
            return collection.ListAsync(filter, top, cancellationToken);
        }

        #endregion
    }
}
