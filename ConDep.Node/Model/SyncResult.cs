namespace ConDep.Node.Model
{
    public class SyncResult
    {
        public int DeletedFiles { get; set; }
        public int DeletedDirectories { get; set; }
        public int UpdatedFiles { get; set; }
        public int CreatedFiles { get; set; }
    }
}