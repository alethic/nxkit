using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;

namespace NXKit.Composition
{

    public static class ExportProviderExtensions
    {

        /// <summary>
        /// Gets all exports with the specified contract name.
        /// </summary>
        /// <param name="exports"></param>
        /// <param name="contractType"></param>
        /// <returns></returns>
        public static IEnumerable<object> GetExports(this ExportProvider exports, Type contractType)
        {
            return exports.GetExports(new ContractBasedImportDefinition(
                AttributedModelServices.GetContractName(contractType),
                AttributedModelServices.GetTypeIdentity(contractType),
                null,
                ImportCardinality.ZeroOrMore,
                false,
                false,
                CreationPolicy.Any));
        }

    }

}
