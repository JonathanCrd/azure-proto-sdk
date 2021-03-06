﻿using Azure.ResourceManager.Core;
using azure_proto_compute;
using System;

namespace client
{
    class CheckResourceExists : Scenario
    {
        public override void Execute()
        {
            var client = new AzureResourceManagerClient();
            var subOp = client.DefaultSubscription;
            var rgContainer = subOp.GetResourceGroupContainer();

            Console.WriteLine($"Making sure {Context.RgName} doesn't exist yet.");
            if (rgContainer.DoesExist(Context.RgName))
                throw new Exception($"The resource group {Context.RgName} should not have existed.");

            Console.WriteLine($"Creating {Context.RgName}");
            _ = rgContainer.Construct(LocationData.Default).CreateOrUpdate(Context.RgName).Value;

            Console.WriteLine($"Making sure {Context.RgName} exists now.");
            if (!rgContainer.DoesExist(Context.RgName))
                throw new Exception($"The resource group {Context.RgName} should have existed.");

            Console.WriteLine($"Using try get value to retrieve {Context.RgName}");
            ArmResponse<ResourceGroup> rgOutput;
            if(!rgContainer.TryGetValue(Context.RgName, out rgOutput))
                throw new Exception($"The resource group {Context.RgName} should have existed.");

            var rg = rgOutput.Value;

            var asetContainer = rg.GetAvailabilitySetContainer();
            var asetName = Context.VmName + "_aSet";

            Console.WriteLine($"Making sure {asetName} doesn't exist yet.");
            if (asetContainer.DoesExist(asetName))
                throw new Exception($"The availability set {asetName} should not have existed.");

            Console.WriteLine($"Creating {asetName}");
            var aset = asetContainer.Construct("Aligned").CreateOrUpdate(asetName).Value;

            Console.WriteLine($"Making sure {asetName} exists now.");
            if (!asetContainer.DoesExist(asetName))
                throw new Exception($"The availability set {asetName} should have existed.");

            Console.WriteLine("Using try get value to retrieve the rg");
            ArmResponse<AvailabilitySet> asetOutput;
            if (!asetContainer.TryGetValue(asetName, out asetOutput))
                throw new Exception($"The availability set {asetName} should have existed.");
                
            
        }
    }
}
