﻿using System;
using Newtonsoft.Json.Serialization;

namespace OsuSharp.Net.Serialization
{
    internal sealed class ContractResolver : DefaultContractResolver
    {
        public static readonly ContractResolver Instance = new ContractResolver();

        private ContractResolver()
        {
            
        }
        
        protected override JsonContract CreateContract(Type objectType)
        {
            var contract = base.CreateContract(objectType);

            if (objectType.IsGenericType && objectType.GetGenericTypeDefinition() == typeof(Optional<>))
            {
                contract.Converter = OptionalConverter.Instance;
            }

            return contract;
        }
    }
}