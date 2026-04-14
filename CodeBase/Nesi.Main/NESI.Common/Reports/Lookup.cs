using NESI.Common.Serialization;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace NESI.Common
{
    [DataContract]
    [Serializable]
    [TypeConverter(typeof(LookupConverter))]
    public class Lookup 
    {
        public Lookup(string lookupString)
        {
            if (string.IsNullOrWhiteSpace(lookupString) == false)
            {
                var arr = lookupString.Split(';');

                if (arr.Length > 0)
                {
                    endpoint = arr[0];
                    key = arr[1];
                    var valuemapper = arr[2];
                    if (arr.Length == 4)
                    {
                        var inputmapper = arr[3];
                        this.inputs = new[] { new KeyValuePair<string, string>(inputmapper.Split('@')[0], inputmapper.Split('@')[1]) };
                    }

                    this.values = new[] { new KeyValuePair<string, string>(valuemapper.Split('@')[0], valuemapper.Split('@')[1]) };
                }
            }
        }

        [DataMember]
        public string endpoint { get; set; }

        [DataMember]
        public string key { get; set; }

        [DataMember]
        public KeyValuePair<string, string>[] values { get; set; }

        [DataMember]
        public KeyValuePair<string, string>[] inputs { get; set; }

    }
}
