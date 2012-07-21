using System;

namespace ConDep.Dsl.Core.IntConsole
{
    [AttributeUsage(AttributeTargets.Field)]
	public sealed class CmdParamAttribute : Attribute
	{
	    private readonly bool _mandatory;

        public CmdParamAttribute()
        {
            _mandatory = false;
        }

	    public CmdParamAttribute(bool mandatory)
        {
            _mandatory = mandatory;
        }

	    public bool Mandatory
	    {
	        get { return _mandatory; }
	    }
	}
}