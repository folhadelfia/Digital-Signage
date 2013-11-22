using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using System.ServiceModel;
using System.ServiceModel.Discovery;
using System.Xml;

namespace ServiceDiscoveryProxy
{
    [ServiceBehavior(InstanceContextMode=InstanceContextMode.Single, ConcurrencyMode=ConcurrencyMode.Multiple)]
    public class ServiceDiscoveryProxy : DiscoveryProxy
    {
        private Dictionary<EndpointAddress, EndpointDiscoveryMetadata> onlineServices;

        public event DiscoveryMetadataPrintHandler PrintDiscoveryMetadata;

        public ServiceDiscoveryProxy()
        {
            onlineServices = new Dictionary<EndpointAddress, EndpointDiscoveryMetadata>();
        }

        /// <summary>
        /// Adiciona um serviço online à cache
        /// </summary>
        /// <param name="endpointDiscoveryMetadata"></param>
        public void AddOnlineService(EndpointDiscoveryMetadata endpointDiscoveryMetadata)
        {
            lock (this.onlineServices)
            {
                this.onlineServices[endpointDiscoveryMetadata.Address] = endpointDiscoveryMetadata;
            }

            if (PrintDiscoveryMetadata != null)
                PrintDiscoveryMetadata(endpointDiscoveryMetadata, "Added", this.onlineServices.Count);
        }

        /// <summary>
        /// Remove um serviço da cache
        /// </summary>
        /// <param name="endpointDiscoveryMetadata"></param>
        public void RemoveOnlineService(EndpointDiscoveryMetadata endpointDiscoveryMetadata)
        {
            lock (this.onlineServices)
            {
                this.onlineServices.Remove(endpointDiscoveryMetadata.Address);
            }

            if (PrintDiscoveryMetadata != null)
                PrintDiscoveryMetadata(endpointDiscoveryMetadata, "Removed", this.onlineServices.Count);
        }

        /// <summary>
        /// Encontra os serviços que conferem os requisitos inseridos
        /// </summary>
        /// <param name="findRequestContext"></param>
        public void MatchFromOnlineService(FindRequestContext findRequestContext)
        {
            lock (this.onlineServices)
            {
                foreach (EndpointDiscoveryMetadata endpointDiscoveryMetadata in this.onlineServices.Values)
                {
                    if (findRequestContext.Criteria.IsMatch(endpointDiscoveryMetadata))
                    {
                        if (PrintDiscoveryMetadata != null)
                            PrintDiscoveryMetadata(endpointDiscoveryMetadata, "Matched", this.onlineServices.Count);

                        findRequestContext.AddMatchingEndpoint(endpointDiscoveryMetadata);
                    }
                }
            }
        }

        /// <summary>
        /// Encontra os serviços que conferem os requisitos inseridos
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public EndpointDiscoveryMetadata MatchFromOnlineService(ResolveCriteria criteria)
        {
            lock (this.onlineServices)
            {
                foreach (EndpointDiscoveryMetadata endpointDiscoveryMetadata in this.onlineServices.Values)
                {
                    if (criteria.Address == endpointDiscoveryMetadata.Address)
                    {
                        if (PrintDiscoveryMetadata != null)
                            PrintDiscoveryMetadata(endpointDiscoveryMetadata, "Matched", this.onlineServices.Count);

                        return endpointDiscoveryMetadata;
                    }
                }
            }

            return null;
        }



        #region Overrides de DiscoveryProxy
        protected override IAsyncResult OnBeginFind(FindRequestContext findRequestContext, AsyncCallback callback, object state)
        {
            this.MatchFromOnlineService(findRequestContext);
            return new OnFindAsyncResult(callback, state);
        }

        protected override IAsyncResult OnBeginOfflineAnnouncement(DiscoveryMessageSequence messageSequence, EndpointDiscoveryMetadata endpointDiscoveryMetadata, AsyncCallback callback, object state)
        {
            this.RemoveOnlineService(endpointDiscoveryMetadata);
            return new OnOfflineAnnouncementAsyncResult(callback, state);
        }

        protected override IAsyncResult OnBeginOnlineAnnouncement(DiscoveryMessageSequence messageSequence, EndpointDiscoveryMetadata endpointDiscoveryMetadata, AsyncCallback callback, object state)
        {
            this.AddOnlineService(endpointDiscoveryMetadata);
            return new OnOnlineAnnouncementAsyncResult(callback, state);
        }

        protected override IAsyncResult OnBeginResolve(ResolveCriteria resolveCriteria, AsyncCallback callback, object state)
        {
            return new OnResolveAsyncResult(this.MatchFromOnlineService(resolveCriteria), callback, state);
        }

        protected override void OnEndFind(IAsyncResult result)
        {
            OnFindAsyncResult.End(result);
        }

        protected override void OnEndOfflineAnnouncement(IAsyncResult result)
        {
            OnOnlineAnnouncementAsyncResult.End(result);
        }

        protected override void OnEndOnlineAnnouncement(IAsyncResult result)
        {
            OnOnlineAnnouncementAsyncResult.End(result);
        }

        protected override EndpointDiscoveryMetadata OnEndResolve(IAsyncResult result)
        {
            return OnResolveAsyncResult.End(result);
        }
        #endregion
    }

    sealed class OnOnlineAnnouncementAsyncResult : AsyncResultProxy
    {
        public OnOnlineAnnouncementAsyncResult(AsyncCallback callback, object state)
            : base(callback, state)
        {
            this.Complete(true);
        }

        public static void End(IAsyncResult result)
        {
            AsyncResultProxy.End<OnOnlineAnnouncementAsyncResult>(result);
        }
    }

    sealed class OnOfflineAnnouncementAsyncResult : AsyncResultProxy
    {
        public OnOfflineAnnouncementAsyncResult(AsyncCallback callback, object state)
            : base(callback, state)
        {
            this.Complete(true);
        }

        public static void End(IAsyncResult result)
        {
            AsyncResultProxy.End<OnOfflineAnnouncementAsyncResult>(result);
        }
    }

    sealed class OnFindAsyncResult : AsyncResultProxy
    {
        public OnFindAsyncResult(AsyncCallback callback, object state)
            : base(callback, state)
        {
            this.Complete(true);
        }

        public static void End(IAsyncResult result)
        {
            AsyncResultProxy.End<OnFindAsyncResult>(result);
        }
    }

    sealed class OnResolveAsyncResult : AsyncResultProxy
    {
        EndpointDiscoveryMetadata matchingEndpoint;

        public OnResolveAsyncResult(EndpointDiscoveryMetadata matchingEndpoint, AsyncCallback callback, object state)
            : base(callback, state)
        {
            this.matchingEndpoint = matchingEndpoint;
            this.Complete(true);
        }

        public static EndpointDiscoveryMetadata End(IAsyncResult result)
        {
            OnResolveAsyncResult thisPtr = AsyncResultProxy.End<OnResolveAsyncResult>(result);
            return thisPtr.matchingEndpoint;
        }
    }
}
