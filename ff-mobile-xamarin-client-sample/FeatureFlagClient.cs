using System;
using System.Collections.Generic;

namespace ff_mobile_xamarin_client_sample
{
    public struct ValueType
    {
        public bool? BoolValue { get; set; }
        public int? IntValue { get; set; }
        public string StringValue { get; set; }
        public Dictionary<string,ValueType> ObjectValue { get; set; }
    }
    public struct Evaluation
    {
        public string Flag { get; set; }
        public string Identifier { get; set; }
        public ValueType Value { get; set; }
    }
    public delegate void NotifyEvaluationChanged(List<Evaluation> ev);
    public interface IFeatureFlagClient
    {
        void Authenticate(string account);
        event NotifyEvaluationChanged EvaluationChanged;
        event EventHandler<bool> InitializationStatus;
        event EventHandler<string> Error;
    }
}
