using ConDep.Dsl.WebDeploy;

namespace ConDep.Dsl.Experimental.Core
{
    public interface ISequenceElement : IValidate
    {
        IReportStatus Execute(IReportStatus status);
    }
}