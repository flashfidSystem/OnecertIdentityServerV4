using System;

namespace OnecertApiV1.Services
{
    public class clsParameters
    {
        private readonly clsSetup _st;
        public clsParameters(clsSetup st)
        {
            _st = st;
        }
        private string[] _parameterNames;
        private string[] _parameterValues;
        private string[] parameterNames
        {
            get
            {
                return _parameterNames;
            }
            set
            {
                _parameterNames = value;
            }
        }

        private string[] parameterValues
        {
            get
            {
                return _parameterValues;
            }
            set
            {
                _parameterValues = value;
            }
        }

        public (string[] parameterNames, string[] parameterValues) GetParameters()
        {
            var paramTB = _st.listSomeSetup("CATEGORY='Parameters'");

            if (paramTB.Rows.Count > 0)
            {
                string[] parameterNames = new string[paramTB.Rows.Count];
                string[] parameterValues = new string[paramTB.Rows.Count];

                for (int i = 0; i < paramTB.Rows.Count; i++)
                {
                    var dr = paramTB.Rows[i];
                    parameterNames[i] = Convert.ToString(dr["code"]);
                    parameterValues[i] = Convert.ToString(dr["description"]);
                }

                return (parameterNames, parameterValues);
            }
            else
            {
                return (Array.Empty<string>(), Array.Empty<string>());
            }
        }

        public string getValue((string[] parameterNames, string[] parameterValues) parameters, string parameterName)
        {
            var (parameterNames, parameterValues) = parameters;
            string parameterValue = "";

            for (int i = 0; i < parameterNames.Length; i++)
            {
                if (string.Equals(parameterNames[i], parameterName, StringComparison.OrdinalIgnoreCase))
                {
                    parameterValue = parameterValues[i];
                    break;
                }
            }

            return parameterValue;
        }


    }
}