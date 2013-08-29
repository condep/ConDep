using ConDep.Dsl.Security;

namespace ConDep.Console.Decrypt
{
    public class CmdDecryptValidator : CmdBaseValidator<ConDepDecryptOptions>
    {
        public override void Validate(ConDepDecryptOptions options)
        {
            if (string.IsNullOrWhiteSpace(options.Key))
                throw new ConDepValidationException("Decryption key not provided.");

            if (!JsonPasswordCrypto.ValidKey(options.Key))
                throw new ConDepCmdParseException("Decryption key not valid. Key must be base64 encoded and 128, 192 or 256 bits long.");
        }
    }
}