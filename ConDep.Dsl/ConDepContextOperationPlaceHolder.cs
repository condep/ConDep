using System;
using System.Diagnostics;
using ConDep.Dsl.Experimental.Core;
using ConDep.Dsl.WebDeploy;

namespace ConDep.Dsl
{
    internal class ConDepContextOperationPlaceHolder : ConDepOperationBase
    {
        private string _contextName;

        public ConDepContextOperationPlaceHolder(string contextName)
        {
            ContextName = contextName;
        }

        public string ContextName
        {
            get { return _contextName; }
            set { _contextName = value; }
        }

        public override IReportStatus Execute(IReportStatus status)
        {
            throw new NotImplementedException();
        }

        public override bool IsValid(Notification notification)
        {
            return true;
        }
    }
}