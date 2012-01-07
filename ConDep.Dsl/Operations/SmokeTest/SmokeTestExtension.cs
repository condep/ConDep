using ConDep.Dsl.Builders;

namespace ConDep.Dsl
{
    public static class SmokeTestExtension
    {
        public static void SmokeTest(this SetupOptions setupOptions, string url)
        {
            var smokeTestOperation = new SmokeTestOperation(url);
            setupOptions.AddOperation(smokeTestOperation);
        }
    }
}