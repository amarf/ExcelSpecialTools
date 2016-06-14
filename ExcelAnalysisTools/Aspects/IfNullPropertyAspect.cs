using MethodDecoratorInterfaces;
using System;
using System.Reflection;

namespace ExcelAnalysisTools.Aspects
{

    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Constructor | AttributeTargets.Assembly | AttributeTargets.Module)]
    public class IfNullPropertyAspectAttribute : Attribute, IMethodDecorator
    {
        public void Init(object instance, MethodBase method, object[] args)
        {
            if (instance == null)
            {
                var type = method.DeclaringType.FullName;
                instance = Activator.CreateInstance(method.DeclaringType);
            }
        }

        public void OnEntry()
        {
            
        }

        public void OnException(Exception exception)
        {
            
        }

        public void OnExit()
        {
            
        }
    }
}
