using System;

namespace Vale.Parsing
{
    class AbilityParameterInfo
    {
        private string paramType;

        public string Name { get; set; }
        public object Value { get; set; }

        public string ParamType
        {
            set
            {
                paramType = value;

                try
                {
                    ParameterType = Type.GetType(paramType);

                }
                catch (Exception)
                {
                    throw new Exception("Could not create parameter of type \"" + paramType + "\"");
                }
            }
        }

        public Type ParameterType { get; private set; }
    }
}
