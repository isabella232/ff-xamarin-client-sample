using System;
using System.Linq;
using IO.Harness.Cfsdk.Cloud.Events;
using IO.Harness.Cfsdk.Cloud.Model;
using IO.Harness.Cfsdk.Cloud.Oksse;
using IO.Harness.Cfsdk.Cloud.Oksse.Model;
using Java.Interop;
using Java.Util;
using IO.Harness.Cfsdk;

namespace ff_mobile_xamarin_client_sample.Droid
{
    public class CfListener : Java.Lang.Object, IAuthCallback, IEventsListener
    {
        public class InfoEventArgs : EventArgs
        {
            public string Information { get; set; }
        }
        public class EvaluationEventArgs : EventArgs
        {
            public IO.Harness.Cfsdk.Cloud.Core.Model.Evaluation[] Evaluation { get; set; }
        }
        public class ErrorEventArgs : EventArgs
        {
            public string Message { get; set; }
        }

        public CfListener()
        {
            CfClient.Instance.RegisterEventsListener(this);
        }

        public void AuthorizationSuccess(AuthInfo p0, AuthResult p1)
        {
            InitializationStatus?.Invoke(this, p1.Success);
            if (!p1.Success)
            {
                var message = (p1.Error == null || p1.Error.Message == null) ? "Error" : p1.Error.Message;
                ErrorReceived?.Invoke(this, new ErrorEventArgs { Message = message });
            }
        }
        public void OnEventReceived(StatusEvent p0)
        {
            var eventType = p0.EventType;

            if(StatusEvent.EVENT_TYPE.SseStart == eventType)
            { 
                SystemMessageReceived?.Invoke(this, new InfoEventArgs { Information = "On Stream Completed" });
            }
            else if( StatusEvent.EVENT_TYPE.SseEnd == eventType)
            {
                SystemMessageReceived?.Invoke(this, new InfoEventArgs { Information = "On Stream Ended" });
            }
            else if (StatusEvent.EVENT_TYPE.EvaluationChange == eventType)
            {
                Java.Lang.Object payload = p0.ExtractPayload();
                var ev = payload as IO.Harness.Cfsdk.Cloud.Core.Model.Evaluation;
                if (ev != null)
                {
                    EvaluationReceived?.Invoke( this, new EvaluationEventArgs { Evaluation = new[] { ev } });
                }
            }
            else if (StatusEvent.EVENT_TYPE.EvaluationReload == eventType)
            {
                Java.Lang.Object payload = p0.ExtractPayload();

                var t = payload.JavaCast<ArrayList>();
                var arr = t.ToEnumerable<IO.Harness.Cfsdk.Cloud.Core.Model.Evaluation>().ToArray();
                EvaluationReceived?.Invoke(this, new EvaluationEventArgs { Evaluation = arr }); ;

            }
        }

        public event EventHandler<EvaluationEventArgs> EvaluationReceived;
        public event EventHandler<bool> InitializationStatus;
        public event EventHandler<InfoEventArgs> SystemMessageReceived;
        public event EventHandler<ErrorEventArgs> ErrorReceived;

    }
}
