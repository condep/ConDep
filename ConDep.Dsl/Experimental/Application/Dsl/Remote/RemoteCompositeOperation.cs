namespace ConDep.Dsl.Experimental.Application.Dsl.Remote
{
    public abstract class RemoteCompositeOperation
    {
        public abstract void Configure(IOfferRemoteOptions server);
        public string SourcePath { get; set; }
        public virtual string DestinationPath { get; set; }
        public abstract bool IsValid(Notification notification);
    }
}