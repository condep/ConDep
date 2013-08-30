using ConDep.Dsl.Config;
using ConDep.Dsl.Security;

namespace ConDep.Console
{
    public class CmdDeployValidator : CmdBaseValidator<ConDepOptions>
    {
        public override void Validate(ConDepOptions options)
        {
            ValidateAssemblyName(options);
            ValidateEnvironment(options);
            ValidateApplication(options);
            ValidateCryptoKey(options);
        }

        private void ValidateCryptoKey(ConDepOptions options)
        {
            if (!string.IsNullOrWhiteSpace(options.CryptoKey))
            {
                if (!JsonPasswordCrypto.ValidKey(options.CryptoKey))
                {
                    throw new ConDepCmdParseException("Decryption key not valid. Key must be base64 encoded and 128, 192 or 256 bits long.");
                }
            }
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