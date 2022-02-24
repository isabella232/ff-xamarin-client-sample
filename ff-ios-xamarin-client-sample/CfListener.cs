using System;
using ff_ios_client_sdk_proxy;

namespace ff_ios_xamarin_client_sample
{
    public class CfListener : CfClientDelegate
    {
        public class MessageEventArgs : EventArgs
        {
            public CxMessage Message { get; set; }
        }
        public class EvaluationEventArgs : EventArgs
        {
            public CxEvaluation[] Evaluation { get; set; }
        }
        public class InfoEventArgs : EventArgs
        {
            public string Information { get; set; }
        }
        public class ErrorEventArgs : EventArgs
        {
            public CfErrorProxy Error { get; set; }
        }

        public CfListener()
        {
            CfClientProxy.Shared.Delegate = this;
        }

        public override void OnErrorWithError(CfErrorProxy error)
        {
            ErrorReceived?.Invoke(CfClientProxy.Shared, new ErrorEventArgs { Error = error });
        }
        public override void OnPollingEventReceivedWithEvaluations(CxEvaluation[] evaluations)
        {
            PollingReceived?.Invoke(CfClientProxy.Shared, new EvaluationEventArgs { Evaluation = evaluations });
        }
        public override void OnStreamEventReceivedWithEvaluation(CxEvaluation evaluation)
        {
            EvaluationReceived?.Invoke(CfClientProxy.Shared, new EvaluationEventArgs { Evaluation = new[] { evaluation} });
        }
        public override void OnMessageReceivedWithMessage(CxMessage message)
        {
            MessageReceived?.Invoke(CfClientProxy.Shared, new MessageEventArgs { Message = message });
        }
        public override void OnStreamOpened()
        {
            SystemMessageReceived?.Invoke(CfClientProxy.Shared, new InfoEventArgs { Information = "On Stream Opened" }) ;
        }
        public override void OnStreamCompleted()
        {
            SystemMessageReceived?.Invoke(CfClientProxy.Shared, new InfoEventArgs { Information = "On Stream Completed" });
        }
        public override void OnInitializeSuccess()
        {
            InitializeSuccess?.Invoke(CfClientProxy.Shared, EventArgs.Empty);
        }

        public event EventHandler<MessageEventArgs> MessageReceived;
        public event EventHandler<EvaluationEventArgs> EvaluationReceived;
        public event EventHandler<InfoEventArgs> SystemMessageReceived;
        public event EventHandler<ErrorEventArgs> ErrorReceived;
        public event EventHandler<EvaluationEventArgs> PollingReceived;

        public event EventHandler InitializeSuccess;
    }
}
