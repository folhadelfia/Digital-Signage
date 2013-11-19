using System.ServiceModel.Discovery;

namespace ServiceDiscoveryProxy
{
    public delegate void DiscoveryMetadataPrintHandler(EndpointDiscoveryMetadata endpointDiscoveryMetadata, string verb, int serviceCount);
}