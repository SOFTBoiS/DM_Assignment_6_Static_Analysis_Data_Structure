using System;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using System.Linq;

namespace DM_Assignment_6a_Static_Analysis_Data_Structure
{
    class State
    {
        IDictionary<string, SetTheory.ISet<int>> state;

        public State(IDictionary<string, SetTheory.ISet<int>> state)
        {
            this.state = state;
        }

        void AddVariable(string name, SetTheory.ISet<int> value)
        {
            this.state.Add(name, value);
        }

        

        int CompareTo(IDictionary<string, SetTheory.ISet<int>> other)
        {

            foreach(KeyValuePair<string, SetTheory.ISet<int>> variable in this.state)
            {
                var compareRes = variable.Value.CompareTo(other[variable.Key]);
                switch (compareRes)
                {
                    case -2:
                        return -2;
                    case 1:
                        return 1;
                    case 2:
                        return 2;
                }
                    
            }

            return -1;
        }

        IDictionary<string, SetTheory.ISet<int>> Minimum(IDictionary<string, SetTheory.ISet<int>> other)
        {
            var minState = new Dictionary<string, SetTheory.ISet<int>>();

            foreach (KeyValuePair<string, SetTheory.ISet<int>> variable in this.state)
            {
                minState.Add(variable.Key, variable.Value.Intersection(other[variable.Key]));
            }
            return minState;
        }

        IDictionary<string, SetTheory.ISet<int>> Maximum(IDictionary<string, SetTheory.ISet<int>> other)
        {
            var maxState = new Dictionary<string, SetTheory.ISet<int>>();

            foreach (KeyValuePair<string, SetTheory.ISet<int>> variable in other)
            {
                maxState.Add(variable.Key, variable.Value.Union(this.state[variable.Key]));
            }
            return maxState;
        }
    }
}
