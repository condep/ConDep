using ConDep.Dsl.Operations.Application.Deployment.WindowsService;

namespace ConDep.Dsl
{
    public interface IOfferWindowsServiceFailureActions
    {
        IOfferWindowsServiceFailureOptions TakeNoAction();
        IOfferWindowsServiceFailureOptions RestartService(int delayInMilliseconds);
        IOfferWindowsServiceFailureOptions RunProgram(string program, string parameters, int delayInMilliseconds);
        IOfferWindowsServiceFailureOptions RestartComputer(int delayInMilliseconds);
    }
}