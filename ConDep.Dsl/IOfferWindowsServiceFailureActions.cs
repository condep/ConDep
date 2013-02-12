using ConDep.Dsl.Operations.Application.Deployment.WindowsService;

namespace ConDep.Dsl
{
    public interface IOfferWindowsServiceFailureActions
    {
        /// <summary>
        /// Will take no action on service failure
        /// </summary>
        /// <returns></returns>
        IOfferWindowsServiceFailureOptions TakeNoAction();

        /// <summary>
        /// Will restart the Windows Service after given amount of milliseconds on service failure.
        /// </summary>
        /// <param name="delayInMilliseconds"></param>
        /// <returns></returns>
        IOfferWindowsServiceFailureOptions RestartService(int delayInMilliseconds);

        /// <summary>
        /// Will run a program with given parameters after the defined amount of milliseconds on service failure.
        /// </summary>
        /// <param name="program"></param>
        /// <param name="parameters"></param>
        /// <param name="delayInMilliseconds"></param>
        /// <returns></returns>
        IOfferWindowsServiceFailureOptions RunProgram(string program, string parameters, int delayInMilliseconds);

        /// <summary>
        /// Will restart computer after given amount of millisecond on service failure.
        /// </summary>
        /// <param name="delayInMilliseconds"></param>
        /// <returns></returns>
        IOfferWindowsServiceFailureOptions RestartComputer(int delayInMilliseconds);
    }
}