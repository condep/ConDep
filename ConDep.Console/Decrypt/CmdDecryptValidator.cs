namespace ConDep.Console.Decrypt
{
    public class CmdDecryptValidator : CmdBaseValidator<ConDepDecryptOptions>
    {
        public override void Validate(ConDepDecryptOptions options)
        {
            if (string.IsNullOrWhiteSpace(options.Key))
                throw new ConDepValidationException("Decryption key not provided.");
        }
    }
}