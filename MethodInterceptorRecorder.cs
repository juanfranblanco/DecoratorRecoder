using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Proxies;
using System.Text;
using System.Threading.Tasks;

namespace DecoratorRecorder
{
    //using RealProxy as described here http://stackoverflow.com/a/32238417
    //and here http://www.codeproject.com/Articles/229486/Generic-Dynamic-Decorator-modified
    //and here http://simpleinjector.readthedocs.org/en/latest/InterceptionExtensions.html

    public class MethodInterceptorRecorder<T> : RealProxy where T: class
    {

        private T _decorated;
        public List<RecordedMethod<T>> RecordedMethods { get; private set; }

        public MethodInterceptorRecorder(T decorated)
            : base(typeof(T))
        {
            _decorated = decorated;
            RecordedMethods = new List<RecordedMethod<T>>();
        }

        public override IMessage Invoke(IMessage msg)
        {
            var methodCall = msg as IMethodCallMessage;
            var methodInfo = methodCall.MethodBase;
            //pre call   
            var recordedMethod = new RecordedMethod<T>() { MethodBase = methodInfo, Arguments = methodCall.InArgs };
            var result = recordedMethod.ExecuteMethod(_decorated);
            //post call
            RecordedMethods.Add(recordedMethod);
        
            return new ReturnMessage(result, null, 0,
                methodCall.LogicalCallContext, methodCall);
        }

        public void ReplayMethods(T obj)
        {
            foreach(var method in RecordedMethods)
            {
                method.ExecuteMethod(obj);
            }
        }

    }


    public class RecordedMethod<T>
    {
        public MethodBase MethodBase { get; set; }
        public object[] Arguments { get; set; }

        public object ExecuteMethod(T obj)
        {
            return MethodBase.Invoke(obj, Arguments);
        }

    }
}
