using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace ConDep.Dsl.Console
{
    public class SettingsHandler
    {
        public static TSettings CreateInstanceOfEnvironmentSettings<TSettings>(string env) where TSettings : class
        {
            var interfaceType = typeof(TSettings);

            if (!interfaceType.IsInterface)
            {
                throw new ArgumentException(string.Format("Not an interface [{0}].", interfaceType.Name));
            }

            return GetImplementersOfInterfaceForEnvironment<TSettings>(interfaceType, env);
        }

        public static void AddSettingsFromCmdLine<TSettings>(IEnumerable<CmdParam> settingParams, TSettings settings)
        {
            foreach (var param in settingParams)
            {
                settings.GetType().GetField(param.ParamName).SetValue(settings, param.ParamValue);
            }
        }

        public static StringBuilder GetCmdHelpForSettings<TSettings>(TSettings settings)
        {
            var validSettings = new StringBuilder();
            foreach (var setting in settings.GetType().GetFields().Where(setting => setting.IsDefined(typeof(CmdParamAttribute), false)))
            {
                var attrib = setting.GetCustomAttributes(typeof(CmdParamAttribute), false).FirstOrDefault() as CmdParamAttribute;
                string settingHelp;

                if (attrib == null || !attrib.Mandatory)
                {
                    settingHelp = string.Format("{0}=value ", setting.Name);
                }
                else
                {
                    settingHelp = string.Format("[{0}=value] ", setting.Name);
                }
                validSettings.Append(settingHelp);
            }
            return validSettings;
        }

        private static TSettings GetImplementersOfInterfaceForEnvironment<TSettings>(Type interfaceType, string env) where TSettings : class
        {
            var implementers = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type =>
                       interfaceType.IsAssignableFrom(type)
                       &&
                       type.IsClass
                       &&
                       type.Name.StartsWith(env, true, CultureInfo.InvariantCulture)
                ).ToList();

            if (implementers == null || implementers.Count() == 0)
            {
                throw new ArgumentException(string.Format("No implementor of the interface [{0}] starting with [{1}] was found.", interfaceType.Name, env));
            }

            if (implementers.Count() > 1)
            {
                throw new ArgumentException(string.Format("More than one implementor of the interface [{0}] starting with [{1}] was found.", interfaceType.Name, env));
            }

            return implementers.First() as TSettings;
        }
    }
}