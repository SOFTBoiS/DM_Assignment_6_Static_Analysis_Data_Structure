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
        public IDictionary<string, SetTheory.ISet<long>> state;

        public State(IDictionary<string, SetTheory.ISet<long>> state)
        {
            this.state = state;
        }

        public void AddVariable(string name, SetTheory.ISet<long> value)
        {
            this.state.Add(name, value);
        }

        

        public int CompareTo(IDictionary<string, SetTheory.ISet<long>> other)
        {

            foreach(KeyValuePair<string, SetTheory.ISet<long>> variable in this.state)
            {
                var compareRes = variable.Value.CompareTo(other[variable.Key]!);
                switch (compareRes)
                {
                    case -2:
                        return -2;
                    case 1:
                        return -2;
                    case 2:
                        return 2;
                }
                    
            }

            return -1;
        }

        public IDictionary<string, SetTheory.ISet<long>> Minimum(IDictionary<string, SetTheory.ISet<long>> other)
        {
            var minState = new Dictionary<string, SetTheory.ISet<long>>();

            foreach (KeyValuePair<string, SetTheory.ISet<long>> variable in this.state)
            {
                minState.Add(variable.Key, variable.Value.Intersection(other[variable.Key]));
            }
            return minState;
        }

        public IDictionary<string, SetTheory.ISet<long>> Maximum(IDictionary<string, SetTheory.ISet<long>> other)
        {
            var maxState = new Dictionary<string, SetTheory.ISet<long>>();

            foreach (KeyValuePair<string, SetTheory.ISet<long>> variable in other)
            {
                maxState.Add(variable.Key, variable.Value.Union(this.state[variable.Key]));
            }
            return maxState;
        }
    }
}
