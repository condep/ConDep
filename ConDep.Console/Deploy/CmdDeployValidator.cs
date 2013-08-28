using ConDep.Dsl.Config;

namespace ConDep.Console
{
    public class CmdDeployValidator : CmdBaseValidator<ConDepOptions>
    {
        public override void Validate(ConDepOptions options)
        {
            ValidateAssemblyName(options);
            ValidateEnvironment(options);
            ValidateApplication(options);
        }

        private void ValidateEnvironment(ConDepOptions options)
        {
            if (!string.IsNullOrWhiteSpace(options.Environment)) return;

            throw new ConDepCmdParseException("No environment provided.");
        }

        private void ValidateAssemblyName(ConDepOptions options)
        {
            if (!string.IsNullOrWhiteSpace(options.AssemblyName)) return;

            throw new ConDepCmdParseException("No assembly provided.");
        }

        private void ValidateApplication(ConDepOptions options)
        {
            if (options.DeployAllApps) return;
            if (!string.IsNullOrWhiteSpace(options.Application)) return;

            throw new ConDepCmdParseException("No application provided. If you want to deploy all applications, use the --deployAllApps switch.");
        }
    }
}